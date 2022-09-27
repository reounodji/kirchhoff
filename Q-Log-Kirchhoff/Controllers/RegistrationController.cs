using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Models;
using MVC.Repositories.Interfaces;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    /// <summary>
    /// Handles the process of registrating a vehicle.
    /// </summary>
    /// <remarks>The suggestion of forwarding agencies is done in the registrationHub using SignalR and js.</remarks>
    public class RegistrationController : Controller
    {
        private const int _InputPage = 6;
        private const int _DataCheckPage = 7;
        private const int _FinalPage = 6; //8;

        private readonly ILogger<RegistrationController> _logger;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public RegistrationController(ILogger<RegistrationController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Displays the registration page. Depending on the page, a different content partial will be shown to the user.
        /// </summary>
        /// <param name="message">Optional message</param>
        /// <param name="page">The page to display. On default 1 which ist the language selection.</param>
        /// <returns></returns>
        public IActionResult Index(string message = "", int page = 1)
        {
            ViewBag.Message = message;
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IRegistrationFacade>();
                    var model = facade.GetRegistrationViewModel();
                    model.ApproachTyp = Data.Enums.EApproachTyp.Supplier;
                    ViewData["RegistrationPage"] = page;
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to display the registration page: " + page + ". Message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                return View();
            }
        }


        /// <summary>
        /// Called when one of the forms is submitted. This can be the inputScreen or the DataCheck.
        /// </summary>
        /// <param name="model">The Registration data that the user entered</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(RegistrationViewModel model)
        {
            if (ModelState.IsValid)
            {
                // edit will be true, wenn this call comes from the DataCheck page. This means that the user wants to edit his information.
                if(model.Edit)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var _loadingStationsRepository = scope.ServiceProvider.GetRequiredService<ILoadingStationsRepository>();
                        model.LoadingStations = _loadingStationsRepository.GetAll();
                    }
                    model.Edit = false;
                    ViewData["RegistrationPage"] = _InputPage;
                    return View(model);
                }
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var facade = scope.ServiceProvider.GetRequiredService<IRegistrationFacade>();
                        facade.CheckForUnknownInput(model);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while trying to check for unknown input. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                }
                ViewData["RegistrationPage"] = _DataCheckPage; 
                return View(model);
            }
            else
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _loadingStationsRepository = scope.ServiceProvider.GetRequiredService<ILoadingStationsRepository>();
                    model.LoadingStations = _loadingStationsRepository.GetAll();
                }
                ViewData["RegistrationPage"] = _InputPage;
                return View(model);
            }
        }


        /// <summary>
        /// Adds the registration from the users data
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddRegistration(RegistrationViewModel model)
        {
            int page = 6;
            if (ModelState.IsValid)
            {
                _logger.LogInformation("Adding a new Registration.");
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var facade = scope.ServiceProvider.GetRequiredService<IRegistrationFacade>();
                        await facade.AddRegistrationFromViewModel(model);
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while trying to add a registration. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                    return RedirectToAction("Index", new { message = "Fehler beim Hinzufügen der Anmeldung", page = _FinalPage });
                }

                return RedirectToAction("Index", new { message = "", page = _FinalPage });
            }
            ViewData["RegistrationPage"] = _InputPage; // _InputPage;

            return RedirectToAction("Index", new { model });
          //  return View(model);
        }



        /// <summary>
        /// Changes the culture of the client. Used to be able to choose between different languages. Localizer uses this.
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public IActionResult SetLanguage(string culture, int page = 1)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue( new RequestCulture(culture) ),
                new CookieOptions { }             // Tests if language still swaps unwanted
                //new CookieOptions { Expires = DateTimeOffset.UtcNow.AddMinutes(30) }  
                );
            return RedirectToAction("Index", new {page});
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}