
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
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.DataProtection;
using System.Net.Http;

namespace MusicStoreDemo
{
    public class Startup
    {
        private readonly IHostingEnvironment environment;
        private readonly IConfiguration configuration;
        private readonly ILogger<Startup> logger;
        private readonly bool isDevelopmentEnv;
        public Startup(IConfiguration configuration, IHostingEnvironment env, ILogger<Startup> logger)
        {
            this.logger = logger;
            this.environment = env;
            this.configuration = configuration;
            this.isDevelopmentEnv = env.IsDevelopment() || env.EnvironmentName == "Development.osx";
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            

            services.AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            string connectionString = configuration.GetConnectionString("SqlServerConnection");


            services.AddDbContext<MusicStoreDbContext>(builder =>
            {
                var appDatabaseMigrationsAssembly = typeof(MusicStoreDbContext).GetTypeInfo().Assembly.GetName().Name;
                builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly));
            });


            services.AddIdentity<DbUser, DbRole>(options =>
           {
                // set which claims the IdentityManager<TUser> should use when doing a lookup using a user principal
                options.ClaimsIdentity.RoleClaimType = ClaimTypes.Role;
                options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier;
           }).AddEntityFrameworkStores<MusicStoreDbContext>();

            // map the claims of type role into the user principal after authentication
                
            string identityServerAuthority = configuration.GetValue<string>("IdentityServerAuthorityUrl");
            string identityServerMetadataAddress = configuration.GetValue<string>("IdentityServerMetadataAddress");

            logger.LogInformation($"Adding jwt authentication with authority: {identityServerAuthority} and metadataURL: {identityServerMetadataAddress}");
            services.AddAuthentication(options =>
            {
                // must set default scheme when calling services.AddIdentity(...) in the 
                // pipeline for querying users so that the app knows to always use bearer
                options.DefaultAuthenticateScheme = "Bearer";
                options.DefaultChallengeScheme = "Bearer";
                options.DefaultForbidScheme = "Bearer";
                options.DefaultScheme = "Bearer";
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.Authority = identityServerAuthority;
                options.Audience = "api1";
                options.MetadataAddress = identityServerMetadataAddress;
                
                options.IncludeErrorDetails = true;
                options.TokenValidationParameters = new TokenValidationParameters() {
                    NameClaimType = "sub",
                    RoleClaimType = ClaimTypes.Role,
                };
                // if using unsigned certs in dev or in a docker environment where the internal dns name of the
                // container will be something other than localhost then allow unsigned certs 
                if(isDevelopmentEnv || environment.EnvironmentName == "localdocker"){
                    options.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };
                }
            });

            services.AddCors(options =>
                options.AddPolicy("defaultCorsPolicy", builder =>
                {
                    builder.WithOrigins(
                        identityServerAuthority.Trim().TrimEnd('/'),
                        "http://localhost:5600", 
                        "http://localhost:5607",
                        "https://localhost:44350", 
                        "https://localhost:44357"
                        )
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials();
                }));

            services.AddAuthorization(options =>
            {
                // define access policies for the api
                options.AddPolicy("musicStoreApi.readAccess", policy =>
                {
                    policy.RequireClaim("musicStoreApi.readAccessClaim", "true");
                });

                options.AddPolicy("musicStoreApi.writeAccess", policy =>
                {
                    policy.RequireClaim("musicStoreApi.readAccessClaim", "true");
                    policy.RequireClaim("musicStoreApi.writeAccessClaim", "true");
                });
            });

            // if running on a linux environment dotnet doesn't know where to put the data protection keys by default
            if(System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux)){
                services.AddDataProtection()
                    .SetApplicationName("musicstore-frontend")
                    .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"/var/dpkeys/"));
            }

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
            
            if (isDevelopmentEnv)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            
            app.UseAuthentication();

            app.UseCors("defaultCorsPolicy");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();


            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            // enable swagger endpoint

            app.UseSwagger(c =>
            {
                // set the host property in the swagger doc so it can be 
                // imported by external clients like postman
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) => swaggerDoc.Host = httpReq.Host.Value);
            });
            // enable swagger UI api docs to run at  ~/swagger
            app.UseSwaggerUI(ui =>
            {
                ui.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (isDevelopmentEnv)
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });



        }
    }
}
