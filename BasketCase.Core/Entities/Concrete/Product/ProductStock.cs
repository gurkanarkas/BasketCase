using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasketCase.Core.Entities.Concrete
{
    /// <summary>
    /// Sadece modellemek amacıyla yazdım. Herhangi bir db operasyonu gerçekleşmeyecek.
    /// </summary>
    public class ProductStock : IEntity
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        /*
         * Another Product Stock Entity Fields...
         */
    }
}
