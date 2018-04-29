using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MusicStoreDemo.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace DatabaseMySqlMigrations
{
    public class MySqlMusicStoreIdentityServerDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            
            //builder.UseSqlite("Filename=./MusicStoreDatabase.sqlite");
            builder.UseMySql(
                configuration.GetConnectionString("MySqlConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(MySqlMusicStoreDesignTimeDbContextFactory).GetTypeInfo().Assembly.GetName().Name)
            );
                
            return new ConfigurationDbContext(builder.Options, new ConfigurationStoreOptions());
        }
    }

    public class MusicStoreIdentityServerPersistedGrantDesignTimeDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
            
            builder.UseMySql(
                configuration.GetConnectionString("MySqlConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(MySqlMusicStoreDesignTimeDbContextFactory).GetTypeInfo().Assembly.GetName().Name)
            );
            return new PersistedGrantDbContext(builder.Options,new OperationalStoreOptions());
        }
    }
}
