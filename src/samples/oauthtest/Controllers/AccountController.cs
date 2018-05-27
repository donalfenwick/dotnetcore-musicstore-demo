 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace oauthtest.Controllers
{
    public class AccountController : Controller
    {
        // do login against identity server
        public IActionResult Login()
        {
            // return a challange for the open id connect auth scheme using 
            // the name the middleware was registerd with
            return Challenge(new AuthenticationProperties
            {
                RedirectUri = Url.Action("index","home")

            }, "oidc");
        }

        // do logout against the identity server
        public IActionResult Logout()
        {
            return SignOut(new AuthenticationProperties
            {
                RedirectUri = Url.Action("index", "home")
            }, "cookie", "oidc");
        }
    }
}
