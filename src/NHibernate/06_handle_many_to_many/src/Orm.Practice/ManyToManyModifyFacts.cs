using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using Orm.Practice.Entities;
using Xunit;
using Xunit.Abstractions;

namespace Orm.Practice
{
    public class ManyToManyModifyFacts : OrmFactBase
    {
        public ManyToManyModifyFacts(ITestOutputHelper output) : base(output)
        {
            ExecuteNonQuery("delete from store_product where ProductID in (select ProductID from products where IsForQuery=0) or StoreID in (select StoreID from stores where IsForQuery=0)");
            ExecuteNonQuery("DELETE FROM [dbo].[stores] WHERE IsForQuery=0");
            ExecuteNonQuery("DELETE FROM [dbo].[products] WHERE IsForQuery=0");
        }

        [Fact]
        public void should_create_product_when_create_store()
        {
            var nqStore1 = new Store
            {
                Name = "nq-store1",
                IsForQuery = false
            };

            var nqStore2 = new Store
            {
                Name = "nq-store2",
                IsForQuery = false
            };

            var nqProduct1 = new Product
            {
                Name = "nq-product1",
                IsForQuery = false
            };

            var nqProduct2 = new Product
            {
                Name = "nq-product2",
                IsForQuery = false
            };

            var nqProduct3 = new Product
            {
                Name = "nq-product3",
                IsForQuery = false
            };

            nqStore1.AddProduct(nqProduct1);
            nqStore1.AddProduct(nqProduct2);

            nqStore2.AddProduct(nqProduct2);
            nqStore2.AddProduct(nqProduct3);

            Session.Save(nqStore1);
            Session.Save(nqStore2);
            Session.Flush();
            Session.Clear();

            List<Store> stores = Session.Query<Store>()
                .Fetch(p => p.Products)
                .Where(p => !p.IsForQuery)
                .OrderBy(s => s.Name)
                .ToList();
            Assert.Equal(
                new[] { "nq-product1", "nq-product2" },
                stores.First().Products.Select(c => c.Name).OrderBy(n => n));
            Assert.Equal(
                new[] { "nq-product2", "nq-product3" },
                stores.Last().Products.Select(c => c.Name).OrderBy(n => n));

            List<Product> products = Session.Query<Product>()
               .Fetch(p => p.StoresStockedIn)
               .Where(p => !p.IsForQuery)
               .OrderBy(s => s.Name)
               .ToList();

            Assert.Equal(3, products.Count);
            Assert.Equal(
                new[] { "nq-store1" },
                products[0].StoresStockedIn.Select(c => c.Name).OrderBy(n => n));
            Assert.Equal(
                new[] { "nq-store1", "nq-store2" },
                products[1].StoresStockedIn.Select(c => c.Name).OrderBy(n => n));
            Assert.Equal(
               new[] { "nq-store2" },
               products[2].StoresStockedIn.Select(c => c.Name).OrderBy(n => n));
        }

        [Fact]
        public void should_create_store_when_create_product()
        {
            var nqProduct1 = new Product
            {
                Name = "nq-product1",
                IsForQuery = false
            };

            var nqProduct2 = new Product
            {
                Name = "nq-product2",
                IsForQuery = false
            };

            var nqProduct3 = new Product
            {
                Name = "nq-product3",
                IsForQuery = false
            };

            var nqStore1 = new Store
            {
                Name = "nq-store1",
                IsForQuery = false
            };

            var nqStore2 = new Store
            {
                Name = "nq-store2",
                IsForQuery = false
            };


            nqProduct1.StockedIn(nqStore1);

            nqProduct2.StockedIn(nqStore1);
            nqProduct2.StockedIn(nqStore2);

            nqProduct3.StockedIn(nqStore2);

            Session.Save(nqProduct1);
            Session.Save(nqProduct2);
            Session.Save(nqProduct3);
            Session.Flush();
            Session.Clear();

            List<Store> stores = Session.Query<Store>()
                .Fetch(p => p.Products)
                .Where(p => !p.IsForQuery)
                .OrderBy(s => s.Name)
                .ToList();
            Assert.Equal(
                new[] { "nq-product1", "nq-product2" },
                stores.First().Products.Select(c => c.Name).OrderBy(n => n));

            Assert.Equal(
                new[] { "nq-product2", "nq-product3" },
                stores.Last().Products.Select(c => c.Name).OrderBy(n => n));

            List<Product> products = Session.Query<Product>()
               .Fetch(p => p.StoresStockedIn)
               .Where(p => !p.IsForQuery)
               .OrderBy(s => s.Name)
               .ToList();
            Assert.Equal(3, products.Count);
            Assert.Equal(
                new[] { "nq-store1" },
                products[0].StoresStockedIn.Select(c => c.Name).OrderBy(n => n));
            Assert.Equal(
                new[] { "nq-store1", "nq-store2" },
                products[1].StoresStockedIn.Select(c => c.Name).OrderBy(n => n));
            Assert.Equal(
               new[] { "nq-store2" },
               products[2].StoresStockedIn.Select(c => c.Name).OrderBy(n => n));
        }

