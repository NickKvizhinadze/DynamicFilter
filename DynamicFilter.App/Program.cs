using DynamicFilter.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using static DynamicFilter.App.Factories.ProductsFactory;


namespace DynamicFilter.App
{
    class Program
    {
        static void Main(string[] args)
        {
            var filters = new List<FilterModel>() {
                new FilterModel("Caption", typeof(string), new List<string>(){ "Apple", "Pear" }, "Contains"),
                new FilterModel("Price", typeof(decimal), 3, "Equal")
            };

            var queryGenerator = new QueryGenerator<Product>(filters, ProductsList.AsQueryable());
            IQueryable<Product> result;
            //Filter Parameter
            foreach (var filter in filters)
            {
                queryGenerator = (QueryGenerator<Product>)
                                typeof(QueryGenerator<Product>)
                                .GetMethod(filter.MethodName)
                                .Invoke(queryGenerator, new[] { filter });
                queryGenerator.AddFilter();
            }

            result = queryGenerator.ApplyFilter();

            foreach (var item in result.ToList())
            {
                Console.WriteLine($"Name: {item.Caption}, Description: {item.Description}, Price: {item.Price}, Quantity: {item.Quantity}");
            }
            Console.ReadKey();
        }
    }
}
