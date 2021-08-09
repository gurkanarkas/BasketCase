using BasketCase.Business.Manager;
using Core.Models.Basket;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BasketCaseAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private IBasketService basketService;

        public BasketController(IBasketService _basketService)
        {
            basketService = _basketService;
        }

        [HttpPost]
        [Route("add")]
        public async Task<BasketResponseModel> AddBasketItem(BasketRequestModel model)
            => await basketService.AddBasket(model);

        [HttpPost]
        [Route("delete")]
        public async Task<BasketResponseModel> DeleteBasketItem(string key, int productId)
            => await basketService.DeleteBaskteItem(key, productId);

        [HttpPost]
        [Route("clear")]
        public async Task<object> ClearBasket(string key)
            => await basketService.ClearBasket(key);

        [HttpGet]
        [Route("get")]
        public async Task<object> GetBasket(string key)
            => await basketService.GetBasket(key);
    }
}
