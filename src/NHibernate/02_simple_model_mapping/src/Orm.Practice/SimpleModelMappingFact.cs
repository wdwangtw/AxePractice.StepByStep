﻿using System;
using System.Reflection;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using Xunit;

namespace Orm.Practice
{
    /*
     * Please read the corresponding table definition in the SQL script or from
     * the table definition and map the table to the object. If the mapping is
     * correct, then all the tests should pass.
     */

    public class SimpleModelMappingFact : IDisposable
    {
        readonly ISessionFactory sessionFactory;
        readonly ISession session;

        #region please provide a valid sql server connection string

        /*
         * A connection string is needed when connecting to SQL Server instance. 
         * Our connection string contains the following parts.
         * 
         * - Data Source: The data source name or location
         * - Initial Catalog: The default database that the connection will work with
         * - Integrated Security: whether User ID and Password are specified in the 
         *   connection. For example, if you use Windows Authentication to access SQL
         *   Server instance, this value should set as `true`.
         */

        protected string ConnectionString { get; } = "Data Source=(local);Initial Catalog=AdventureWorks2014;Integrated Security=true;";

        #endregion

        public SimpleModelMappingFact()
        {
            sessionFactory = CreateSessionFactory(ConnectionString);

            #region Please initialize the session object

            session = sessionFactory.OpenSession();

            #endregion
        }

        static ISessionFactory CreateSessionFactory(string connectionString)
        {
            #region Please implement the method to pass the test

            /*
             * We use *FluentNHiberate* package to configure NHibernate. In order
             * to do query, we should get a `ISession` object, which holds the 
             * connection to database. The `ISession` object is created by a 
             * `ISessionFactory` so `ISessionFactory` should be created first.
             */

            return Fluently.Configure().Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(
                    mcf =>
                        mcf.AutoMappings.Add(AutoMap.Assembly(Assembly.GetExecutingAssembly(),
                            new TypeSpecificMappingConfig())
                            .UseOverridesFromAssembly(Assembly.GetExecutingAssembly())))
                .BuildSessionFactory();

            #endregion
        }

        [Fact]
        public void should_map_person_address_model()
        {
            var addressRepository = new AddressRepository(session);
            Address address = addressRepository.Get(1);

            Assert.Equal(1, address.Id);
            Assert.Equal("1970 Napa Ct.", address.AddressLine1);
            Assert.Null(address.AddressLine2);
            Assert.Equal("Bothell", address.City);
            Assert.Equal("98011", address.PostalCode);
        }

        public void Dispose()
        {
            session?.Dispose();
            sessionFactory?.Dispose();
        }
    }
}