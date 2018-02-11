using System;
using System.Collections.Generic;
using System.Linq;
using Orm.Practice.Entities;
using Xunit;
using Xunit.Abstractions;

namespace Orm.Practice
{
    public class ManyToManyQueryFacts : OrmFactBase
    {
        public ManyToManyQueryFacts(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void should_get_product_by_store()
        {
            Console.WriteLine(1);
            List<Store> stores = Session.Query<Store>()
                .Where(p => p.IsForQuery)
                .OrderBy(p => p.Name)
                .ToList();

            Console.WriteLine(2);
            Store firstStore = stores.First();

            Console.WriteLine(3);
            IList<Product> productsFormFirstStore = firstStore.Products;

            Console.WriteLine(4);
            IOrderedEnumerable<string> orderedEnumerable = productsFormFirstStore.Select(c => c.Name).OrderBy(n => n);

            Console.WriteLine(5);
            Assert.Equal(new[] {"product-query-1", "product-query-2"}, orderedEnumerable);

            Console.WriteLine(6);
            Assert.Equal(
                new[] {"product-query-2", "product-query-3"},
                stores.Last().Products.Select(c => c.Name).OrderBy(n => n));
        }

        [Fact]
        public void should_get_store_by_product()
        {
            Console.WriteLine(1);
            List<Product> products = Session.Query<Product>()
                .Where(p => p.IsForQuery)
                .OrderBy(p => p.Name)
                .ToList();

            Assert.Equal(3, products.Count);

            Console.WriteLine(2);
            Product secondProduct = products[1];

            Console.WriteLine(3);
            IList<Store> storesFromSecondProduct = secondProduct.StoresStockedIn;

            Console.WriteLine(4);
            IOrderedEnumerable<string> orderedEnumerable = storesFromSecondProduct.Select(c => c.Name).OrderBy(n => n);

            Console.WriteLine(5);
            Assert.Equal(new[] { "store-query-1", "store-query-2" }, orderedEnumerable);

            Console.WriteLine(6);
            Assert.Equal(
                new[] {"store-query-1"},
                products.First().StoresStockedIn.Select(c => c.Name).OrderBy(n => n));
            Assert.Equal(
                new[] {"store-query-2"},
                products.Last().StoresStockedIn.Select(c => c.Name).OrderBy(n => n));
        }
    }
}