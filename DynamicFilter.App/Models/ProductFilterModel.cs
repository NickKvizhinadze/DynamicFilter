using System.Collections.Generic;
using DynamicFilter.App.Attributes;

namespace DynamicFilter.App.Models
{
    [FilterFor(typeof(Product))]
    public class ProductFilterModel
    {
        [FilterMethod("Contains")]
        public List<string> Caption { get; set; }

        [FilterMethod("Equal", "Price")]
        public decimal Price { get; set; }
    }
}
