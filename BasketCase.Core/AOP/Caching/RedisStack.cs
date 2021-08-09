using log4net;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Core.CrossCuttingConcerns.Caching
{
    public class RedisManager
    {
        private static readonly ILog _log = LogManager.GetLogger(typeof(RedisManager));

        internal static Lazy<ConnectionMultiplexer> LazyConnection;

        public static void Initialize(Configuration config)
        {
            ConfigurationOptions opt = new ConfigurationOptions()
            {
                AbortOnConnectFail = config.AbortOnConnectFail,
                ConnectTimeout = config.ConnectTimeout,
                SyncTimeout = config.SyncTimeout,
                Password = config.Key ?? null
            };

            opt.EndPoints.Add(config.Endpoint, config.Port);

            LazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(opt));

        }

        internal static ConnectionMultiplexer Connection
        {
            get
            {
                if (LazyConnection == null)
                    return null;
                return LazyConnection.Value;
            }
        }

        internal static IServer Server
        {
            get
            {
                try
                {
                    var endpoints = Connection.GetEndPoints();

                    var connectedEndPoints = endpoints.Where(x => Connection.GetServer(x).IsConnected);

                    if (connectedEndPoints.Count() > 1)
                    {
                        var slave = connectedEndPoints.First(endpoint => Connection.GetServer(endpoint).IsReplica);

                        return Connection.GetServer(slave);
                    }

                    return Connection.GetServer(connectedEndPoints.FirstOrDefault());
                }
                catch (Exception ex)
                {
                    _log.Error("Server Get: Unexpected error", ex);
                }
                return null;
            }
        }

        public static IDatabase Database
        {
            get
            {
                try
                {
                    if (Connection != null)
                    return Connection.GetDatabase();
                }
                catch (Exception ex)
                {
                    _log.Error("Database Get: Unexpected error", ex);
                }
                return null;
            }
        }

        public static async Task<bool> Set(string key, object value, TimeSpan? expire = null)
        {
            try
            {
                var tname = GetTableName(key);
                var timeToLive = Database.KeyTimeToLive(key);
                if (timeToLive != null)
                {
                    expire = timeToLive;
                }
                string serializedObject = null;

                if (value is string)
                {
                    serializedObject = value as string;
                }
                else
                {
                    serializedObject = JsonConvert.SerializeObject(value);
                }

                await Remove(key);
                return await Database.StringSetAsync(tname, serializedObject, expire, When.NotExists, CommandFlags.DemandMaster);
            }
            catch (Exception ex)
            {
                _log.Error("Set: Unexpected error", ex);
                return false;
            }
        }

        /// <summary>
        /// Retrieves the key's data and parse it into the model
        /// </summary>
        /// <typeparam name="T">class object</typeparam>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static async Task<T> Get<T>(string key)
        {
            try
            {
                var tname = GetTableName(key);
                var redisValue = await Database.StringGetAsync(tname);

                if (!redisValue.IsNullOrEmpty)
                {
                    return JsonConvert.DeserializeObject<T>(redisValue);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Get<T>: Unexpected error", ex);
            }
            return default(T);
        }

        /// <summary>
        /// Retrieves the key's data as a string
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static async Task<string> Get(string key)
        {
            try
            {
                var tname = GetTableName(key);

                var redisValue = await Database.StringGetAsync(tname);

                return redisValue;
            }
            catch (Exception ex)
            {
                _log.Error("Get: Unexpected error", ex);
            }
            return null;
        }

        /// <summary>
        /// Deletes key and returns true/false (boolean)
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static async Task<bool> Remove(string key)
        {
            try
            {
                var tname = GetTableName(key);
                if (await Exists(tname))
                {
                    return await Database.KeyDeleteAsync(tname);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Remove: Unexpected error", ex);
            }
            return false;
        }

        /// <summary>
        /// Returns true if key exist in Redis
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static async Task<bool> Exists(string key)
        {
            try
            {
                return await Database.KeyExistsAsync(key);
            }
            catch (Exception ex)
            {
                _log.Error("Exists: Unexpected error", ex);
            }
            return false;
        }

        private static string GetTableName(string tableName)
        {
            try
            {
                var tname = RemoveTurkish(tableName);
                return tname;
            }
            catch (Exception ex)
            {
                _log.Error("GetTableName: Unexpected error", ex);
            }
            return null;
        }

        private static string RemoveTurkish(string str)
        {
            return str
                .Replace("ğ", "g")
                .Replace("ü", "u")
                .Replace("ş", "s")
                .Replace("ı", "i")
                .Replace("ö", "o")
                .Replace("ç", "c")
                .Replace("Ğ", "G")
                .Replace("Ü", "U")
                .Replace("Ş", "S")
                .Replace("İ", "I")
                .Replace("Ö", "O")
                .Replace("Ç", "C");
        }
    }
}
