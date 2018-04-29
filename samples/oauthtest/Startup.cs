using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MusicStoreDemo.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Design;
using MusicStoreDemo.Common.Repositories;
using System.Runtime.InteropServices;
using MusicStoreDemo.Database.Entities;
using System.Reflection;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.SpaServices.Webpack;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using DatabaseMySqlMigrations;

namespace oauthtest
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

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
            services.AddIdentity<DbUser, DbRole>().AddEntityFrameworkStores<MusicStoreDbContext>();

            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            string cookieSchemeName = "cookie";

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = cookieSchemeName;
                options.DefaultChallengeScheme = "oidc";
                // set default authentication scheme to prevent auth loop while 
                // also using aspnet identity in the pipeline
                options.DefaultAuthenticateScheme = "oidc";
            })
            .AddCookie(cookieSchemeName)
            .AddOpenIdConnect("oidc", options =>
            {
                options.ClientId = "testMvcClient";
                options.SignInScheme = cookieSchemeName;
                //var backChannel = options.Backchannel;
                options.RequireHttpsMetadata = false;
                options.ClientSecret = "secret";
                // location of identity server
                options.Authority = "http://localhost:5601/";

                // request the id_token (jwt with users identity claims) and the code, which is used in a 
                // backend request to identity server to get the JWT access_token.
                options.ResponseType = "code id_token";

                // save tokes in cookie so that we can use it for signout
                options.SaveTokens = true;
                options.GetClaimsFromUserInfoEndpoint = true;

                // request the following scopes from identity server
                options.Scope.Add("profile");
                options.Scope.Add("apiAccess");
                options.Scope.Add("email");
                options.Scope.Add("offline_access");
                options.Scope.Add("profile");
                options.Scope.Add("roles");


                // map the claims of type role into the user principal after authentication
                options.Events = new OpenIdConnectEvents()
                {
                    OnUserInformationReceived = (context) =>
                    {
                        // IDS4 returns multiple claim values as JSON arrays, the array type causes issues with the 
                        // the authentication handler. Map them to claims manually from a JArray
                        if (context.User.TryGetValue(ClaimTypes.Role, out JToken role))
                        {
                            var claims = new List<Claim>();
                            if (role.Type != JTokenType.Array)
                            {
                                claims.Add(new Claim(ClaimTypes.Role, (string)role));
                            }
                            else
                            {
                                foreach (var r in role)
                                    claims.Add(new Claim(ClaimTypes.Role, (string)r));
                            }
                            var id = context.Principal.Identity as ClaimsIdentity;
                            id.AddClaims(claims);
                            
                        }
                        return Task.FromResult(0);
                    }
                };
            });

            services.AddMvc();
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

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
