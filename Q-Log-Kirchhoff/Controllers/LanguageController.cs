using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class LanguageController : Controller
    {
        public IActionResult SetLanguage(string code)
        {
            var culture = new CultureInfo(code);
            Response.Cookies.Append(
              CookieRequestCultureProvider.DefaultCookieName,
              CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
              new CookieOptions { }             // Expires = DateTimeOffset.UtcNow.AddMinutes()
              );
            return Redirect(Request.Headers["Referer"]);
        }

        
    }
}