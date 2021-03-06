﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicStoreDemo.Database;
using MusicStoreDemo.Database.Entities;
using MusicStoreDemo.Database.Entities.Relationships;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;
using IdentityServer4.EntityFramework.DbContexts;
using System.Reflection;
using DatabaseSeeder.IdentityServerDefaults;

namespace DatabaseSeeder
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // create service collection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            var logger = serviceProvider.GetService<ILoggerFactory>()
                .CreateLogger<Program>();

            logger.LogInformation("Starting application");
            try
            {
                //run the seeder in a service context so we can use DI
                var seederApplication = serviceProvider.GetService<IDatabaseSeeder>();
                await seederApplication.Seed();
                logger.LogInformation("Seeder application execution complete!");
            }
            catch(Exception e)
            {
                logger.LogCritical(e, "Fatal error in seeder");
            }
            

        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            var configBasePath = Directory.GetCurrentDirectory();

            string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            // build configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(configBasePath)
                .AddJsonFile("appsettings.json", false)
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                .AddEnvironmentVariables()
                .Build();
            
            
            serviceCollection.AddOptions();

            serviceCollection.AddSingleton(typeof(IConfiguration), configuration);

            // add logging
            serviceCollection.AddSingleton(new LoggerFactory()
                .AddConsole(LogLevel.Trace)
                //.AddConsole(configuration)
                .AddDebug(LogLevel.Trace)
            );
            serviceCollection.AddLogging(); 

            // default to using sqlserver and enable MusicStoreAppDatabaseProvider=MYSQL is set as an env varible or in the config file
            string connectionString = configuration.GetConnectionString("SqlServerConnection");
            string appDatabaseMigrationsAssembly = typeof(MusicStoreDbContext).GetTypeInfo().Assembly.GetName().Name;

            serviceCollection.AddIdentity<DbUser, DbRole>()
                .AddEntityFrameworkStores<MusicStoreDbContext>();

            
            appDatabaseMigrationsAssembly = typeof(MusicStoreDbContext).GetTypeInfo().Assembly.GetName().Name;
            serviceCollection.AddDbContext<MusicStoreDbContext>(options => 
                                                                options.UseSqlServer(connectionString,
                                                                sqlOptions => sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly)));           
        

            serviceCollection.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = (builder) =>
                        {
                            builder.UseSqlServer(connectionString, sqlOptions =>
                                                    sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly));
                        };
                    }).AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = (builder) =>
                        {
                            builder.UseSqlServer(connectionString, sqlOptions =>
                                            sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly));
                        };
                    })
                    .AddAspNetIdentity<DbUser>();

            // add services
            serviceCollection.AddSingleton<IDatabaseSeeder,DatabaseSeeder>();

        }
    }
}
