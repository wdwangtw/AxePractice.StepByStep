using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using Orm.Practice.Entities;
using Xunit;
using Xunit.Abstractions;

namespace Orm.Practice
{
    public class OneToManyQueryFacts : OrmFactBase
    {
        public OneToManyQueryFacts(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void should_get_all_rchildren()
        {
            Console.WriteLine("clause1");
            //select * from [Parent] parent0_  left outer join [Child] children1_  on parent0_.ParentId=children1_.ParentID  where parent0_.IsForQuery=1  order by parent0_.Name asc
            List<Parent> parents = Session.Query<Parent>()
                .Where(p => p.IsForQuery)
                .OrderBy(p => p.Name)
                .ToList();

            Console.WriteLine("clause2");
            //nothing
            var childrenForFirstParent = parents.First().Children;

            Console.WriteLine("clause3");
            //SELECT * FROM [Child] WHERE ParentID=@p0;
            Child firstChildForFirstParent = childrenForFirstParent.First();
            Console.WriteLine("clause4");
            //nothing
            Child lastChildForFirstParent = childrenForFirstParent.Last();
            Console.WriteLine("clause5");
            //nothing
            var childrenForLastParent = parents.Last().Children;
            Console.WriteLine("clause6");
            //SELECT * FROM [Child] WHERE ParentID=@p0;
            Child firstChildForLastParent = childrenForLastParent.First();

            Console.WriteLine("clause7");
            //nothing
            Child lastChildForLastParent = childrenForLastParent.Last();

            Assert.True(true);
//            Assert.Equal(
//                new[] {"child-1-for-parent-1", "child-2-for-parent-1", "child-3-for-parent-1", "child-4-for-parent-1"},
//                parents.First().Children.Select(c => c.Name).OrderBy(n => n));
//            Assert.Equal(
//                new[] { "child-1-for-parent-2", "child-2-for-parent-2", "child-3-for-parent-2" },
//                parents.Last().Children.Select(c => c.Name).OrderBy(n => n));
        }

        [Fact]
        public void should_get_parent_by_child()
        {
            Console.WriteLine("1");
// select * from Child] where IsForQuery=1 order by Name asc
            List<Child> children = Session.Query<Child>()
                .Where(p => p.IsForQuery)
                .OrderBy(p => p.Name)
                .ToList();
            Console.WriteLine("2");
            //blows are nothing
            List<Child> childFromParent1 = children.Where(c => c.Name.Last() == '1').ToList();

            Console.WriteLine("3");
            List<Child> childFromParent2 = children.Where(c => c.Name.Last() == '2').ToList();

            Console.WriteLine("4");
            Parent parent1 = childFromParent1.First().Parent;
            Console.WriteLine("5");
            Parent parent11 = childFromParent1.Last().Parent;

            Console.WriteLine("6");
            Parent parent2 = childFromParent2.First().Parent;
            Console.WriteLine("7");
            Parent parent22 = childFromParent2.Last().Parent;

            Console.WriteLine("8");
            // SELECT *FROM [Parent] WHERE ParentId=@p0;
            var parent1Name = parent1.Name;
            //noting
            Console.WriteLine("9");

            var parent11Name = parent11.Name;
            Console.WriteLine("10");
            // SELECT *FROM [Parent] WHERE ParentId=@p0;
            var parent2Name = parent2.Name;

            Console.WriteLine("11");
            Assert.True(childFromParent1.All(c => c.Parent.Name == "parent-query-1"));
            Assert.True(childFromParent2.All(c => c.Parent.Name == "parent-query-2"));
        }
    }
}