using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using MusicStoreDemo.Common;

namespace DatabaseSeeder.IdentityServerDefaults
{

    internal class IdentityServerResources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            var profileResource = new IdentityResources.Profile();
            profileResource.Required = true;
            // add the standard name claim type to the profile scope resource so it 
            // populates User.Identity.Name within the client MVC app
            profileResource.UserClaims.Add(ClaimTypes.Name);
            return new List<IdentityResource> {
                new IdentityResources.OpenId(),
                profileResource,
                new IdentityResources.Email(),
                new IdentityResource {
                    Name = "roles",
                    DisplayName = "Your access roles",
                    Required = true,
                    Description = "Your permissions on the system",
                    UserClaims = new List<string> {
                        //"role",
                        ClaimTypes.Role
                    },
                },
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource> {
                new ApiResource("api1", "Music store api"){
                    Scopes = new List<Scope>(){
                        new Scope("apiAccess", "Api access"){
                            // user claims will be read from the userclaims table in the 
							// database or can be dynamicly populated in the ID servers custom
							// profile service implementation (see age demographic example)
							UserClaims ={ 
                                ClaimTypes.Role,
								MusicStoreConstants.CLaimTypes.MusicStoreApiReadAccess,                        
								MusicStoreConstants.CLaimTypes.MusicStoreApiWriteAccess,
								MusicStoreConstants.CLaimTypes.MusicStoreApiAgeDemographic

                            }
                        }
                    },

                }
            };
        }
    }
}
