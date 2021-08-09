using BasketCase.Core.Entities.Enums;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasketCase.Core.Entities.Concrete
{
    /// <summary>
    /// Sadece modellemek amacıyla yazdım. Herhangi bir db operasyonu gerçekleşmeyecek.
    /// </summary>
    public class Products : IEntity
    {
        public int Id { get; set; }
        /// <summary>
        /// Ürün/Hizmet adı
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Bir ürünün stoklu mu yoksa stoksuz mu olduğunu tipine göre ayırt edeceğiz. 
        /// Ben örnek olarak Physical ve Virtual olarak ayırdım. 
        /// Virtual tipini stoksuz bir hizmet olarak düşünebiliriz.
        /// </summary>
        public ProductType ProductType { get; set; }
        /// <summary>
        /// Ürün/Hizmetin satışa açılma tarihi
        /// </summary>
        public DateTime AvailableTo { get; set; }
        /// <summary>
        /// Ürün/Hizmetin satışa kapatılma tarihi
        /// </summary>
        public DateTime AvailableFrom { get; set; }
        /// <summary>
        /// Ürün/Hizmet fiyat bilgisi
        /// </summary>
        public decimal Price { get; set; }

        /*
         * Another Product Entity Fields...
         */
    }
}
