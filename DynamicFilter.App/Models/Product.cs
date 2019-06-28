using System;

namespace DynamicFilter.App.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Caption { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public DateTime? ReceiveDate { get; set; }

        public int CategoryId { get; set; }
        public int? CardTariffId { get; set; }
        public int? CreditCategoryId { get; set; }
        public int? AccountTariffId { get; set; }
    }
}
