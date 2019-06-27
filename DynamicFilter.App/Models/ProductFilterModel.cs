using System;
using System.Collections.Generic;
using DynamicFilter.Enums;
using DynamicFilter.Models;
using DynamicFilter.Attributes;

namespace DynamicFilter.App.Models
{
    [FilterFor(typeof(Product))]
    public class ProductFilterModel : BaseFilter
    {
        [FilterMethod(FilterMethods.Contains, nameof(Product.Caption))]
        public List<string> Captions { get; set; }

        [FilterMethod(FilterMethods.Equal)]
        public decimal? Price { get; set; }

        [FilterMethod(FilterMethods.GreaterThanOrEqual, nameof(Product.ReceiveDate))]
        public DateTime? ReceiveDateFrom { get; set; }

        [FilterMethod(FilterMethods.LessThanOrEqual, nameof(Product.ReceiveDate))]
        public DateTime? ReceiveDateTo { get; set; }

        [FilterMethod(FilterMethods.Equal, nameof(Product.CategoryId))]
        public int CategoryId { get; set; }

        [FilterMethod(FilterMethods.Contains, nameof(Product.CategoryId))]
        public List<int> Categories { get; set; }


        public override void Configure()
        {
            ValidationBuilder
                .For<ProductFilterModel>()
                .Property(f => f.Price)
                .AddValidation(x => x != null && (decimal)x > 0);

            ValidationBuilder
                .For<ProductFilterModel>()
                .Property(f => f.CategoryId)
                .AddValidation(x => (int)x > 0);

            ValidationBuilder
                .For<ProductFilterModel>()
                .Property(f => f.Categories)
                .AddValidation(x => CategoryId == 0);

            var test = _predicates;
        }
    }
}
