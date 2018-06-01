using System;
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
using Microsoft.AspNetCore.DataProtection;
using IdentityServer4.Services;

namespace MusicStoreDemo.IdentityServer
{
    public class Startup
    {
        

        public Startup(IConfiguration configuration, IHostingEnvironment env, ILogger<Startup> log, ILoggerFactory loggerFactory)
        {
            _log = log;
            _configuration = configuration;
            _env = env;
            _loggerFactory = loggerFactory;
        }

        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;
        private readonly ILogger<Startup> _log;
        private readonly ILoggerFactory _loggerFactory;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);
        
            // setup tutorial https://www.scottbrady91.com/Identity-Server/Getting-Started-with-IdentityServer-4

            // default to using sqlserver and enable MusicStoreAppDatabaseProvider=MYSQL is set as an env varible or in the config file
            string connectionString = _configuration.GetConnectionString("SqlServerConnection");
            string appDatabaseMigrationsAssembly = typeof(MusicStoreDbContext).GetTypeInfo().Assembly.GetName().Name;


            services.AddDbContext<MusicStoreDbContext>(builder =>
            {
                builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly));
            });

            services.AddIdentity<DbUser, DbRole>().AddEntityFrameworkStores<MusicStoreDbContext>();


			services.AddIdentityServer(o =>{
                o.IssuerUri = _configuration.GetValue<string>("IdentityServerIssuerUri");
            })
			.AddSigningCredential(GetSigningCredentialCert())
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
            .AddAspNetIdentity<DbUser>()
            .AddProfileService<MusicStoreDbProfileService>();

            // if running on a linux environment dotnet doesn't know where to put the data protection keys by default
            if(System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux)){
                services.AddDataProtection()
                    .SetApplicationName("musicstore-identityserver")
                    .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"/var/dpkeys/"));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            bool isDevEnvironment = env.IsDevelopment() || env.EnvironmentName.StartsWith("Development.");
            if (isDevEnvironment)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseStaticFiles();

            //app.UseCors("defaultCorsPolicy");

            app.UseIdentityServer();

            app.UseMvcWithDefaultRoute();

            
        }

        private X509Certificate2 GetSigningCredentialCert(){
            X509Certificate2 cert = null;
            string certPassword = "SecretPassword123";
            // Fallback to local file for development
            if (cert == null)
            {
                string certPath = Path.Combine(_env.ContentRootPath, "Certs/example.pfx");
                _log.LogInformation($"GetSigningCredentialCert - Loadcert {certPath}");
                FileInfo certFile = new FileInfo(certPath);
                cert = new X509Certificate2(certPath, certPassword);
            }
            return cert;
        }
    }
}
