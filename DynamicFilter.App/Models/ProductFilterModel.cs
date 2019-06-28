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

        [FilterMethod(FilterMethods.LessThanOrEqual, ConditionalOperators.And, nameof(Product.ReceiveDate))]
        public DateTime? ReceiveDateTo { get; set; }

        [FilterMethod(FilterMethods.Equal, nameof(Product.CategoryId))]
        public int CategoryId { get; set; }

        [FilterMethod(FilterMethods.Contains, ConditionalOperators.Or, nameof(Product.CategoryId))]
        public List<int> Categories { get; set; }


        [FilterMethod(FilterMethods.HasValueAndContains, nameof(Product.CardTariffId))]
        public List<int> CardTariffIds { get; set; }

        [FilterMethod(FilterMethods.HasValueAndContains, nameof(Product.CreditCategoryId))]
        public List<int> CreditCategoryIds { get; set; }

        [FilterMethod(FilterMethods.HasValueAndContains, nameof(Product.AccountTariffId))]
        public List<int> AccountTariffIds { get; set; }

        [FilterMethod(FilterMethods.IsNotNull, nameof(Product.CardTariffId))]
        [FilterMethod(FilterMethods.IsNotNull, nameof(Product.CreditCategoryId))]
        [FilterMethod(FilterMethods.IsNotNull, nameof(Product.AccountTariffId))]
        public bool? IsAllTariff { get; set; }


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

            ValidationBuilder
                .For<ProductFilterModel>()
                .Property(f => f.CardTariffIds)
                .AddValidation(x => IsAllTariff != true);

            ValidationBuilder
                .For<ProductFilterModel>()
                .Property(f => f.CreditCategoryIds)
                .AddValidation(x => IsAllTariff != true);

            ValidationBuilder
                .For<ProductFilterModel>()
                .Property(f => f.AccountTariffIds)
                .AddValidation(x => IsAllTariff != true);

            ValidationBuilder
                .For<ProductFilterModel>()
                .Property(f => f.IsAllTariff)
                .AddValidation(x => IsAllTariff == true);

            var test = _predicates;
        }
    }
}
