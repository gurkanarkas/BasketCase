using Core.CrossCuttingConcerns.Caching;
using Core.Models.Basket;
using System.Threading.Tasks;
namespace Core.Caching.Stack
{
    public interface IRedisStack
    {
        void Initialize(Configuration configuration);
        bool IsDatabaseAvailable();
        Task<bool> Set(BasketCacheModel cacheModel);
        Task<bool> IsExist(string key);
        Task<bool> Delete(string key);
        Task<BasketCacheModel> Get(string key);
    }
}
