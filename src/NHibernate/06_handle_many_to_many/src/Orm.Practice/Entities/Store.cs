using System;
using System.Collections.Generic;
using FluentNHibernate.Mapping;

namespace Orm.Practice.Entities
{
    public class Store
    {
        public virtual Guid StoreId { get; protected set; }
        public virtual string Name { get; set; }
        public virtual bool IsForQuery { get; set; }
        public virtual IList<Product> Products { get; set; } = new List<Product>();

        public virtual void AddProduct(Product product)
        {
            Products.Add(product);
        }
    }

    public class StoreMap : ClassMap<Store>
    {
        public StoreMap()
        {
            Table("stores");
            Id(s => s.StoreId).Column("StoreID").GeneratedBy.Guid();
            Map(s => s.Name).Column("Name");
            Map(p => p.IsForQuery).Column("IsForQuery");

            HasManyToMany(s => s.Products).Cascade.All().Table("store_product").ParentKeyColumn("StoreID").ChildKeyColumn("ProductID");
        }
    }
}