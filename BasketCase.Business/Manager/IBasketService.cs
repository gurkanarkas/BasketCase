using Core.Models.Basket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BasketCase.Business.Manager
{
    public interface IBasketService
    {
        Task<BasketResponseModel> AddBasket(BasketRequestModel model);
        Task<BasketResponseModel> GetBasket(string key);
        Task<BasketResponseModel> DeleteBaskteItem(string key, int productId);
        Task<BasketResponseModel> ClearBasket(string key);
    }
}
