using System;
using FluentNHibernate;
using FluentNHibernate.Automapping;

namespace Orm.Practice
{
    class TypeSpecificMappingConfig : DefaultAutomappingConfiguration
    {
        public override bool ShouldMap(Type type)
        {
            return type != null && type ==  typeof(Address);
        }
    }
}