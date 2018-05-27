using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicStoreDemo.AdminSite.Models;
using MusicStoreDemo.AdminSite.Services;
using MusicStoreDemo.Database;
using System.Runtime.InteropServices;
using MusicStoreDemo.Database.Entities;
using System.Reflection;
using MusicStoreDemo.Common.Repositories;
using MusicStoreDemo.Common.Mappers;
using MusicStoreDemo.Common;
using DatabaseMySqlMigrations;
using Microsoft.AspNetCore.DataProtection;

namespace MusicStoreDemo.AdminSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

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

            services.AddIdentity<DbUser, DbRole>()
			    .AddDefaultTokenProviders()    
			    .AddEntityFrameworkStores<MusicStoreDbContext>();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddTransient<ArtistMapper>();
            services.AddTransient<AlbumMapper>();
            services.AddTransient<AlbumGroupMapper>();
            services.AddScoped<IArtistRepository, ArtistRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IAlbumGroupRepository, AlbumGroupRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();


            // if running on a linux environment dotnet doesn't know where to put the data protection keys by default
            if(System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux)){
                services.AddDataProtection()
                    .SetApplicationName("musicstore-adminsite")
                    .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"/var/dpkeys/"));
            }

            services.AddMvc()
                .AddCookieTempDataProvider();

			services.AddAuthorization(options =>
            {
				options.AddPolicy("isAdmin", policy =>
				{
					policy.RequireRole(MusicStoreConstants.Roles.AdminUser);
				});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseAuthentication();
            //app.UseSession()
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