        [Fact]
        public void should_create_store_and_product()
        {
            var nqStore1 = new Store
            {
                Name = "nq-store1",
                IsForQuery = false
            };

            var nqStore2 = new Store
            {
                Name = "nq-store2",
                IsForQuery = false
            };

            var nqProduct1 = new Product
            {
                Name = "nq-product1",
                IsForQuery = false
            };

            var nqProduct2 = new Product
            {
                Name = "nq-product2",
                IsForQuery = false
            };

            var nqProduct3 = new Product
            {
                Name = "nq-product3",
                IsForQuery = false
            };

            nqStore1.AddProduct(nqProduct1);
            nqProduct2.StockedIn(nqStore1);
            nqProduct2.StockedIn(nqStore2);
            nqStore2.AddProduct(nqProduct3);

            Session.Save(nqStore1);
            Session.Save(nqProduct2);
            Session.Flush();
            Session.Clear();

            List<Store> stores = Session.Query<Store>()
                .Fetch(p => p.Products)
                .Where(p => !p.IsForQuery)
                .OrderBy(s => s.Name)
                .ToList();
            Assert.Equal(
                new[] { "nq-product1", "nq-product2" },
                stores.First().Products.Select(c => c.Name).OrderBy(n => n));

            Assert.Equal(
                new[] { "nq-product2", "nq-product3" },
                stores.Last().Products.Select(c => c.Name).OrderBy(n => n));

            List<Product> products = Session.Query<Product>()
                .Fetch(p => p.StoresStockedIn)
                .Where(p => !p.IsForQuery)
                .OrderBy(s => s.Name)
                .ToList();
            Assert.Equal(3, products.Count);
            Assert.Equal(
                new[] { "nq-store1" },
                products[0].StoresStockedIn.Select(c => c.Name).OrderBy(n => n));
            Assert.Equal(
                new[] { "nq-store1", "nq-store2" },
                products[1].StoresStockedIn.Select(c => c.Name).OrderBy(n => n));
            Assert.Equal(
                new[] { "nq-store2" },
                products[2].StoresStockedIn.Select(c => c.Name).OrderBy(n => n));
        }

        [Fact]
        public void should_update_store_or_product()
        {
            var nqStore1 = new Store
            {
                Name = "nq-store1",
                IsForQuery = false
            };

            var nqProduct1 = new Product
            {
                Name = "nq-product1",
                IsForQuery = false
            };

            Console.WriteLine(1);
            nqStore1.AddProduct(nqProduct1);
            Session.Save(nqStore1);
            Session.Flush();
            Session.Clear();


            var nqStore2 = new Store
            {
                Name = "nq-store2",
                IsForQuery = false
            };

            var nqProduct2 = new Product
            {
                Name = "nq-product2",
                IsForQuery = false
            };

            var nqProduct3 = new Product
            {
                Name = "nq-product3",
                IsForQuery = false
            };

            Console.WriteLine(2);
            Store firstStore = Session.Query<Store>().Fetch(s => s.Products).Single(s => !s.IsForQuery);
            firstStore.AddProduct(nqProduct2);
            nqProduct2.StockedIn(nqStore2);
            nqStore2.AddProduct(nqProduct3);
            Session.Update(firstStore);
            Session.Flush();
            Session.Clear();

            Console.WriteLine(3);
            List<Store> stores = Session.Query<Store>()
                .Fetch(p => p.Products)
                .Where(p => !p.IsForQuery)
                .OrderBy(s => s.Name)
                .ToList();
            Assert.Equal(
                new[] { "nq-product1", "nq-product2" },
                stores.First().Products.Select(c => c.Name).OrderBy(n => n));

            Assert.Equal(
                new[] { "nq-product2", "nq-product3" },
                stores.Last().Products.Select(c => c.Name).OrderBy(n => n));

            List<Product> products = Session.Query<Product>()
                .Fetch(p => p.StoresStockedIn)
                .Where(p => !p.IsForQuery)
                .OrderBy(s => s.Name)
                .ToList();
            Assert.Equal(3, products.Count);
            Assert.Equal(
                new[] { "nq-store1" },
                products[0].StoresStockedIn.Select(c => c.Name).OrderBy(n => n));
            Assert.Equal(
                new[] { "nq-store1", "nq-store2" },
                products[1].StoresStockedIn.Select(c => c.Name).OrderBy(n => n));
            Assert.Equal(
                new[] { "nq-store2" },
                products[2].StoresStockedIn.Select(c => c.Name).OrderBy(n => n));
        }

