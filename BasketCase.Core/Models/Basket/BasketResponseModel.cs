using BasketCase.Core.Entities.Concrete;
using Core.Models.Basket;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Models.Basket
{
    public class BasketResponseModel : GenericResponse<BasketCacheModel>
    {
        public decimal TotalPrice { get; set; }
    }
}
