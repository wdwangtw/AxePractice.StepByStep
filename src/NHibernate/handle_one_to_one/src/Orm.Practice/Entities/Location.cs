using System;
using FluentNHibernate.Mapping;

namespace Orm.Practice.Entities
{
    public class Location
    {
        public virtual long EmployeeId { get; set; }
        public virtual string Country { get; set; }
        public virtual Employee Employee { get; set; }
    }

    public class LocationMap : ClassMap<Location>
    {
        public LocationMap()
        {
            Table("location");
            Id(s => s.EmployeeId).Column("employee_id").GeneratedBy.Foreign("Employee");
            Map(s => s.Country).Column("country");

            HasOne(s => s.Employee).Constrained().Cascade.All();
        }
    }
}