using BasketCase.Core.Entities.Concrete;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasketCase.Core.DTO
{
    /// <summary>
    /// Burada şimdilik ürün olarak belirttik fakat yarın bir hizmet ile aynı sepete eklenme durumuna göre generic bir yapı oluşturulabilir.
    /// </summary>
    public class BasketItem : IDataTransferObject
    {
        /// <summary>
        /// İlgili üründen kaç adet satın alındığını belirtmek için kullanıyoruz.
        /// Buradaki Quantity sadece ürün adedini belirtiyor. Adet bazında fiyat politikası yönetimi ile ilgili değil.
        /// Bu kontrol ProductPrices gibi bir modelleme ile tanımlanabilir bir şekilde yapılabilir.
        /// Örneğin 1 Product = 10 TL, 2 Product = 15 TL gibi...
        /// </summary>
        public int Quantity { get; set; }
        /// <summary>
        /// Satın alınan ürün bilgisi.
        /// </summary>
        public Products Product { get; set; }
    }
}
