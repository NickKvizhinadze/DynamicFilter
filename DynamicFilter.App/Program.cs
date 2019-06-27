using DynamicFilter.App.Models;
using DynamicFilter.Helpers;
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
            //Product filter model
            var productFilter = new ProductFilterModel
            {
                //Captions = new List<string>() { "Apple", "Pear" },
                //Price = 3,
                ReceiveDateFrom = new DateTime(2019, 05, 07),
                ReceiveDateTo = new DateTime(2019, 07, 07),
                CategoryId = 1,
                Categories = new List<int> { 3, 4 }
            };

            //Filter data
            IQueryable<Product> result = FilterHelper.Filter(productFilter, ProductsList.AsQueryable());


            //Show filtered data
            foreach (var item in result.ToList())
            {
                Console.WriteLine($"Name: {item.Caption}, Description: {item.Description}, Price: {item.Price}, Quantity: {item.Quantity}, ReceiveDate: {item.ReceiveDate}, CategoryId: {item.CategoryId}");
            }


            Console.ReadKey();
        }
    }
}
