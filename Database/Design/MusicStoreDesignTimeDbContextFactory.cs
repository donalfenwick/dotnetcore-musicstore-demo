using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace MusicStoreDemo.Database.Design
{
    public class MySqlMusicStoreDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MusicStoreDbContext>
    {
        public MusicStoreDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<MusicStoreDbContext>();

            builder.UseSqlServer(configuration.GetConnectionString("SqlServerConnection"));
            
            return new MusicStoreDbContext(builder.Options);
        }
    }
}
