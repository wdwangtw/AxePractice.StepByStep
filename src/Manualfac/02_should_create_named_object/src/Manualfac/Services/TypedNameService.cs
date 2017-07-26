using System;

namespace Manualfac.Services
{
    class TypedNameService : Service, IEquatable<TypedNameService>
    {
        #region Please modify the following code to pass the test

        /*
         * This class is used as a key for registration by both type and name.
         */

        public TypedNameService(Type serviceType, string name)
        {
            ServiceType = serviceType;
            Name = name;
        }

        public string Name { get; }
        public Type ServiceType { get; }

        public bool Equals(TypedNameService other)
        {
            if (other == null) return false;
            if (this == other) return true;
            return ServiceType == other.ServiceType && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (this == obj) return true;
            if (GetType() != obj.GetType()) return false;
            return Equals((TypedNameService)obj);
        }

        public override int GetHashCode()
        {
            return ServiceType.GetHashCode() ^ Name.GetHashCode();
        }

        #endregion
    }
}