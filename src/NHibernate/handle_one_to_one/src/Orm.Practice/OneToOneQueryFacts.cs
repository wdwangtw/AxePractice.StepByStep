using System.Collections.Generic;
using System.Linq;
using Orm.Practice.Entities;
using Xunit;
using Xunit.Abstractions;

namespace Orm.Practice
{
    public class OneToOneFacts : OrmFactBase
    {
        public OneToOneFacts(ITestOutputHelper output) : base(output)
        {
            Session.CreateSQLQuery("trancate table employee");
            Session.CreateSQLQuery("trancate table salary");
        }


        [Fact]
        public void create_and_query()
        {
            var employee = new Employee
            {
                Name = "helloworld"
            };

            var location = new Location {Country = "China"};

            employee.Location = location;
            location.Employee = employee;

            Session.Save(employee);

            Session.Flush();

            Session.Clear();

            var e = Session.Query<Employee>().ToList().Last();
            Assert.Equal("helloworld", e.Name);
            Assert.Equal("China", e.Location.Country);
        }


    }
}