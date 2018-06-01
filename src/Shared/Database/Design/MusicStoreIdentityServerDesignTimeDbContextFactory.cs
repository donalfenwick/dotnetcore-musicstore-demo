using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace MusicStoreDemo.Database.Design
{
    public class MusicStoreIdentityServerConfigDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ConfigurationDbContext>
    {
        public ConfigurationDbContext CreateDbContext(string[] args)
        {
            var appDatabaseMigrationsAssembly = typeof(MusicStoreDbContext).GetTypeInfo().Assembly.GetName().Name;
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            
            builder.UseSqlServer(
                configuration.GetConnectionString("SqlServerConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly)
            );
            
            return new ConfigurationDbContext(builder.Options, new ConfigurationStoreOptions());
        }
    }

    public class MusicStoreIdentityServerPersistedGrantDesignTimeDbContextFactory : IDesignTimeDbContextFactory<PersistedGrantDbContext>
    {
        public PersistedGrantDbContext CreateDbContext(string[] args)
        {
            var appDatabaseMigrationsAssembly = typeof(MusicStoreDbContext).GetTypeInfo().Assembly.GetName().Name;
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
            
            builder.UseSqlServer(
                configuration.GetConnectionString("SqlServerConnection"),
                sqlOptions => sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly)
            );
        

            return new PersistedGrantDbContext(builder.Options,new OperationalStoreOptions());
        }
    }
}
