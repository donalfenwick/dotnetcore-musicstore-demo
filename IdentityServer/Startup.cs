﻿using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IdentityServer4.Hosting;
using IdentityServer4.Test;
using IdentityServer4.Models;
using System.Security.Claims;
using IdentityModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using IdentityServer4;
using System.Runtime.InteropServices;
using MusicStoreDemo.Database;
using MusicStoreDemo.Database.Entities;
using Microsoft.AspNetCore.Cors.Infrastructure;
using MusicStoreDemo.Common.Mappers;
using MusicStoreDemo.Api.IdentityServer;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using Microsoft.Extensions.Logging;
using IdentityServer4.Validation;
using DatabaseMySqlMigrations;

namespace MusicStoreDemo.IdentityServer
{
    public class Startup
    {
        

        public Startup(IConfiguration configuration, IHostingEnvironment env, ILogger<Startup> log)
        {
            _log = log;
            _configuration = configuration;
            _env = env;
        }

        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;
        private readonly ILogger<Startup> _log;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            // setup tutorial https://www.scottbrady91.com/Identity-Server/Getting-Started-with-IdentityServer-4

            // default to using sqlserver and enable MusicStoreAppDatabaseProvider=MYSQL is set as an env varible or in the config file
            string connectionString = _configuration.GetConnectionString("SqlServerConnection");
            string appDatabaseMigrationsAssembly = typeof(MusicStoreDbContext).GetTypeInfo().Assembly.GetName().Name;

            string databaseProvider =  _configuration.GetValue<string>("MusicStoreAppDatabaseProvider");
            bool useMySql = false;
            if(databaseProvider.Equals("MYSQL", StringComparison.InvariantCultureIgnoreCase)){
                useMySql = true;
                appDatabaseMigrationsAssembly = typeof(MySqlMusicStoreIdentityServerDesignTimeDbContextFactory).GetTypeInfo().Assembly.GetName().Name;
                connectionString = _configuration.GetConnectionString("MySqlConnection");
            }

            services.AddDbContext<MusicStoreDbContext>(builder =>
            {
                if (useMySql)
                {
                    builder.UseMySql(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly));
                }
                else
                {
                    builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly));
                }
            });

            services.AddIdentity<DbUser, DbRole>().AddEntityFrameworkStores<MusicStoreDbContext>();

            
            X509Certificate2 cert = GetSigningCredentialCert();

			services.AddIdentityServer()
					.AddSigningCredential(cert)
					.AddOperationalStore(options =>
					{
						options.ConfigureDbContext = (builder) =>
						{
							if (useMySql)
                            {
                                builder.UseMySql(connectionString, sqlOptions =>
												 sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly));
							}
							else
							{
                                builder.UseSqlServer(connectionString, sqlOptions =>
													 sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly));
							}
						};
					}).AddConfigurationStore(options =>
					{
						options.ConfigureDbContext = (builder) =>
						{
							if (useMySql)
                            {
                                builder.UseMySql(connectionString, sqlOptions =>
										 sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly));
							}
							else
							{
                                builder.UseSqlServer(connectionString, sqlOptions =>
											 sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly));
							}
						};
					})
					.AddAspNetIdentity<DbUser>()
					.AddProfileService<MusicStoreDbProfileService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            //app.UseCors("defaultCorsPolicy");

            app.UseIdentityServer();

            app.UseMvcWithDefaultRoute();
        }

        private X509Certificate2 GetSigningCredentialCert(){
            X509Certificate2 cert = null;
            /*using (X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser))
            {
                certStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection certCollection = certStore.Certificates.Find(
                    X509FindType.FindByThumbprint,
                    // Replace below with your cert's thumbprint
                    "CB781679561914B7539BE120EE9C4F6780579A86",
                    false);
                // Get the first cert with the thumbprint
                if (certCollection.Count > 0)
                {
                    cert = certCollection[0];
                    _log.LogInformation($"Successfully loaded cert from registry: {cert.Thumbprint}");
                }
            }*/
            string certPassword = "SecretPassword123";
            // Fallback to local file for development
            if (cert == null)
            {
                cert = new X509Certificate2(Path.Combine(_env.ContentRootPath, "certs/example.pfx"), certPassword);
                _log.LogInformation($"Falling back to cert from file. Successfully loaded: {cert.Thumbprint}");
            }
            return cert;
        }
    }
}