        [Fact]
        public void should_delete_product_when_removed_from_store()
        {
            var nqStore1 = new Store
            {
                Name = "nq-store1",
                IsForQuery = false
            };

            var nqStore2 = new Store
            {
                Name = "nq-store2",
                IsForQuery = false
            };

            var nqProduct1 = new Product
            {
                Name = "nq-product1",
                IsForQuery = false
            };

            var nqProduct2 = new Product
            {
                Name = "nq-product2",
                IsForQuery = false
            };

            var nqProduct3 = new Product
            {
                Name = "nq-product3",
                IsForQuery = false
            };

            nqStore1.AddProduct(nqProduct1);
            nqStore1.AddProduct(nqProduct2);
            nqStore2.AddProduct(nqProduct2);
            nqStore2.AddProduct(nqProduct3);
            Session.Save(nqStore1);
            Session.Save(nqStore2);
            Session.Flush();
            Session.Clear();

            List<Store> stores = Session.Query<Store>().Fetch(s => s.Products).Where(s => !s.IsForQuery).OrderBy(s => s.Name).ToList();
            var firstStore = stores.First();
            Product firstProduct = firstStore.Products.Single(p => p.Name == "nq-product1");
            firstStore.Products.Remove(firstProduct);
            Console.WriteLine(1);
            Session.Update(firstStore);
            Session.Flush();
            Session.Clear();

            Console.WriteLine(2);
            List<Store> storesAfterUpdated = Session.Query<Store>().Fetch(s => s.Products).Where(s => !s.IsForQuery).OrderBy(s => s.Name).ToList();
            Assert.Equal(2, storesAfterUpdated.Count);
            Assert.Equal(1, storesAfterUpdated[0].Products.Count);
            Assert.Equal(2, storesAfterUpdated[1].Products.Count);

            List<Product> productsAfterUpdated = Session.Query<Product>().Fetch(s => s.StoresStockedIn).Where(s => !s.IsForQuery).OrderBy(s => s.Name).ToList();
            Assert.Equal(3, productsAfterUpdated.Count);
            Assert.Equal(0, productsAfterUpdated[0].StoresStockedIn.Count);
            Assert.Equal(2, productsAfterUpdated[1].StoresStockedIn.Count);
            Assert.Equal(1, productsAfterUpdated[2].StoresStockedIn.Count);

            Session.Clear();
            List<Product> products = Session.Query<Product>().Fetch(s => s.StoresStockedIn).Where(s => !s.IsForQuery).OrderBy(s => s.Name).ToList();
            products[1].StoresStockedIn.Clear();
            Console.WriteLine(3);
            Session.Update(products[1]);
            Session.Flush();
            Session.Clear();

            storesAfterUpdated = Session.Query<Store>().Fetch(s => s.Products).Where(s => !s.IsForQuery).OrderBy(s => s.Name).ToList();
            Assert.Equal(2, storesAfterUpdated.Count);
            Assert.Equal(0, storesAfterUpdated[0].Products.Count);
            Assert.Equal(1, storesAfterUpdated[1].Products.Count);

            productsAfterUpdated = Session.Query<Product>().Fetch(s => s.StoresStockedIn).Where(s => !s.IsForQuery).OrderBy(s => s.Name).ToList();
            Assert.Equal(3, productsAfterUpdated.Count);
            Assert.Equal(0, productsAfterUpdated[0].StoresStockedIn.Count);
            Assert.Equal(0, productsAfterUpdated[1].StoresStockedIn.Count);
            Assert.Equal(1, productsAfterUpdated[2].StoresStockedIn.Count);

        }

