using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicFilter.Helpers;
using DynamicFilter.Test.Models;
using static DynamicFilter.Test.Factories.ProductsFactory;

namespace DynamicFilter.Test
{
    [TestClass]
    public class FilterTest
    {
        [TestMethod]
        public void BasicFilterTest_ReturnedArrayCountShouldBeOne()
        { 
            //Product filter model
            var productFilter = new ProductFilterModel
            {
                Captions = new List<string>() { "Apple", "Pear" },
                Price = 3,
                ReceiveDateFrom = new DateTime(2019, 05, 04),
                ReceiveDateTo = new DateTime(2019, 07, 07),
                CategoryId = 1
            };

            //Filter data
            List<Product> result = FilterHelper.Filter(productFilter, ProductsList.AsQueryable()).ToList();

            //assert
            Assert.AreEqual(result.Count(), 1);
        }

        [TestMethod]
        public void OverloadFilterPropertiesTest_ReturnedArrayCountShouldBeTwo()
        {
            //Product filter model
            var productFilter = new ProductFilterModel
            {
                CategoryId = 1,
                Categories = new List<int> { 3, 4 },
            };

            //Filter data
            List<Product> result = FilterHelper.Filter(productFilter, ProductsList.AsQueryable()).ToList();

            //assert
            Assert.AreEqual(result.Count(), 2);
        }
    }
}
