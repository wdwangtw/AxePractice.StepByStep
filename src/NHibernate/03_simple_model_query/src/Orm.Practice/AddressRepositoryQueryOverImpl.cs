using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentNHibernate.Utils;
using NHibernate;
using NHibernate.Criterion;

namespace Orm.Practice
{
    public class AddressRepositoryQueryOverImpl : RepositoryBase, IAddressRepository
    {
        public AddressRepositoryQueryOverImpl(ISession session) : base(session)
        {
        }

        public Address Get(int id)
        {
            #region Please implement the method

            return Session.QueryOver<Address>().Where(a => a.Id == id).SingleOrDefault();

            #endregion
        }

        public IList<Address> Get(IEnumerable<int> ids)
        {
            #region Please implement the method

            return Session.QueryOver<Address>().OrderBy(a => a.Id).Asc.Where(a => a.Id.IsIn(ids.ToArray())).List();

            #endregion
        }

        public IList<Address> GetByCity(string city)
        {
            #region Please implement the method

            return Session.QueryOver<Address>().Where(a => a.City == city).OrderBy(a => a.Id).Asc.List();

            #endregion
        }

        public Task<IList<Address>> GetByCityAsync(string city)
        {
            #region Please implement the method

            Task<IList<Address>> addresses = Session.QueryOver<Address>().Where(a => a.City == city)
                .OrderBy(a => a.Id).Asc.ListAsync();

            return addresses;

            #endregion
        }

        public Task<IList<Address>> GetByCityAsync(string city, CancellationToken cancellationToken)
        {
            #region Please implement the method

            Task<IList<Address>> addresses = Session.QueryOver<Address>().Where(a => a.City == city)
                .OrderBy(a => a.Id).Asc.ListAsync(cancellationToken);

            return addresses;

            #endregion
        }

        public IList<KeyValuePair<int, string>> GetOnlyTheIdAndTheAddressLineByCity(string city)
        {
            #region Please implement the method

            return Session.QueryOver<Address>()
                .Where(a => a.City == city)
                .OrderBy(a => a.Id)
                .Asc
                .Select(a => a.Id, a => a.AddressLine1)
                .List<object[]>()
                .Select(list => new KeyValuePair<int, string>((int) list[0], (string) list[1]))
                .ToList();

            #endregion
        }

        public IList<string> GetPostalCodesByCity(string city)
        {
            #region Please implement the method

            return Session.QueryOver<Address>()
                .Where(a => a.City == city)
                .Select(a => a.PostalCode)
                .Select(Projections.Distinct(Projections.Property("PostalCode")))
                .List<string>();

            #endregion
        }
    }
}