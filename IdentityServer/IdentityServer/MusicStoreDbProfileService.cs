using System;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MusicStoreDemo.Database.Entities;
using System.Collections.Generic;
using MusicStoreDemo.Common;

namespace MusicStoreDemo.Api.IdentityServer
{
    // see the following linka for info about a customer profile service
    // http://docs.identityserver.io/en/release/reference/profileservice.html
    // https://github.com/IdentityServer/IdentityServer4/blob/dev/src/IdentityServer4/Test/TestUserProfileService.cs

    public class MusicStoreDbProfileService : IProfileService
    {
        readonly UserManager<DbUser> _usermanager;
        readonly ILogger<MusicStoreDbProfileService> _logger;

        public MusicStoreDbProfileService(UserManager<DbUser> usermanager, ILogger<MusicStoreDbProfileService> logger)
        {
            _logger = logger;
            _usermanager = usermanager;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            

            context.LogProfileRequest(_logger);

            //if (context.RequestedClaimTypes.Any())
            {
                var requestedClaims = context.RequestedClaimTypes;
                string userId = context.Subject?.GetSubjectId();
                if (userId != null)
                {
                    var user = await _usermanager.FindByIdAsync(userId);
                    if (user != null)
                    {
                        IList<Claim> claims = await _usermanager.GetClaimsAsync(user);

                        claims.Add(new Claim("birthdate", user.DateOfBirth.HasValue ? user.DateOfBirth.Value.ToString("yyyy-MM-dd") : ""));
                        claims.Add(new Claim("email", user.Email));
                        claims.Add(new Claim("given_name", user.Firstname));
                        claims.Add(new Claim("family_name", user.Surname));
                        claims.Add(new Claim("preferred_username", user.UserName));
                        claims.Add(new Claim("email_verified", user.EmailConfirmed.ToString()));
                        //claims.Add(new Claim("gender", user.));
                        claims.Add(new Claim("name", $"{user.Firstname ?? string.Empty}  {user.Surname ?? string.Empty}"));
                        //claims.Add(new Claim("updated_at", user.));
                        claims.Add(new Claim(ClaimTypes.Name, user.UserName));

						// example of populating a dynamicly calculated claim to be included in the access_token for a client resource
						if (context.RequestedClaimTypes.Contains(MusicStoreConstants.CLaimTypes.MusicStoreApiAgeDemographic))
						{
							string ageDemographic = "unknown";
							if (user.DateOfBirth.HasValue)
							{
								int age = DateTime.Now.Year - user.DateOfBirth.Value.Year;
								if (age < 12)
								{
									ageDemographic = "0-11";
								}
								else if (age >= 12 && age < 18)
								{
									ageDemographic = "12–17";
								}
								else if (age >= 18 && age < 25)
								{
									ageDemographic = "18–24";
								}
								else if (age >= 25 && age < 35)
								{
									ageDemographic = "25–34";
								}
								else if (age >= 35 && age < 45)
								{
									ageDemographic = "35–44";
								}
								else if (age >= 45 && age < 55)
								{
									ageDemographic = "45–54";
								}
								else if (age >= 55 && age < 65)
								{
									ageDemographic = "55–64";
								}
								else
								{
									ageDemographic = "65+";
								}
							}
							claims.Add(new Claim(MusicStoreConstants.CLaimTypes.MusicStoreApiAgeDemographic, ageDemographic));
						}

                        context.AddRequestedClaims(claims);
                    }
                }
            }

            context.LogIssuedClaims(_logger);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            _logger.LogDebug("IsActive called from: {caller}", context.Caller);
            var userId = context.Subject?.GetSubjectId();
            if (userId != null)
            {
                var user = await _usermanager.FindByIdAsync(userId);
                context.IsActive = user != null;
            }

        }
    }
}
