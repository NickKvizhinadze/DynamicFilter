using DynamicFilter.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static DynamicFilter.App.Factories.ProductsFactory;


namespace DynamicFilter.App
{
    class Program
    {
        static void Main(string[] args)
        {
            //Fitler Model Generator
            var productFilter = new ProductFilterModel
            {
                Caption = new List<string>() { "Apple", "Pear" },
                Price = 3
            };

            var filterGenerator = new FilterModelGenerator();
            filterGenerator.GenerateFilterModel(productFilter);


            //Query Generator

            var queryGenerator = new QueryGenerator<Product>();
            IQueryable<Product> result;

            //Filter Parameter
            foreach (var filter in filterGenerator.Filters)
            {
                queryGenerator = (QueryGenerator<Product>)
                                typeof(QueryGenerator<Product>)
                                .GetMethod(filter.MethodName)
                                .Invoke(queryGenerator, new[] { filter });

                queryGenerator.AddFilter();
            }

            result = queryGenerator.ApplyFilter(ProductsList.AsQueryable());

            foreach (var item in result.ToList())
            {
                Console.WriteLine($"Name: {item.Caption}, Description: {item.Description}, Price: {item.Price}, Quantity: {item.Quantity}");
            }
            Console.ReadKey();
        }
    }
}
