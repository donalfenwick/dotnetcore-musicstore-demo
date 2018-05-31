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
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System.Net;
using System.Net.Http;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.DataProtection;

namespace oauthtest
{
    public class Startup
    {
        private readonly ILogger<Startup> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;
        private readonly bool isDevEnvironment;

        public Startup(IConfiguration configuration, IHostingEnvironment environment, ILogger<Startup> logger)
        {
            _logger = logger;
            _configuration = configuration;
            _environment = environment;
            isDevEnvironment = _environment.IsDevelopment() || _environment.EnvironmentName.StartsWith("Development.");
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // default to using sqlserver and enable MusicStoreAppDatabaseProvider=MYSQL is set as an env varible or in the config file
            string connectionString = _configuration.GetConnectionString("SqlServerConnection");
            string appDatabaseMigrationsAssembly = typeof(MusicStoreDbContext).GetTypeInfo().Assembly.GetName().Name;



            services.AddDbContext<MusicStoreDbContext>(builder =>
            {
                builder.UseSqlServer(connectionString, sqlOptions => sqlOptions.MigrationsAssembly(appDatabaseMigrationsAssembly));             
            });
            services.AddIdentity<DbUser, DbRole>().AddEntityFrameworkStores<MusicStoreDbContext>();

            
            string cookieSchemeName = "cookie";

            string identityServerAuthority = _configuration.GetValue<string>("IdentityServerAuthorityUrl");
            string identityServerMetadataAddress = _configuration.GetValue<string>("IdentityServerMetadataAddress");

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
                options.Authority = identityServerAuthority;
                options.MetadataAddress = identityServerMetadataAddress;
                
                
                if (_environment.EnvironmentName == "localdocker")
                {
                    string certPassword = "SecretPassword123";
                    string certPath = Path.Combine(_environment.ContentRootPath, "Certs/example.pfx");
                    X509Certificate2 cert = new X509Certificate2(certPath, certPassword);
        
                    options.TokenValidationParameters = new TokenValidationParameters{
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = new X509SecurityKey(cert),
                        IssuerSigningKeyResolver = (string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters) => new List<X509SecurityKey> { new X509SecurityKey(cert) }
    
                    };
                }
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

            // if running on a linux environment dotnet doesn't know where to put the data protection keys by default
            if(System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux)){
                services.AddDataProtection()
                    .SetApplicationName("musicstore-oauthtest")
                    .PersistKeysToFileSystem(new System.IO.DirectoryInfo(@"/var/dpkeys/"));
            }

            services.AddMvc()
                .SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (this.isDevEnvironment)
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
