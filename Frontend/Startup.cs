
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicStoreDemo.Common.Repositories;
using Swashbuckle.AspNetCore.Swagger;
using MusicStoreDemo.Common.Mappers;
using MusicStoreDemo.Database;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using MusicStoreDemo.Database.Entities;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using DatabaseMySqlMigrations;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.AspNetCore.SpaServices.AngularCli;



namespace MusicStoreDemo
{
    public class Startup
    {
        private readonly IHostingEnvironment environment;
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            this.environment = env;
            this.configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
                options.AddPolicy("defaultCorsPolicy", builder =>
                {
                    builder.WithOrigins("http://localhost:5600","http://localhost:5601")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                }));
            
            services.AddMvc();

            string connectionString = configuration.GetConnectionString("SqlServerConnection");
            bool useMySql = false;
            string databaseProvider =  configuration.GetValue<string>("MusicStoreAppDatabaseProvider");
            if(databaseProvider.Equals("MYSQL", StringComparison.InvariantCultureIgnoreCase)){
                useMySql = true;
                connectionString = configuration.GetConnectionString("MySqlConnection");
            }

            

            services.AddDbContext<MusicStoreDbContext>(builder =>
            {
                if (useMySql)
                {
                    string appDatabaseMigrationsAssembly = typeof(MySqlMusicStoreIdentityServerDesignTimeDbContextFactory).GetTypeInfo().Assembly.GetName().Name;
                    builder.UseMySql(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly));
                }
                else
                {
                    var appDatabaseMigrationsAssembly = typeof(MusicStoreDbContext).GetTypeInfo().Assembly.GetName().Name;
                    
                    builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly));
                }
            });
            

            services.AddIdentity<DbUser, DbRole>( options =>
            {
                // set which claims the IdentityManager<TUser> should use when doing a lookup using a user principal
                options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
                options.ClaimsIdentity.UserIdClaimType = "sub";
            }).AddEntityFrameworkStores<MusicStoreDbContext>();


            services.AddCors(options =>
                 options.AddPolicy("defaultCorsPolicy", builder =>
                 {
                    builder.WithOrigins("http://localhost:5600",
                                        "http://localhost:5602")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials();
                 }));


            services.AddAuthentication(options =>
            {
                // must set default scheme when calling services.AddIdentity(...) in the 
                // pipeline for querying users so that the app knows to always use bearer
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
                options.DefaultForbidScheme = "Bearer";
                options.DefaultScheme = "Bearer";
            })
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "http://localhost:5601";
                    options.RequireHttpsMetadata = false;

                    options.ApiName = "api1";

                    // set which claims the roles and username will be on the identity principal
                    options.NameClaimType = ClaimTypes.NameIdentifier;
                    options.RoleClaimType = ClaimTypes.Role;
                });


            services.AddAuthorization(options =>
            {
                // define access policies for the api
                options.AddPolicy("musicStoreApi.readAccess", policy => { 
                    policy.RequireClaim("musicStoreApi.readAccessClaim", "true"); 
                });
            
                options.AddPolicy("musicStoreApi.writeAccess", policy => { 
                    policy.RequireClaim("musicStoreApi.readAccessClaim", "true"); 
                    policy.RequireClaim("musicStoreApi.writeAccessClaim", "true"); 
                });
            });

            services.AddTransient<ArtistMapper>();
            services.AddTransient<AlbumMapper>();
            services.AddTransient<AlbumGroupMapper>();
            services.AddScoped<IArtistRepository, ArtistRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IAlbumRepository, AlbumRepository>();
            services.AddScoped<IAlbumGroupRepository, AlbumGroupRepository>();
            services.AddScoped<IImageRepository, ImageRepository>();
            
            // add swagger to generate api documentation
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
            
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

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

            app.UseAuthentication();

            app.UseCors("defaultCorsPolicy");
            
            app.UseStaticFiles();
            app.UseSpaStaticFiles();


            app.UseMvc(routes =>
            {
                
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            // enable swagger endpoint
            
            app.UseSwagger(c => {
                // set the host property in the swagger doc so it can be 
                // imported by external clients like postman
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Host = httpReq.Host.Value);
            });
            // enable swagger UI api docs to run at  ~/swagger
            app.UseSwaggerUI(ui => {
                ui.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";
              
                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });


            
        }
    }
}