        [Fact]
        public void should_delete_in_a_cascade_manner()
        {
            var nqStore1 = new Store
            {
                Name = "nq-store1",
                IsForQuery = false
            };

            var nqStore2 = new Store
            {
                Name = "nq-store2",
                IsForQuery = false
            };

            var nqProduct1 = new Product
            {
                Name = "nq-product1",
                IsForQuery = false
            };

            var nqProduct2 = new Product
            {
                Name = "nq-product2",
                IsForQuery = false
            };

            var nqProduct3 = new Product
            {
                Name = "nq-product3",
                IsForQuery = false
            };

            nqStore1.AddProduct(nqProduct1);
            nqStore1.AddProduct(nqProduct2);

            nqStore2.AddProduct(nqProduct2);
            nqStore2.AddProduct(nqProduct3);

            Session.Save(nqStore1);
            Session.Save(nqStore2);
            Session.Flush();
            Session.Clear();

            List<Store> stores = Session.Query<Store>().Fetch(s => s.Products).Where(s => !s.IsForQuery).OrderBy(s => s.Name).ToList();
            Session.Delete(stores.First());
            Session.Flush();
            Session.Clear();

            List<Store> storesAfterDeleted = Session.Query<Store>().Fetch(s => s.Products).Where(s => !s.IsForQuery).OrderBy(s => s.Name).ToList();
            Assert.Equal(0, storesAfterDeleted.Count);
            List<Product> productsAfterDeleted = Session.Query<Product>().Fetch(s => s.StoresStockedIn).Where(s => !s.IsForQuery).OrderBy(s => s.Name).ToList();
            Assert.Equal(0, productsAfterDeleted.Count);
        }

        [Fact(Skip = "for some reason")]
        public void should_not_delete_product_when_delete_store()//all saveorupdate non empty
        {
            var nqStore1 = new Store
            {
                Name = "nq-store1",
                IsForQuery = false
            };

            var nqStore2 = new Store
            {
                Name = "nq-store2",
                IsForQuery = false
            };

            var nqProduct1 = new Product
            {
                Name = "nq-product1",
                IsForQuery = false
            };

            var nqProduct2 = new Product
            {
                Name = "nq-product2",
                IsForQuery = false
            };

            var nqProduct3 = new Product
            {
                Name = "nq-product3",
                IsForQuery = false
            };

            nqStore1.AddProduct(nqProduct1);
            nqStore1.AddProduct(nqProduct2);
            nqStore2.AddProduct(nqProduct2);
            nqStore2.AddProduct(nqProduct3);
            Session.Save(nqStore1);
            Session.Save(nqStore2);

//            nqProduct1.StockedIn(nqStore1);
//            nqProduct2.StockedIn(nqStore1);
//            nqProduct2.StockedIn(nqStore2);
//            nqProduct3.StockedIn(nqStore2);
//            Session.Save(nqProduct1);
//            Session.Save(nqProduct2);
//            Session.Save(nqProduct3);

            Session.Flush();
            Session.Clear();

            List<Store> stores = Session.Query<Store>().Fetch(s => s.Products).Where(s => !s.IsForQuery).OrderBy(s => s.Name).ToList();
            Session.Delete(stores.First());
            Session.Flush();
            Session.Clear();

            List<Store> storesAfterDeleted = Session.Query<Store>().Fetch(s => s.Products).Where(s => !s.IsForQuery).OrderBy(s => s.Name).ToList();
            Assert.Equal(1, storesAfterDeleted.Count);
            Assert.Equal("nq-store2", storesAfterDeleted[0].Name);
            Assert.Equal(new[] {"nq-product2", "nq-product3"},
                storesAfterDeleted[0].Products.Select(p => p.Name).OrderBy(n => n));

            Session.Clear();
            List<Product> productsAfterDeleted = Session.Query<Product>().Fetch(s => s.StoresStockedIn).Where(s => !s.IsForQuery).OrderBy(s => s.Name).ToList();
            Assert.Equal(3, productsAfterDeleted.Count);
            Assert.Equal("nq-product1", productsAfterDeleted[0].Name);
            Assert.Equal(0, productsAfterDeleted[0].StoresStockedIn.Count);
            Assert.Equal("nq-product2", productsAfterDeleted[1].Name);
            Assert.Equal(1, productsAfterDeleted[1].StoresStockedIn.Count);
            Assert.Equal("nq-product3", productsAfterDeleted[2].Name);
            Assert.Equal(1, productsAfterDeleted[1].StoresStockedIn.Count);
        }

        void ExecuteNonQuery(string sql)
        {
            ISQLQuery query = StatelessSession.CreateSQLQuery(sql);
            query.ExecuteUpdate();
        }
    }
}