using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using MusicStoreDemo.Database;

namespace DatabaseMySqlMigrations
{

    public class MySqlMusicStoreDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MusicStoreDbContext>
    {
        public MusicStoreDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            var builder = new DbContextOptionsBuilder<MusicStoreDbContext>();

            builder.UseMySql(configuration.GetConnectionString("MySqlConnection"), 
                sqlOptions => sqlOptions.MigrationsAssembly(typeof(MySqlMusicStoreDesignTimeDbContextFactory).GetTypeInfo().Assembly.GetName().Name)
            );

            return new MusicStoreDbContext(builder.Options);
        }
    }
}
