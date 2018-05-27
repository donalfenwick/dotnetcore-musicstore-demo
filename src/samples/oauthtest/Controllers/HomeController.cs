using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using oauthtest.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using MusicStoreDemo.Database.Entities;

namespace oauthtest.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _log;
        private readonly UserManager<DbUser> _userManager;
        public HomeController(ILogger<HomeController> log, UserManager<DbUser> userManager){
            _log = log;
            _userManager = userManager;
        }
        public IActionResult Index()
        {

            return View();
        }

        [Authorize]
        public async Task<IActionResult> UserInfo()
        {
            ViewData["Message"] = "User details page.";
            if(User.Identity.IsAuthenticated){
                string accessToken = await HttpContext.GetTokenAsync("access_token");
                string idToken = await HttpContext.GetTokenAsync("id_token");
                this._log.LogDebug($"$accessToken: {accessToken}");
                ViewBag.accessToken = accessToken;
                ViewBag.idToken = idToken;
                var claims = User.Claims;
                var user = await _userManager.GetUserAsync(User);
                if(user != null){
                    ViewBag.dbusername = user.UserName;
                }

            }
            return View();
        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
