using System;
using System.Collections.Generic;

namespace Manualfac
{
    public class ComponentContext : IComponentContext
    {
        #region Please modify the following code to pass the test

        /*
         * A ComponentContext is used to resolve a component. Since the component
         * is created by the ContainerBuilder, it brings all the registration
         * information. 
         * 
         * You can add non-public member functions or member variables as you like.
         */

        private readonly Dictionary<Type, Func<IComponentContext, object>> funcs;

        public ComponentContext(Dictionary<Type, Func<IComponentContext, object>> funcs)
        {
            this.funcs = funcs;
        }

        public object ResolveComponent(Type type)
        {
            if (!funcs.ContainsKey(type))
            {
                throw new DependencyResolutionException("This type is not registered.");
            }

            return funcs[type](this);
        }

        #endregion
    }
}