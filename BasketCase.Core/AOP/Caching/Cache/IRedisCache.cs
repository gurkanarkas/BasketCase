using System.Threading.Tasks;
using Core.Models.Basket;

namespace Core.Caching.Cache
{
    public interface IRedisCache
    {
        Task<bool> Set(BasketCacheModel cacheModel);
        Task<bool> IsExist(string key);
        Task<bool> Delete(string key);
        Task<BasketCacheModel> Get(string key);
    }
}
