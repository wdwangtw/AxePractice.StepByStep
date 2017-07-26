using System;
using System.Collections.Generic;

namespace Manualfac
{
    public class ContainerBuilder
    {
        #region Please modify the following code to pass the test

        /*
         * Hello, boys and girls. The container builder is a very good guy to store
         * all the definitions for instantiation as well as lifetime managing. Now,
         * let's forget about lifetime management.
         * 
         * ContainerBuilder however, has no idea how to create an instance. So it is
         * the users' job (func). We just store the procedure and call it when needed.
         * 
         * You can add non-public member functions or member variables as you like.
         */

        readonly Dictionary<Type, Func<IComponentContext, object>> funcs = new Dictionary<Type, Func<IComponentContext, object>>();
        private bool hasBeenBuilt = false;
        public void Register<T>(Func<IComponentContext, T> func)
        {
            funcs[typeof(T)] = c => func(c);
        }

        public IComponentContext Build()
        {
            if (hasBeenBuilt)
            {
                throw new InvalidOperationException("Container builder has been built.");
            }

            hasBeenBuilt = true;
            return new ComponentContext(funcs);
        }

        #endregion
    }
}