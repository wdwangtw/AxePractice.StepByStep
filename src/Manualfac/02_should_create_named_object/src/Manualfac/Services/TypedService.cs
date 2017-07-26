using System;

namespace Manualfac.Services
{
    class TypedService : Service, IEquatable<TypedService>
    {
        #region Please modify the following code to pass the test

        /*
         * This class is used as a key for registration by type.
         */

        public TypedService(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public Type ServiceType { get; }

        public bool Equals(TypedService other)
        {
            if (other == null) return false;
            if (this == other) return true;

            return ServiceType == other.ServiceType;
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (this == obj) return true;
            if (GetType() != obj.GetType()) return false;

            return Equals((TypedService)obj);
        }

        public override int GetHashCode()
        {
            return ServiceType.GetHashCode();
        }

        #endregion
    }
}