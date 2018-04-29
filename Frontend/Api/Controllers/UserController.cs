using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicStoreDemo.Common.Models.User;
using MusicStoreDemo.Database.Entities;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Common.Models.User;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MusicStoreDemo.Api.Controllers
{
    [Route("api/user/me")]
    public class UserController : ControllerBase
    {
        private readonly UserManager<DbUser> _userManager;
        private readonly RoleManager<DbRole> _roleManager;
        private readonly ILogger<UserController> _log;

        public UserController(UserManager<DbUser> userManager, RoleManager<DbRole> roleManager, ILogger<UserController> log)
        {
            _log = log;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [Authorize]
        [Produces(typeof(UserProfile))]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            DbUser usr = await this._userManager.GetUserAsync(this.User);
            if (usr != null)
            {
                return Ok(new UserProfile
                {
                    Username = usr.UserName,
                    Firstname = usr.Firstname,
                    Surname = usr.Surname,
                    EmailAddress = usr.Email,
                    PhoneNumber = usr.PhoneNumber,
                    DateOfBirth = usr.DateOfBirth
                });
            }
            else
            {
                return NotFound();
            }
        }

        [Authorize]
        [Produces(typeof(UserProfile))]
        [HttpPost("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody]UserProfile profile)
        {
            DbUser usr = await this._userManager.GetUserAsync(this.User);
            if (usr != null)
            {
                if (ModelState.IsValid)
                {
                    usr.Firstname = profile.Firstname;
                    usr.Surname = profile.Surname;
                    usr.PhoneNumber = profile.PhoneNumber;
                    usr.DateOfBirth = profile.DateOfBirth;
                    await this._userManager.UpdateAsync(usr);
                
                    return Ok(new UserProfile
                    {
                        Username = usr.UserName,
                        Firstname = usr.Firstname,
                        Surname = usr.Surname,
                        EmailAddress = usr.Email,
                        PhoneNumber = usr.PhoneNumber,
                        DateOfBirth = usr.DateOfBirth
                    });
                }else{
                    return BadRequest(modelState:ModelState);
                }
            }
            else
            {
                _log.LogDebug("User not found in database");
                return NotFound();
            }
        }

        [HttpGet("identity")]
        public IActionResult Get(int id)
        {
            // TODO: this is used for testing introspection with the identity system during dev, remove it later.
            UserIdentity model = new UserIdentity();
            var isAdminRole = User.IsInRole("AdminUser");
            var isTestRole = User.IsInRole("TestUser");
            if (User.Identity.IsAuthenticated)
            {
                //    DbUser dbUser = await _userManager.GetUserAsync(User);
                //    IList<string> roles = await _userManager.GetRolesAsync(dbUser);
                //    model.UserName = User.Identity.Name;
                //    model.Roles.AddRange(roles);
                model.Claims = User.Claims.Select(x => new UserClaim
                {
                    Type = x.Type,
                    Value = x.Value,
                    Issuer = x.Issuer
                }).ToList();
            }
            return Ok(model);
        }
    }
}
