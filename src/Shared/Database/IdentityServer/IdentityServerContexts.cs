using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicStoreDemo.Database.IdentityServer
{
    // override identity DB contexts so that identity columns generated in migrations work accross my sql and sqlserver
    //public class MusicStorePersistedGrantDbContext : PersistedGrantDbContext
    //{
    //    public MusicStorePersistedGrantDbContext(DbContextOptions<PersistedGrantDbContext> options, OperationalStoreOptions storeOptions)
    //        : base(options, storeOptions)
    //    {
    //    }

    //    protected override void OnModelCreating(ModelBuilder builder)
    //    {
    //        base.OnModelCreating(builder);

    //        builder.ForMySqlUseIdentityColumns();
    //        builder.ForSqlServerUseIdentityColumns();
    //    }
    //}

    //public class MusicStoreConfigurationDbContext : ConfigurationDbContext // ConfigurationDbContext
    //{
    //    public MusicStoreConfigurationDbContext(DbContextOptions<ConfigurationDbContext> options, ConfigurationStoreOptions storeOptions) 
    //        : base(options, storeOptions)
    //    {
    //    }

    //    protected override void OnModelCreating(ModelBuilder builder)
    //    {
    //        base.OnModelCreating(builder);

    //        builder.ForMySqlUseIdentityColumns();
    //        builder.ForSqlServerUseIdentityColumns();
    //    }
    //}
}
