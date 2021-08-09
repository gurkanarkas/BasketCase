using BasketCase.Core.Models;
using Core.Caching.Cache;
using Core.Models.Basket;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BasketCase.Business.Manager
{
    public class BasketManager : IBasketService
    {
        private readonly IRedisCache redisCache;

        public BasketManager(IRedisCache _redisCache)
        {
            redisCache = _redisCache;
        }

        public async Task<BasketResponseModel> AddBasket(BasketRequestModel model)
        {
            var response = new BasketResponseModel();
            if (model == null || model.Product == null)
                throw new Exception("Error: Check your basket items.");

            var cacheModel = new BasketCacheModel();

            if (String.IsNullOrEmpty(model.CorrelationId.ToString()))
                model.CorrelationId = Guid.NewGuid();

            //var getUnStokables = cacheModel.Products.Where(x => x.ProductType == ProductType.Virtual).ToList(); // Stoklanabilir fiziksel ürünleri getiriyoruz.
            // Örnek amacıyla stok kontrolü koydum. Normalde product üzerinden yönetmemiz gerekli. %10 ihtimalle stoka takılmasını istiyoruz.  
            //  if (getUnStokables!= null && getUnStokables.Count > 0 && new Random().Next(1, 100) < 10) 
            //  {
            //  string prodName = getUnStokables[0].Name;
            // getUnStokables.RemoveAt(0); // Yine örnek amacıyla eklediği ürünü cache'e atmamak için listedeki ilk ürünü siliyoruz. Bu ürünün stok dışı olduğunu varsayıyoruz.
            int rate = new Random().Next(1, 100);

            if (rate < 10)
            {
                response.Message = $@"{model.Product.Name} ürünü stokta bulunmadığı için sepete eklenemiyor.";
                response.StatusCode = System.Net.HttpStatusCode.OK;
                return response;
            }

            //  }
            cacheModel.CorrelationId = model.CorrelationId;
            cacheModel.Key = KeyGenerator.ReturnRedisKey(model.UserId, model.CorrelationId.ToString());
            // Eğer zaten var olan bir sepette günceleme, silme işlemi yapılıyorsa, sepeti boşaltıp modelden gelen ürünler ile yeniden oluşturuyoruz.
            if (await redisCache.IsExist(cacheModel.Key))
            {
                cacheModel = await redisCache.Get(cacheModel.Key);
                await redisCache.Delete(cacheModel.Key);
            }

            var checkProduct = cacheModel.Products.Where(x => x.Id == model.Product.Id).FirstOrDefault();
            if (checkProduct != null)
                checkProduct.Price += checkProduct.Price;


            cacheModel.Products.Add(model.Product);
            cacheModel.UserId = model.UserId;

            await redisCache.Set(cacheModel); //20 dakika boyunca sepeti rediste ayakta tutuyoruz. TTL=20

            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Model = cacheModel;
            response.TotalPrice = cacheModel.Products.Sum(x => x.Price);
            return response;
        }

        public async Task<BasketResponseModel> ClearBasket(string key)
        {
            var response = new BasketResponseModel();

            if (string.IsNullOrEmpty(key))
            {
                response.Message = "Hatalı sepet bilgisi.";
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return response;
            }
            else
            {
                var basket = await redisCache.Delete(key);

                response.Message = basket ? "Sepetiniz boşaltılmıştır." : "Hatalı sepet bilgisi.";
                response.StatusCode = basket ? System.Net.HttpStatusCode.OK : System.Net.HttpStatusCode.BadRequest;
            }

            return response;
        }


        public async Task<BasketResponseModel> DeleteBaskteItem(string key, int productId)
        {
            var response = new BasketResponseModel();

            if (!string.IsNullOrEmpty(key) || productId > 0)
            {
                var getBasketItems = await redisCache.Get(key);
                if (getBasketItems != null)
                {
                    var getDeletedItem = getBasketItems.Products.Where(x => x.Id == productId).FirstOrDefault();
                    if (getDeletedItem != null)
                    {
                        getBasketItems.Products.Remove(getDeletedItem);
                        await redisCache.Delete(key);
                        await redisCache.Set(getBasketItems);

                        response.Model = getBasketItems;
                        response.StatusCode = System.Net.HttpStatusCode.OK;
                        response.TotalPrice = getBasketItems.Products.Sum(x => x.Price);
                    }
                }
            }
            return response;
        }

        public async Task<BasketResponseModel> GetBasket(string key)
        {
            var response = new BasketResponseModel();

            if (string.IsNullOrEmpty(key))
            {
                response.Message = "Invalid basket id.";
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;
                return response;
            }
            else
            {
                var basket = await redisCache.Get(key);
                if (basket != null)
                    response.Model = basket;
                else
                    response.Message = "Sepetinizde ürün bulunmamaktadır.";
                response.StatusCode = System.Net.HttpStatusCode.OK;
                response.TotalPrice = basket != null ? basket.Products.Sum(x => x.Price) : 0;
            }

            return response;
        }

    }
}
