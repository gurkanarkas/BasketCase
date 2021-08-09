using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System;
using Core.Caching.Cache;
using Core.Models.Basket;
using BasketCase.Core.Models;
using BasketCase.Core.Entities.Concrete;
using BasketCase.Business.Manager;
using BasketCase.Business.Resolver;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using BasketCaseAPI;

namespace BasketCase.Tests
{
    [TestClass]
    public class Basket
    {
        private GenericDependencyResolver _serviceProvider;

        public Basket()
        {
            var webHost = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>()
                .Build();
            _serviceProvider = new GenericDependencyResolver(webHost);
        }

        [TestMethod]
        public async Task AddBasket()
        {
            var cacheModel = new BasketRequestModel();

            cacheModel.CorrelationId = Guid.NewGuid();
            cacheModel.UserId = 1903;

            var product = new Products()
            {
                Id = 1000,
                Name = "Lezzet Meyve Dünyası",
                Price = 64,
                ProductType = Core.Entities.Enums.ProductType.Physical,
                AvailableFrom = DateTime.Now,
                AvailableTo = DateTime.Now.AddDays(15)
            };

            cacheModel.Product = product;

            var basketService = _serviceProvider.GetService<IBasketService>();
            Assert.IsNotNull(await basketService.AddBasket(cacheModel));
        }

        [TestMethod]
        public async Task GetBasket()
        {
            var cacheModel = new BasketRequestModel();

            cacheModel.CorrelationId = Guid.NewGuid();
            cacheModel.UserId = 1903;
            string key = KeyGenerator.ReturnRedisKey(cacheModel.UserId, cacheModel.CorrelationId.ToString());

            var basketService = _serviceProvider.GetService<IBasketService>();
            Assert.IsNotNull(await basketService.GetBasket(key));

        }

        [TestMethod]
        public async Task DeleteBasketItem()
        {
            var cacheModel = new BasketRequestModel();

            cacheModel.CorrelationId = Guid.NewGuid();
            cacheModel.UserId = 1903;
            string key = KeyGenerator.ReturnRedisKey(cacheModel.UserId, cacheModel.CorrelationId.ToString());

            var basketService = _serviceProvider.GetService<IBasketService>();
            Assert.IsNotNull(await basketService.DeleteBaskteItem(key, 1000));
        }
    }
}
