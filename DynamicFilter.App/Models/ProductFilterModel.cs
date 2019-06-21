using System.Collections.Generic;
using DynamicFilter.App.Attributes;
using DynamicFilter.App.Enums;

namespace DynamicFilter.App.Models
{
    [FilterFor(typeof(Product))]
    public class ProductFilterModel
    {
        [FilterMethod(FilterMethods.Contains)]
        public List<string> Caption { get; set; }

        [FilterMethod(FilterMethods.Equal, nameof(Product.Price))]
        public decimal Price { get; set; }
    }
}
