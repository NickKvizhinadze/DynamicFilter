using System;
using System.Collections.Generic;
using DynamicFilter.Test.Models;

namespace DynamicFilter.Test.Factories
{
    public static class ProductsFactory
    {
        public static List<Product> ProductsList
        {
            get
            {
                return new List<Product>()
                {
                    new Product {
                        Id = 1,
                        Caption = "Apple",
                        Description = "Green Apple",
                        Price = 3,
                        Quantity = 15,
                        ReceiveDate = new DateTime(2019, 05, 06),
                        CategoryId = 1,
                        CardTariffId = 1,
                        AccountTariffId = 2,
                        CreditCategoryId = 3
                    },
                    new Product {
                        Id = 2,
                        Caption = "Pear",
                        Description = "Golden Pear",
                        Price = 2.3M,
                        Quantity = 25,
                        ReceiveDate = new DateTime(2019, 05, 15),
                        CategoryId = 2,
                        CardTariffId = 2,
                        AccountTariffId = 3,
                        CreditCategoryId = 4
                    },
                    new Product {
                        Id = 3,
                        Caption = "Orange",
                        Description = "Georgian Orange",
                        Price = 1.25M,
                        Quantity = 18,
                        ReceiveDate = new DateTime(2019, 05, 15),
                        CategoryId = 3,
                        CardTariffId = null,
                        AccountTariffId = 4,
                        CreditCategoryId = 5
                    },
                    new Product {
                        Id = 4,
                        Caption = "Tangerine",
                        Description = "Foreign Tangerine",
                        Price = 1.8M,
                        Quantity = 5,
                        ReceiveDate = new DateTime(2019, 05, 21),
                        CategoryId = 4,
                        CardTariffId = 6,
                        AccountTariffId = 7,
                        CreditCategoryId = 5
                    },
                    new Product {
                        Id = 1,
                        Caption = "Apple",
                        Description = "Winter Apple",
                        Price = 3,
                        Quantity = 10,
                        ReceiveDate = new DateTime(2019, 05, 30),
                        CategoryId = 5,
                        CardTariffId = 9,
                        AccountTariffId = null,
                        CreditCategoryId = 14
                    },
                };
            }
        }
    }
}
