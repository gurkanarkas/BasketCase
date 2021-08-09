using Core.Caching.Stack;
using Core.CrossCuttingConcerns.Caching;
using Core.Models.Basket;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace Core.Caching.Cache
{
    public class RedisCache: IRedisCache
    {
        private readonly IRedisStack _redisStack;
        private IOptions<Configuration> _settings;
        public RedisCache(IRedisStack redisStack, IOptions<Configuration> settings)
        {
            _settings = settings;
            _redisStack = redisStack;
            if (!_redisStack.IsDatabaseAvailable())
            {
                var configuration = new Configuration
                {
                    Endpoint = _settings.Value.Endpoint,
                    Key = _settings.Value.Key,
                    Port = _settings.Value.Port,
                };
                _redisStack.Initialize(configuration);
            }
        }

        public async Task<bool> IsExist(string key) => await _redisStack.IsExist(key);
        public async Task<bool> Set(BasketCacheModel cacheModel) => await _redisStack.Set(cacheModel);
        public async Task<bool> Delete(string key) => await _redisStack.Delete(key);
        public async Task<BasketCacheModel> Get(string key) => await _redisStack.Get(key);
    }
}
