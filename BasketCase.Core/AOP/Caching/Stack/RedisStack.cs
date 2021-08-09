using System;
using System.Threading.Tasks;
using BasketCase.Core.Models;
using Core.CrossCuttingConcerns.Caching;
using Core.Models.Basket;
using Newtonsoft.Json;

namespace Core.Caching.Stack
{
    public class RedisStack : IRedisStack
    {
        public void Initialize(Configuration configuration) => RedisManager.Initialize(configuration);

        public bool IsDatabaseAvailable()
        {
            if (RedisManager.Database != null)
                return true;
            return false;
        }

        public async Task<bool> IsExist(string key) => await RedisManager.Exists(key);

        public async Task<bool> Set(BasketCacheModel cacheModel)
        {
            return await RedisManager.Set(cacheModel.Key, JsonConvert.SerializeObject(cacheModel), TimeSpan.FromMinutes(20));
        }

        public async Task<bool> Delete(string key)
        {
            return await RedisManager.Remove(key);
        }

        public async Task<BasketCacheModel> Get(string key)
        {
            var result = await RedisManager.Get(key);
            if (!string.IsNullOrEmpty(result))
            return JsonConvert.DeserializeObject<BasketCacheModel>(result);

            return null;
        }
    }
}
