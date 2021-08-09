using BasketCase.Core.Entities.Concrete;
using System;
using System.Collections.Generic;

namespace Core.Models.Basket
{
    public class BasketCacheModel
    {
        /// <summary>
        /// Tüm transaction'u bu id ile yöneteceğiz. Redis tarafında key de bu id den üretilecek.
        /// </summary>
        public Guid CorrelationId { get; set; }
        public string Key { get; set; }
        public List<Products> Products { get; set; }
        public int UserId { get; set; }

        public BasketCacheModel()
        {
            Products = new List<Products>();
        }
    }
}
