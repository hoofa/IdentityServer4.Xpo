using Microsoft.Owin.Security.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Test.Framework.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [Authorize]
        public ActionResult Privacy()
        {
            return View();
        }

        public void Login()
        {
            if (!Request.IsAuthenticated)
            {
                HttpContext.GetOwinContext().Authentication.Challenge(
                    //new AuthenticationProperties { RedirectUri = "/home/privacy" },
                    OpenIdConnectAuthenticationDefaults.AuthenticationType);
            }
        }

        public void Logout()
        {
            Request.GetOwinContext()
           .Authentication
           .SignOut(//new AuthenticationProperties { RedirectUri = "http://localhost:65061" },
            HttpContext.GetOwinContext()
                           .Authentication.GetAuthenticationTypes()
                           .Select(o => o.AuthenticationType).ToArray());
        }
    }
}
