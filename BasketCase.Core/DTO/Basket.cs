using BasketCase.Core.Entities.Concrete;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BasketCase.Core.DTO
{
    public class Basket : IDataTransferObject
    {
        /// <summary>
        /// Sepet nesnesinin Redis'te; benzersiz bir key oluşturma, 
        /// payment sürecinde; payment transaction id olarak kullanma ve 
        /// başarılı bir ödeme işlemi sonrası sipariş oluşturma aşamalarında kullanacağız. 
        /// </summary>
        public Guid CoralationId { get; set; }
        /// <summary>
        /// Oluşan sepetin toplam bedeli. Burada ücretsiz kargo, indirim kuponu gibi öğeleri kullanırken ihtiyacımız olacak.
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// Sepet içerisindeki ürünler satın alma adedine göre listeye eklenecek. 
        /// </summary>
        public IEnumerable<BasketItem> BasketItems { get; set; }
    }
}
