using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace Orm.Practice.Entities
{
    public class Product
    {
        public virtual Guid ProductId { get; protected set; }
        public virtual string Name { get; set; }
        public virtual bool IsForQuery { get; set; }
        public virtual IList<Store> StoresStockedIn { get; protected set; } = new List<Store>();

        public virtual void StockedIn(Store store)
        {
            StoresStockedIn.Add(store);
        }
    }

    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Table("products");
            Id(p => p.ProductId).Column("ProductID").GeneratedBy.Guid();
            Map(p => p.Name).Column("Name");
            Map(p => p.IsForQuery).Column("IsForQuery");

            HasManyToMany(p => p.StoresStockedIn).Cascade.All().Table("store_product").ParentKeyColumn("ProductID").ChildKeyColumn("StoreID");
        }
    }
}