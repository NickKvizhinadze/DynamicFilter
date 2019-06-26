using System;
using System.Collections.Generic;
using DynamicFilter.Attributes;
using DynamicFilter.Enums;

namespace DynamicFilter.App.Models
{
    [FilterFor(typeof(Product))]
    public class ProductFilterModel
    {
        [FilterMethod(FilterMethods.Contains, nameof(Product.Caption))]
        public List<string> Captions { get; set; }

        [FilterMethod(FilterMethods.Equal)]
        public decimal? Price { get; set; }

        [FilterMethod(FilterMethods.GreaterThanOrEqual, nameof(Product.ReceiveDate))]
        public DateTime? ReceiveDateFrom { get; set; }

        [FilterMethod(FilterMethods.LessThanOrEqual, nameof(Product.ReceiveDate))]
        public DateTime? ReceiveDateTo { get; set; }
    }
}
