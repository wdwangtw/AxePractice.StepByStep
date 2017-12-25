﻿using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Context;
using NHibernate.Linq;

namespace Orm.Practice
{
    public class AddressRepository
    {
        readonly ISession session;

        public AddressRepository(ISession session)
        {
            if (session == null)
            {
                throw new ArgumentNullException(nameof(session));
            }

            this.session = session;
        }

        public Address Get(int id)
        {
            return session.Query<Address>().FirstOrDefault(a => a.Id == id);
        }
    }
}