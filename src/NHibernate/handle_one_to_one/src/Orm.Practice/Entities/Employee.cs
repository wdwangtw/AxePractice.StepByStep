using System;
using FluentNHibernate.Mapping;

namespace Orm.Practice.Entities
{
    public class Employee
    {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
//        public virtual Salary Salary { get; set; }
        public virtual Location Location { get; set; }
    }

    public class EmployeeMap : ClassMap<Employee>
    {
        public EmployeeMap()
        {
            Table("employees");
            Id(x => x.Id);
            Map(e => e.Name).Column("name");
//            HasOne( x => x.Salary).PropertyRef( x => x.Employee);
            HasOne(x =>x.Location).ForeignKey("Employee").Cascade.All();
        }
    }
}