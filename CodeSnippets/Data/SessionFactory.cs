using CodeSnippets.Data.Entities;
using CodeSnippets.Data.Mappings;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeSnippets.Data
{
    public class SessionFactory
    {
        public static ISessionFactory CreateSessionFactory()
        {
            var cfg = new Mappings.AutoMapper();
            return Fluently.Configure()
              .Database(MsSqlConfiguration.MsSql2005
                                          .ConnectionString("a raw string"))
              .Mappings(m =>
                m.AutoMappings.Add(
                  AutoMap.AssemblyOf<DatabaseEntity>(cfg)))
              .BuildSessionFactory();
        }
    }
}
