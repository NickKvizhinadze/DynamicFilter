using System;
using System.Collections.Generic;
using DynamicFilter.App.Models;

namespace DynamicFilter.App.Factories
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
                        ReceiveDate = new DateTime(2019, 05, 06)
                    },
                    new Product {
                        Id = 2,
                        Caption = "Pear",
                        Description = "Golden Pear",
                        Price = 2.3M,
                        Quantity = 25,
                        ReceiveDate = new DateTime(2019, 05, 15)
                    },
                    new Product {
                        Id = 3,
                        Caption = "Orange",
                        Description = "Georgian Orange",
                        Price = 1.25M,
                        Quantity = 18,
                        ReceiveDate = new DateTime(2019, 05, 15)
                    },
                    new Product {
                        Id = 4,
                        Caption = "Tangerine",
                        Description = "Foreign Tangerine",
                        Price = 1.8M,
                        Quantity = 5,
                        ReceiveDate = new DateTime(2019, 05, 21)
                    },
                    new Product {
                        Id = 1,
                        Caption = "Apple",
                        Description = "Winter Apple",
                        Price = 3,
                        Quantity = 10,
                        ReceiveDate = new DateTime(2019, 05, 30)
                    },
                };
            }
        }
    }
}
