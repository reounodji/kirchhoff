using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Models;
using System;
using System.Linq;
using System.Threading;

namespace MVC.Controllers
{
    /// <summary>
    /// Handles some of the actions performed in the Processinglist.
    /// </summary>
    /// <remarks>Most actions are handled in the ProcessingHub as they are initiated through SignalR and Javascript</remarks>
    [Authorize(Roles = "CanUseProcessingList")]
    public class ProcessingController : Controller
    {
        private readonly ILogger<ProcessingController> _logger;
        private readonly IServiceProvider _serviceProvider;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public ProcessingController(ILogger<ProcessingController> logger, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }


        /// <summary>
        /// Displays the processinglist
        /// </summary>
        /// <param name="msg">Optional error message</param>
        /// <returns></returns>
        public IActionResult Index(string msg = "")
        {
            var processListFilter = Request.Cookies["processListFilter"];
            if (msg != "")
            {

                Response.Cookies.Append("processListFilter", msg);
                processListFilter = msg;
            }

            ViewBag.ErrorMessage = "";
            ViewBag.processListFilter = processListFilter ?? "";
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _processingFacade = scope.ServiceProvider.GetRequiredService<IProcessingFacade>();
                    var _localizationFacade = scope.ServiceProvider.GetRequiredService<ILocalizationFacade>();

                    ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
                    var model = FilterData(_processingFacade.GetProcessingViewModel(), processListFilter);
                    return View(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to display the processing list. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "\nFehler beim Laden der Anmeldungen.";
                return View();
            }
        }

        private ProcessingViewModel FilterData(ProcessingViewModel processingViewModel, string processListFilter)
        {



            if (String.IsNullOrEmpty(processListFilter))
            {
                processingViewModel.Registrations = processingViewModel.Registrations.Where(item => item.ID == -1000).ToList();

            }
            else
            {
                switch (processListFilter)
                {
                    case "leergut":
                        processingViewModel.Registrations = processingViewModel.Registrations.Where(item => item.GoodsReceiptCustomerEmpties == true && (item.StatusCall == null || item.StatusCall < 3)).OrderBy(item => item.TimeOfRegistration).ToList();

                        break;

                    case "anlieferung":
                        processingViewModel.Registrations = processingViewModel.Registrations.Where(item => item.GoodsReceiptdelivery == true && AnlieferungFilter(item)).OrderBy(item => item.TimeOfRegistration).ToList();

                        break;
                    case "versand":
                        processingViewModel.Registrations = processingViewModel.Registrations.Where(item => item.LoadCustomerPickup == true && VersandFilter(item)).OrderBy(item => item.TimeOfRegistration).ToList();

                        break;
                    default:
                        processingViewModel.Registrations = processingViewModel.Registrations.Where(item => item.GoodsReceiptCustomerEmpties == false && item.GoodsReceiptdelivery == false && item.LoadCustomerPickup == false && item.LoadEmptiesCollection == false).OrderBy(item => item.TimeOfRegistration).ToList();

                        break;
                }
            }

            return processingViewModel;

        }

        Func<Data.Entities.OpenRegistration, bool> AnlieferungFilter = (item) =>
          {
              return !item.GoodsReceiptCustomerEmpties ? (item.StatusCall == null || item.StatusCall < 3) : (item.StatusCall < 5 || item.StatusCall == null);
          };


        Func<Data.Entities.OpenRegistration, bool> VersandFilter = (item) =>
        {
            int optionSelectedCount = (item.GoodsReceiptCustomerEmpties ? 1 : 0) + (item.LoadCustomerPickup ? 1 : 0) + (item.GoodsReceiptdelivery ? 1 : 0);
            if (optionSelectedCount == 1)
            {
                return (item.StatusCall == null || item.StatusCall < 3);
            }
            if (optionSelectedCount == 2)
            {
                return (item.StatusCall == null || item.StatusCall < 5);
            }
            if (optionSelectedCount == 3)
            {
                return (item.StatusCall == null || item.StatusCall < 8);
            }
            return (item.StatusCall < 5 || item.StatusCall == null);
        };
    }
}