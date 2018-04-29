using System;
using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace DatabaseSeeder.IdentityServerDefaults
{

    internal class IdentityServerClients
    {
        public static IEnumerable<Client> GetClients()
        {
            var allowedScopes = new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                IdentityServerConstants.StandardScopes.Email,
                "role",
                "customAPI.write",
                "apiAccess",
                "roles"
            };

            string angularClientUrl = "http://localhost:5600";
            var angularFrontendClient = new Client {
                ClientId = "musicStoreAngularFrotend",
                ClientName = "Music store Angular 4 Client",
                AllowedGrantTypes = GrantTypes.Implicit,
                AllowedScopes = allowedScopes,
                RedirectUris = new List<string> { $"{angularClientUrl}/auth-callback", $"{angularClientUrl}/assets/silent-refresh.html" },
                PostLogoutRedirectUris = new List<string> { angularClientUrl+"/" },
                AllowedCorsOrigins = new List<string> { angularClientUrl },
                AllowAccessTokensViaBrowser = true
            };
            string mvcClientUrl = "http://localhost:5607";
            var mvcFrontendClient = new Client {
                ClientId = "testMvcClient",
                ClientName = "Frontend MVC application for music store",
                //AllowedGrantTypes = GrantTypes.Implicit,
                AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                AllowedScopes = allowedScopes,
                ClientSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                RedirectUris = new List<string> {$"{mvcClientUrl}/signin-oidc"},
                //logout callback URL
                PostLogoutRedirectUris = new List<string> {$"{mvcClientUrl}/signout-callback-oidc"},
                FrontChannelLogoutUri = $"{mvcClientUrl}/signout-oidc",
                // cant find this for microsofts OIDC implementation
                //BackChannelLogoutUri = "??",
                AllowOfflineAccess = true,
                // enable all profile claims to be sent in the ID token if this is configured false the 
                // client must make a separate call to the idserver endpoint /connect/userinfo to acces the user claims
                AlwaysIncludeUserClaimsInIdToken = true,
                AlwaysSendClientClaims = false
            };
            var postmanClient = new Client {
                ClientId = "postmanClient",
                ClientName = "Postman test client",
                //AllowedGrantTypes = GrantTypes.Implicit,
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes = allowedScopes,
                ClientSecrets = new List<Secret> { new Secret("secret".Sha256()) },
                RedirectUris = new List<string> {"https://www.getpostman.com/oauth2/callback"},
                PostLogoutRedirectUris = new List<string> {"https://www.getpostman.com/"},
                AllowOfflineAccess = true,
                // enable all profile claims to be sent in the ID token if this is configured false the 
                // client must make a separate call to the idserver endpoint /connect/userinfo to acces the user claims
                AlwaysIncludeUserClaimsInIdToken = true,
                AlwaysSendClientClaims = false
            };

            var deafaultClient = new Client {
                ClientId = "oauthClient",
                ClientName = "Example Client Credentials Client Application",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = new List<Secret> {
                    new Secret("superSecretPassword".Sha256())},
                AllowedScopes = new List<string> {"customAPI.read"}
            };

            return new List<Client> { angularFrontendClient, mvcFrontendClient, deafaultClient, postmanClient};
        }
    }
}
