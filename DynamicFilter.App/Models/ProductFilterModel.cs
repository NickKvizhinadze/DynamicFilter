using System;
using System.Collections.Generic;
using DynamicFilter.Enums;
using DynamicFilter.Models;
using DynamicFilter.Attributes;
using DynamicFilter.ValidationBuilder;

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
            this.FilterBy(f => f.Price)
                .If(f => f.Price > 0);

            this.FilterBy(f => f.CategoryId)
                .If(f => f.CategoryId > 0);

            this.FilterBy(f => f.Categories)
                .If(f => f.CategoryId == 0);

            this.FilterBy(f => f.CardTariffIds)
                .If(f => f.IsAllTariff != true);

            this.FilterBy(f => f.CreditCategoryIds)
                .If(f => f.IsAllTariff != true);

            this.FilterBy(f => f.AccountTariffIds)
                .If(f => f.IsAllTariff != true);

            this.FilterBy(f => f.IsAllTariff)
                .If(f => f.IsAllTariff != true);
        }
    }
}
