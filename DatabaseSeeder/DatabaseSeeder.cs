using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MusicStoreDemo.Database;
using MusicStoreDemo.Database.Entities;
using MusicStoreDemo.Database.Entities.Relationships;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System.IO;
using System.Text;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.AspNetCore.Identity;
using DatabaseSeeder.IdentityServerDefaults;
using IdentityServer4.EntityFramework.Mappers;
using System.Security.Claims;
using MusicStoreDemo.Common;

namespace DatabaseSeeder
{
    
    public interface IDatabaseSeeder{
        Task Seed();
    }
    public class DatabaseSeeder: IDatabaseSeeder{
        private readonly IConfiguration _configuration;
        private readonly UserManager<DbUser> _userManager;
        private readonly RoleManager<DbRole> _roleManager;
        private readonly MusicStoreDbContext _context;
        private readonly ILogger<DatabaseSeeder> _logger;
        private readonly ConfigurationDbContext _identityServerConfigContext;
        private readonly PersistedGrantDbContext _identityServerPersistedGrantContext;

        public DatabaseSeeder(
            IConfiguration configuration,
            UserManager<DbUser> userManager,
            RoleManager<DbRole> roleManager,
            MusicStoreDbContext context, 
            ILogger<DatabaseSeeder> logger, 
            ConfigurationDbContext identityServerConfigContext,
            PersistedGrantDbContext identityServerPersistedGrantContext)
        {
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
            _identityServerPersistedGrantContext = identityServerPersistedGrantContext;
            _identityServerConfigContext = identityServerConfigContext;
            _context = context;
            _logger = logger;
        }

        public async Task Seed()
        {
            
            // create the database contexts and apply any pending migrations
            if (_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
            }

            if (_identityServerConfigContext.Database.GetPendingMigrations().Any())
            {
                _identityServerConfigContext.Database.Migrate();
            }

            if (_identityServerPersistedGrantContext.Database.GetPendingMigrations().Any())
            {
                _identityServerPersistedGrantContext.Database.Migrate();
            }
            if ((await _roleManager.FindByNameAsync("AdminUserClassicRole")) == null)
            {
                var addedRole = new DbRole { Name = "AdminUserClassicRole", Description = "classic style admin role" };
                var addRoleResult = await _roleManager.CreateAsync(addedRole);
                if (addRoleResult.Succeeded)
                {
                    await _roleManager.AddClaimAsync(addedRole, new Claim(ClaimTypes.Role, "AdminUSerClassicRoleClaim"));
                }
            }
            
            string emailUsername = "testuser@mysite.com";
            DbUser user = await _userManager.FindByNameAsync(emailUsername);
            if (user == null)
            {
                var result = await _userManager.CreateAsync(new DbUser
                {
                    UserName = emailUsername,
                    DateOfBirth = new DateTime(1970, 1, 1),
                    Email = emailUsername,
                    Firstname = "test",
                    LockoutEnabled = true,
                    Surname = "user",
                    EmailConfirmed = true,
                    TwoFactorEnabled = false
                }, "Pa$$word1");

                    
                if (result.Succeeded)
                {
                    user = await _userManager.FindByNameAsync(emailUsername);

					await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, MusicStoreConstants.Roles.AdminUser));
					await _userManager.AddClaimAsync(user, new Claim(ClaimTypes.Role, MusicStoreConstants.Roles.TestUser));
					await _userManager.AddClaimAsync(user, new Claim(MusicStoreConstants.CLaimTypes.MusicStoreApiReadAccess, "true"));
					await _userManager.AddClaimAsync(user, new Claim(MusicStoreConstants.CLaimTypes.MusicStoreApiWriteAccess, "true"));
                    
                    var addRoleResult = await _userManager.AddToRoleAsync(user, "AdminUserClassicRole");
                }
                else
                {
                    _logger.LogError("Error adding user - error: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            
            if (!_identityServerConfigContext.Clients.Any())
            {
                foreach (var client in IdentityServerClients.GetClients())
                {
                    _identityServerConfigContext.Clients.Add(client.ToEntity());
                }
                _identityServerConfigContext.SaveChanges();
            }

            if (!_identityServerConfigContext.IdentityResources.Any())
            {
                foreach (var resource in IdentityServerResources.GetIdentityResources())
                {
                    _identityServerConfigContext.IdentityResources.Add(resource.ToEntity());
                }
                _identityServerConfigContext.SaveChanges();
            }

            if (!_identityServerConfigContext.ApiResources.Any())
            {
                foreach (var resource in IdentityServerResources.GetApiResources())
                {
                    _identityServerConfigContext.ApiResources.Add(resource.ToEntity());
                }
                _identityServerConfigContext.SaveChanges();
            }

			ContentCreator contentCreator = new ContentCreator(_context);

			if (await _context.AlbumGroups.CountAsync() == 0)
            {
				//create default groups
				await _context.AlbumGroups.AddAsync(new DbAlbumGroup
				{
					Key = "FEATURED_ALBUMS",
					Name = "Featured albums",
					CreatedUtc = DateTime.UtcNow,
					UpdatedUtc = DateTime.UtcNow
				});
				await _context.AlbumGroups.AddAsync(new DbAlbumGroup
                {
                    Key = "NEW_ALBUMS",
                    Name = "New albums",
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                });
				await _context.AlbumGroups.AddAsync(new DbAlbumGroup
                {
                    Key = "BESTSELLING_ALBUMS",
                    Name = "Bestselling albums",
                    CreatedUtc = DateTime.UtcNow,
                    UpdatedUtc = DateTime.UtcNow
                });
				await _context.SaveChangesAsync();
            }

            if(await _context.Genres.CountAsync() == 0){
				await contentCreator.CreateGenres("Rock", "Pop", "Indie", "Funk", "Grunge","Electronic", "Punk", "Alternative");
            }

            if (await _context.Artists.CountAsync() == 0)
            {

				await contentCreator.CreateSeedContent();
            }
        }

              
        
    }

    internal struct TrackStruct{
        public string Title;
        public int? TrackNo;
        public int DurationInSec;
        public TrackStruct(string title, int durationInSec = 0){
            TrackNo = null;
            Title = title;
            DurationInSec= durationInSec;
        }
        public TrackStruct(string title, string duration)
        {
            int durationInSec = 0;
            try
            {
                string[] timeComponents = duration.Split(':');
                if (timeComponents.Length == 2)
                {
                    int mins = int.Parse(timeComponents[0]);
                    int seconds = int.Parse(timeComponents[1]);
                    durationInSec = (mins * 60) + seconds;
                }
            }catch{}

            TrackNo = null;
            Title = title;
            DurationInSec = durationInSec;
        }
    }
}