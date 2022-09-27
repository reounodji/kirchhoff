using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using System;
using System.Threading;

namespace MVC.Controllers
{
    /// <summary>
    /// Handles actions performed in the "Historie"-Area.
    /// </summary>
    /// <remarks>Some actions such as the "Auswahl exportieren" and "Erweitert filtern" are done on the client with js.</remarks>
    //[Authorize(Roles = "CanUseHistory")]
    public class HistoryController : Controller
    {
        private readonly ILogger<HistoryController> _logger;
        private readonly IServiceProvider _serviceProvider;
        

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="serviceProvider"></param>
    public HistoryController(ILogger<HistoryController> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Displays the history with entries within the timeframe from start to end.
        /// </summary>
        /// <param name="start">Date of the earliest entries</param>
        /// <param name="end">Date of the latest entries</param>
        /// <returns></returns>
        public IActionResult Index(DateTime start = new DateTime(), DateTime end = new DateTime(), string msg = "", string vehiculeNr = "", string firstname = "", string lastname = "", string phonenumber = "", string forwarder = "", string customer = "")
        {
            ViewBag.ErrorMessage = msg;
            ViewBag.vehiculeNr = vehiculeNr;
            ViewBag.firstname = firstname;
            ViewBag.lastname = lastname;
            ViewBag.phonenumber = phonenumber;
            ViewBag.forwarder = forwarder;
            ViewBag.customer = customer;

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _historyFacade = scope.ServiceProvider.GetRequiredService<IHistoryFacade>();
                    var _localizationFacade = scope.ServiceProvider.GetRequiredService<ILocalizationFacade>();

                    ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
                    if (start == end && start == new DateTime())
                    {
                        start = _historyFacade.GetDefaultHistoryStart();
                        end = DateTime.Now;
                    }
                    else if (end == new DateTime())
                    {
                        end = DateTime.Now;
                    }
                    else if (start > end)
                    {
                        end = start;
                    }

                    ViewBag.start = start;
                    ViewBag.end = end;

                    var model = _historyFacade.GetHistoryViewModel(start, end, vehiculeNr ?? "" , firstname ?? "", lastname ?? "", phonenumber ?? "", forwarder ?? "", customer ?? "");
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while loading the history. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "Fehler beim Laden der Historie.";
                return View();
            }
        }


        /// <summary>
        /// Export the entire registration history. 
        /// </summary>
        /// <returns></returns>
        //[Authorize(Roles = "CanExportHistory")]
        public IActionResult ExportRegistrationHistoryCSV(DateTime start = new DateTime(), DateTime end = new DateTime(), string msg = "", string vehiculeNr = "", string firstname = "", string lastname = "", string phonenumber = "", string forwarder = "", string customer = "")
        {
            _logger.LogInformation("Exporting Registration History CSV");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _historyFacade = scope.ServiceProvider.GetRequiredService<IHistoryFacade>();

                    var model = _historyFacade.GetHistoryViewModel(start, end, vehiculeNr ?? "", firstname ?? "", lastname ?? "", phonenumber ?? "", forwarder ?? "", customer ?? "");

                    var stream = _historyFacade.ExportRegistrationHistoryCSV(model.Registrations);
                    var response = File(stream, "configuration/ExportRegistrationHistoryCSV", "GesamteAnmeldeHistorieExport_" + DateTime.Now.ToString() + ".csv");
                    return response;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while exporting registration history csv. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                return RedirectToAction("Index", new { msg = "Fehler beim Herunterladen der Historie." });
            }
        }
    }
}