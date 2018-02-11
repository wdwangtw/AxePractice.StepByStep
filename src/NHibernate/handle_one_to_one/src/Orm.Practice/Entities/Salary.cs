using System;
using FluentNHibernate.Mapping;

namespace Orm.Practice.Entities
{
    public class Salary
    {
        public virtual long Id { get; set; }
        public virtual long Fee { get; set; }
        public virtual Employee Employee { get; set; }
    }

    public class SalaryMap : ClassMap<Salary>
    {
        public SalaryMap()
        {
            Table("salary");
            Id(x => x.Id).Column("id");
            Map(x => x.Fee).Column("fee");
            References( x => x.Employee, "Id" ).Unique();



        }
    }
}