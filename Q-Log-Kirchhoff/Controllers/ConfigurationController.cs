using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Data.Entities;
using MVC.Models.ConfigurationViewModels;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    /// <summary>
    /// Handles all actions perfomed while beeing in the "Konfiguration"-Area.
    /// </summary>
    /// <remarks>Requires role "CanUseConfig"</remarks>
    [Authorize(Roles = "CanUseConfig")]
    public class ConfigurationController : Controller
    {
        private readonly ILogger<ConfigurationController> _logger;

        private readonly IServiceProvider _serviceProvider;

        private readonly ILocalizationFacade _localizationFacade;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public ConfigurationController(ILogger<ConfigurationController> logger, IServiceProvider serviceProvider, ILocalizationFacade localizationFacade)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _localizationFacade = localizationFacade;
        }

        /// <summary>
        /// First page of the configuration. Will show the general settings.
        /// </summary>
        /// <param name="msg">Error message to display</param>
        /// <returns></returns>
        public IActionResult Index(string msg = "")
        {
            ViewBag.ErrorMessage = msg;
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetGeneralSettingsViewModel();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to get GeneralSettingsViewModel. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "\nFehler beim Laden der allgemeinen Einstellungen.";
            }
            return View();
        }


        /// <summary>
        /// Saves the general settings from the ViewModel
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> SetGeneralSettings(GeneralSettingsViewModel model)
        {
            var errMessage = "";
            _logger.LogInformation("Saving the general settings from the ViewModel");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.SetGeneralSettings(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to save the general settings. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                errMessage = "Fehler beim Speichern der Einstellungen.";
            }
            return RedirectToAction("Index", new { msg = errMessage });
        }


        #region Terminal
        /// <summary>
        /// Displays the settings for the terminal.
        /// </summary>
        /// <param name="msg">Optional error message</param>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult Terminal(string msg="")
        {
            ViewBag.ErrorMessage = msg;
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetTerminalSettingsViewModel();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to get the terminal settings. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "Fehler beim Laden der Einstellungen.";
            }
            
            return View();
        }


        /// <summary>
        /// Saves the settings for the terminal.
        /// </summary>
        /// <param name="settings"></param>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> SetTerminalSettings(TerminalSettings settings)
        {
            var errMessage = "";
            _logger.LogInformation("Saving the terminal settings.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.SetTerminalSettings(settings);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to save the terminal settings. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                errMessage = "Fehler beim Speichern der Einstellungen.";
            }
            return RedirectToAction("Terminal", new { msg = errMessage });
        }
        #endregion


        #region LoadingStations
        /// <summary>
        /// Displays the LoadingStations settings
        /// </summary>
        /// <param name="msg"></param>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult LoadingStations(string msg = "")
        {
            ViewBag.ErrorMessage = msg;
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetLoadingStationsViewModel();
                    return View(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to get the LoadingStations view model. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "\nFehler beim Laden der Einstellungen.";
            }

            return View();
        }


        /// <summary>
        /// Displays the edit view for the LoadingStation with the selected ID.
        /// </summary>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult EditLoadingStation(int id)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetLoadingStation(id);
                    if (model == null)
                    {
                        _logger.LogError("Could not show EditLoadingStation View because the LoadingStation with id: " + id + " could not be found.");
                        var error = "Das Tor konnte nicht geladen werden.";
                        return RedirectToAction("LoadingStations", new { msg = error });
                    }
                    return View(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not display EditLoadingStation View. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                var error = "Fehler beim Laden des Tores";
                return RedirectToAction("LoadingStations", new { msg = error });
            }
        }


        /// <summary>
        /// Displays the View for adding a new LoadingStation.
        /// </summary>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult AddLoadingStation()
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            return View(new LoadingStation());
        }


        /// <summary>
        /// Adds the new LoadingStation to the db.
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> AddLoadingStation(LoadingStation model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation("Adding LoadingStation to DB.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.AddLoadingStation(model);
                    return RedirectToAction("LoadingStations");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to add a new LoadingStation to the DB. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = e.Message;
                return View(model);
            }
        }


        /// <summary>
        /// Deletes the LoadingStation with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> DeleteLoadingStation(int id)
        {
            _logger.LogInformation("Deleting LoadingStation with id: " + id);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.DeleteLoadingStation(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete LoadingStation with id: " + id + " Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Löschen des Tores.";
            }
            return RedirectToAction("LoadingStations", new { msg = ViewBag.ErrorMessage });
        }


        /// <summary>
        /// Saves the properties of the LoadingStation.
        /// </summary>
        /// <param name="LoadingStation"></param>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> EditLoadingStation(LoadingStation LoadingStation)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            if (!ModelState.IsValid)
                return View(LoadingStation);

            _logger.LogInformation("Saving the changes made to the LoadingStation with id: " + LoadingStation.ID);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.EditLoadingStation(LoadingStation);
                    return RedirectToAction("LoadingStations");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to save the changes made to the LoadingStation with id: " + LoadingStation.ID + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Speichern der Änderungen.";
                return View(LoadingStation);
            }
        }


        /// <summary>
        /// Imports LoadingStations from a csv file that is passed along as a formfile
        /// </summary>
        /// <param name="file">IFormFile that contains the csv</param>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> ImportLoadingStationsFromCSV(IFormFile file)
        {
            _logger.LogInformation("Importing LoadingStations from a csv file.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.ImportLoadingStationsFromCSV(file);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to import LoadingStations from a csv file. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Importieren der CSV-Datei. Stellen Sie sicher, dass die Datei der Beschreibung entspricht.";
            }
            return RedirectToAction("LoadingStations", new { msg = ViewBag.ErrorMessage });
        }
        #endregion


        #region Gates
        /// <summary>
        /// Displays the gates settings
        /// </summary>
        /// <param name="msg"></param>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult Gates(string msg = "")
        {
            ViewBag.ErrorMessage = msg;
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetGatesViewModel();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to get the gates view model. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "\nFehler beim Laden der Einstellungen.";
            }
            
            return View();
        }


        /// <summary>
        /// Displays the edit view for the gate with the selected ID.
        /// </summary>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult EditGate(int id)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    GateViewModel gateViewModel = new GateViewModel
                    {
                        Gate = facade.GetGate(id),
                        LoadingStations = facade.GetAllLoadingStations()
                    };

                    if (gateViewModel.Gate == null || gateViewModel.LoadingStations == null)
                    {
                        _logger.LogError("Could not show EditGate View. Gate id: " + id);
                        var error = "Das Tor konnte nicht geladen werden.";
                        return RedirectToAction("Gates", new { msg = error });
                    }
                    return View(gateViewModel);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not display EditGate View. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                var error = "Fehler beim Laden des Tores";
                return RedirectToAction("Gates", new { msg = error });
            }
        }


        /// <summary>
        /// Displays the View for adding a new gate.
        /// </summary>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult AddGate()
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();

                    GateViewModel gateViewModel = new GateViewModel
                    {
                        Gate = new Gate(),
                        LoadingStations = facade.GetAllLoadingStations()
                    };

                    if (gateViewModel.LoadingStations == null)
                    {
                        _logger.LogError("Could not show AddGate View because Loadingstations could not be loaded.");
                        var error = "Die Ladestationen konnten nicht geladen werden.";
                        return RedirectToAction("Gates", new { msg = error });
                    }
                    return View(gateViewModel);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not display Addgate View. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                var error = "Fehler beim Laden";
                return RedirectToAction("Gates", new { msg = error });
            }
        }


        /// <summary>
        /// Adds the new gate to the db.
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> AddGate(GateViewModel model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation("Adding gate to DB.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.AddGate(model.Gate);
                    return RedirectToAction("Gates");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to add a new gate to the DB. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = e.Message;
                return View(model);
            }
        }


        /// <summary>
        /// Deletes the gate with the given ID
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> DeleteGate(int id)
        {
            _logger.LogInformation("Deleting gate with id: " + id);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.DeleteGate(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete gate with id: " + id + " Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Löschen des Tores.";
            }
            return RedirectToAction("Gates", new { msg = ViewBag.ErrorMessage });
        }


        /// <summary>
        /// Saves the properties of the gate.
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> EditGate(GateViewModel model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation("Saving the changes made to the gate with id: " + model.Gate.ID);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.EditGate(model.Gate);
                    return RedirectToAction("Gates");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to save the changes made to the gate with id: " + model.Gate.ID + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message );
                ViewBag.ErrorMessage = "Fehler beim Speichern der Änderungen.";
                return View(model);
            }
        }


        /// <summary>
        /// Imports gates from a csv file that is passed along as a formfile
        /// </summary>
        /// <param name="file">IFormFile that contains the csv</param>
        /// <remarks>Requires role "CanModifyAllSettings"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> ImportGatesFromCSV(IFormFile file)
        {
            _logger.LogInformation("Importing gates from a csv file.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.ImportGatesFromCSV(file);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to import gates from a csv file. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Importieren der CSV-Datei. Stellen Sie sicher, dass die Datei der Beschreibung entspricht.";
            }
            return RedirectToAction("Gates", new { msg = ViewBag.ErrorMessage });
        }
        #endregion


        #region ForwardingAgencies
        /// <summary>
        /// Displays the ForwardingAgencies view.
        /// </summary>
        /// <param name="msg">Optional error message</param>
        /// <remarks>Requires one of the roles "CanModifyAllSettings" or "CanInspectCompanyLists"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings,CanInspectCompanyLists")]
        public IActionResult ForwardingAgencies(string msg = "")
        {
            ViewBag.ErrorMessage = msg;
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetForwardingAgenciesIndexViewModel();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while getting the forwardingAgenciesIndes viewmodel. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "Fehler beim Laden der Speditionen.";
            }

            return View();
        }


        /// <summary>
        /// Displays the view for adding a new forwarding agency
        /// </summary>
        /// <remarks>Requires one of the roles "CanModifyAllSettings" or "CanModifyCompanyLists"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        public IActionResult AddForwardingAgency()
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetForwardingAgencyViewModel();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to get a forwardingAgency viewmodel. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
            }
            return RedirectToAction("ForwardingAgencies", new { msg = "Fehler beim Laden der Ansicht zum Hinzufügen von Speditionen." });
        }


        /// <summary>
        /// Adds a new forwarding agency to the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> AddForwardingAgency(ForwardingAgencyViewModel model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation("Adding new forwarding agency to DB. Name: " + model.ForwardingAgency?.Name);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.AddForwardingAgency(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add ForwardingAgency. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Hinzufügen der Spedition.";
                return View(model);
            }
            return RedirectToAction("ForwardingAgencies");
        }


        /// <summary>
        /// Displays the view for editing the forwarding agency with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Requires one of the roles "CanModifyAllSettings, CanModifyCompanyLists"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        public IActionResult EditForwardingAgency(int id)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetForwardingAgencyViewModel(id);
                    return View(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to get a forwardingAgency viewmodel for forwarding agency with id: " + id + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
            }
            return RedirectToAction("ForwardingAgencies", new { msg = "Fehler beim Laden der Ansicht zum Bearbeiten der Spedition." });
        }


        /// <summary>
        /// Saves the changes made to the forwarding agency.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> EditForwardingAgency(ForwardingAgencyViewModel model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation("Saving the changes made to the forwarding agency with id: " + model.ForwardingAgency.ID);

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.EditForwardingAgency(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not save the ForwardingAgency with id: " + model.ForwardingAgency.ID + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Speichern der Änderungen.";
                return View(model);
            }
            return RedirectToAction("ForwardingAgencies");
        }


        /// <summary>
        /// Deletes the forwarding agency with the given id.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> DeleteForwardingAgency(int id)
        {
            _logger.LogInformation("Deleting the forwarding agency with id: " + id);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.DeleteForwardingAgency(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete forwarding agency with id: " + id + " Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Löschen der Spedition.";
            }
            return RedirectToAction("ForwardingAgencies", new { msg = ViewBag.ErrorMessage });
        }


        /// <summary>
        /// Imports forwarding agencies from a csv file.
        /// </summary>
        /// <param name="file"></param>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> ImportForwardingAgenciesFromCSV(IFormFile file)
        {
            _logger.LogInformation("Importing forwarding agencies from a csv file.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.ImportForwardingAgenciesFromCSV(file);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to import forwarding agencies from csv file. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Importieren der Speditionen. Stellen Sie sicher, dass die Datei der Beschreibung entspricht.";
            }
            return RedirectToAction("ForwardingAgencies", new { msg = ViewBag.ErrorMessage });
        }
        #endregion


        #region Supplier
        /// <summary>
        /// Displays the suppliers view.
        /// </summary>
        /// <remarks>Requires on of the roles CanModifyAllSettings, CanInspectCompanyLists</remarks>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanInspectCompanyLists")]
        public IActionResult Suppliers(string msg = "")
        {
            ViewBag.ErrorMessage = msg;
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetSuppliersIndexViewModel();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to get the suppliersIndex viewmodel. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                return View();
            }
        }


        /// <summary>
        /// Displays the view for adding a new supplier.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        public IActionResult AddSupplier()
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetSupplierViewModel();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to get the supplier viewmodel to display the AddSupplier view. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                return RedirectToAction("Suppliers", new { msg = "Fehler beim Anzeigen der Eingabemaske zum Hinzufügen von Lieferanten."});
            }
        }


        /// <summary>
        /// Adds the new supplier to the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> AddSupplier(SupplierViewModel model)
        {

            _logger.LogInformation("Adding a new supplier to the DB. Name: " + model.Supplier.Name);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    if (!ModelState.IsValid)
                    {
                        ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
                        return View(model);
                    }
                    
                    await facade.AddSupplier(model);
                }
            }
            catch (Exception e)
            {
                ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
                _logger.LogError("Could not add Supplier with name: " + model.Supplier.Name + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Hinzufügen des Lieferanten.";
                return View(model);
            }
            return RedirectToAction("Suppliers");
        }

        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        public async Task<IActionResult> UpdateSupplierWithERP()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    bool success = await facade.getSupplierFromERP();
                    var model = facade.GetSuppliersIndexViewModel();


                    if (!success)
                        ViewBag.ErrorMessage = "Das updaten von der API '.../lieferanten' hat nicht funktioniert. Für weitere infos sehen Sie im Log nach.";
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to update suppliers manually from API 'lieferanten'. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim updaten der Lieferanten von der API. Mehr Details sind in dem Log.";
            }
            return RedirectToAction("Suppliers", new { msg = ViewBag.ErrorMessage });
        }

        /// <summary>
        /// Displays the edit view for the supplier with the given id.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        public IActionResult EditSupplier(int id)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetSupplier(id);
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to edit the supplier with the id: " + id + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                return RedirectToAction("Suppliers", new { msg = "Fehler beim Anzeigen der Bearbeitungsmaske für den Lieferanten mit der ID: " + id });
            }
        }


        /// <summary>
        /// Saves the changes made to the supplier.
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> EditSupplier(SupplierViewModel model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation("Saving changes for the supplier with id: " + model.Supplier.ID);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.EditSupplier(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not save changes for the supplier with the id: " + model.Supplier.ID + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage ="Fehler beim Speichern des Lieferanten.";
                return View(model);
            }
            return RedirectToAction("Suppliers");
        }


        /// <summary>
        /// Deletes the supplier with the given id.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            _logger.LogInformation("Deleting the supplier with the id: " + id);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.DeleteSupplier(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete the supplier with the id: " + id + " Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Löschen des Lieferanten mit der ID: " + id;
            }
            return RedirectToAction("Suppliers", new { msg = ViewBag.ErrorMessage });
        }


        /// <summary>
        /// Imports suppliers from a csv file
        /// </summary>
        /// <param name="file"></param>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> ImportSuppliersFromCSV(IFormFile file)
        {
            _logger.LogInformation("Importing suppliers from a csv file.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.ImportSuppliersFromCSV(file);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to import suppliers from csv file. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Importieren der Lieferanten. Stellen Sie sicher, dass die Datei der Beschreibung entspricht.";
            }
            return RedirectToAction("Suppliers", new { msg = ViewBag.ErrorMessage });
        }
        #endregion


        #region ParcelServices
        /// <summary>
        /// Displays the ParcelServices view.
        /// </summary>
        /// <param name="msg">Optional error message</param>
        /// <remarks>Requires one of the roles "CanModifyAllSettings" or "CanInspectCompanyLists"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings,CanInspectCompanyLists")]
        public IActionResult ParcelServices(string msg = "")
        {
            ViewBag.ErrorMessage = msg;
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetParcelservicesIndexViewModel();
                    return View(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while getting the ParcelServicesIndex viewmodel. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "Fehler beim Laden der Speditionen.";
            }

            return View();
        }


        /// <summary>
        /// Displays the view for adding a new ParcelService
        /// </summary>
        /// <remarks>Requires one of the roles "CanModifyAllSettings" or "CanModifyCompanyLists"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        public IActionResult AddParcelService()
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetParcelServiceViewModel();
                    return View(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to get a ParcelService viewmodel. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
            }
            return RedirectToAction("ParcelServices", new { msg = "Fehler beim Laden der Ansicht zum Hinzufügen von Paketdienste." });
        }


        /// <summary>
        /// Adds a new ParcelService to the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> AddParcelService(ParcelServiceViewModel model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation("Adding new ParcelService to DB. Name: " + model.ParcelService?.Name);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.AddParcelService(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add ParcelService. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Hinzufügen des Paketdienstes.";
                return View(model);
            }
            return RedirectToAction("ParcelServices");
        }


        /// <summary>
        /// Displays the view for editing the Parcel Service with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Requires one of the roles "CanModifyAllSettings, CanModifyCompanyLists"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        public IActionResult EditParcelService(int id)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetParcelServiceViewModel(id);
                    return View(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to get a ParcelService viewmodel for ParcelService with id: " + id + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
            }
            return RedirectToAction("ParcelServices", new { msg = "Fehler beim Laden der Ansicht zum Bearbeiten des Paketdienstes." });
        }


        /// <summary>
        /// Saves the changes made to the ParcelService.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> EditParcelService(ParcelServiceViewModel model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation("Saving the changes made to the ParcelService with id: " + model.ParcelService.ID);

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.EditParcelService(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not save the ParcelService with id: " + model.ParcelService.ID + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Speichern der Änderungen.";
                return View(model);
            }
            return RedirectToAction("ParcelServices");
        }


        /// <summary>
        /// Deletes the forwarding agency with the given id.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> DeleteParcelService(int id)
        {
            _logger.LogInformation("Deleting the parcel service with id: " + id);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.DeleteParcelService(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete ParcelService with id: " + id + " Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Löschen der Paketdienst.";
            }
            return RedirectToAction("ParcelServices", new { msg = ViewBag.ErrorMessage });
        }


        /// <summary>
        /// Imports ParcelServices from a csv file.
        /// </summary>
        /// <param name="file"></param>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> ImportParcelServicesFromCSV(IFormFile file)
        {
            _logger.LogInformation("Importing ParcelService from a csv file.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.ImportParcelServiceFromCSV(file);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to import ParcelService from csv file. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Importieren des Paketdienstes. Stellen Sie sicher, dass die Datei der Beschreibung entspricht.";
            }
            return RedirectToAction("ParcelServices", new { msg = ViewBag.ErrorMessage });
        }
        #endregion


        #region Fitters
        /// <summary>
        /// Displays the Fitters view.
        /// </summary>
        /// <param name="msg">Optional error message</param>
        /// <remarks>Requires one of the roles "CanModifyAllSettings" or "CanInspectCompanyLists"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings,CanInspectCompanyLists")]
        public IActionResult Fitters(string msg = "")
        {
            ViewBag.ErrorMessage = msg;
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetFittersIndexViewModel();
                    return View(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while getting the FittersIndex viewmodel. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "Fehler beim Laden der Monteure.";
            }

            return View();
        }


        /// <summary>
        /// Displays the view for adding a new Fitter
        /// </summary>
        /// <remarks>Requires one of the roles "CanModifyAllSettings" or "CanModifyCompanyLists"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        public IActionResult AddFitter()
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetFitterViewModel();
                    return View(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to get a Fitter viewmodel. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
            }
            return RedirectToAction("Fitters", new { msg = "Fehler beim Laden der Ansicht zum Hinzufügen von Monteuren." });
        }


        /// <summary>
        /// Adds a new Fitter to the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> AddFitter(FitterViewModel model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation("Adding new Fitter to DB. Name: " + model.Fitter?.Name);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.AddFitter(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add Fitter. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Hinzufügen des Monteurs.";
                return View(model);
            }
            return RedirectToAction("Fitters");
        }


        /// <summary>
        /// Displays the view for editing the Fitter with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>Requires one of the roles "CanModifyAllSettings, CanModifyCompanyLists"</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        public IActionResult EditFitter(int id)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetFitterViewModel(id);
                    return View(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to get a Fitter viewmodel for Fitter with id: " + id + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
            }
            return RedirectToAction("Fitters", new { msg = "Fehler beim Laden der Ansicht zum Bearbeiten des Monteur." });
        }


        /// <summary>
        /// Saves the changes made to the Fitter.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> EditFitter(FitterViewModel model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation("Saving the changes made to the Fitter with id: " + model.Fitter.ID);

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.EditFitter(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not save the Fitter with id: " + model.Fitter.ID + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Speichern der Änderungen.";
                return View(model);
            }
            return RedirectToAction("Fitters");
        }


        /// <summary>
        /// Deletes the Fitter with the given id.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> DeleteFitter(int id)
        {
            _logger.LogInformation("Deleting the fitter with id: " + id);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.DeleteFitter(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete Fitter with id: " + id + " Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Löschen der Monteurs.";
            }
            return RedirectToAction("Fitters", new { msg = ViewBag.ErrorMessage });
        }


        /// <summary>
        /// Imports Fitters from a csv file.
        /// </summary>
        /// <param name="file"></param>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanModifyCompanyLists</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanModifyCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> ImportFittersFromCSV(IFormFile file)
        {
            _logger.LogInformation("Importing Fitters from a csv file.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.ImportFittersFromCSV(file);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to import Fitter from csv file. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Importieren der Monteure. Stellen Sie sicher, dass die Datei der Beschreibung entspricht.";
            }
            return RedirectToAction("Fitters", new { msg = ViewBag.ErrorMessage });
        }
        #endregion


        #region UnknownForwardingAgencies
        /// <summary>
        /// Displays the view for unknown forwarding agencies
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanInspectUnknownCompanyLists</remarks>
        /// <param name="msg"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanInspectUnknownCompanyLists")]
        public IActionResult UnknownForwardingAgencies(string msg = "")
        {
            ViewBag.ErrorMessage = msg;
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetUnknownForwardingAgenciesViewModel();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to display the UnknownForwardingAgencies view. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                return View();
            }
        }


        /// <summary>
        /// Creates a downloadfile of the unknown forwarding agencies
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanExportUnknownCompanyLists</remarks>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanExportUnknownCompanyLists")]
        public IActionResult ExportUnknownForwardingAgenciesCSV()
        {
            var errorMessage = "";
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var stream = facade.ExportUnknownForwardingAgenciesCSVStream();
                    var response = File(stream, "configuration/ExportUnknownForwardingAgenciesCSV", "UnknownForwardingAgencies_CSV_Data.csv");
                    return response;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to export UnkownForwardingAgenciesCSV. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                errorMessage = "Fehler beim Exportieren der unbekannten Speditionen als csv-Datei.";
            }
            return RedirectToAction("UnknownForwardingAgencies", new { msg = errorMessage });
        }


        /// <summary>
        /// Deletes the entry for the unknown forwarding agency with the given id.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanInspectUnknownCompanyLists</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanInspectUnknownCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> DeleteUnknownForwardingAgency(int id)
        {
            _logger.LogInformation("Deleting the unknown forwarding agency with the id: " + id);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.DeleteUnknownForwardingAgency(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete the unknown forwarding agency with the id: " + id + " Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Löschen der unbekannten Spedition.";
            }
            return RedirectToAction("UnknownForwardingAgencies", new { msg = ViewBag.ErrorMessage });
        }
        #endregion


        #region UnknownSuppliers
        /// <summary>
        /// Displays the unknown suppliers view.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanInspectUnknownCompanyLists</remarks>
        /// <param name="msg">Optional error message</param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanInspectUnknownCompanyLists")]
        public IActionResult UnknownSuppliers(string msg = "")
        {
            ViewBag.ErrorMessage = msg;
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetUnknownSuppliers();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to display the unknown suppliers. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "\nFehler beim Laden der unbekannten Lieferanten.";
                return View();
            }
        }


        /// <summary>
        /// Deletes the unknown supplier with the given id
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanInspectUnknownCompanyLists</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanInspectUnknownCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> DeleteUnknownSupplier(int id)
        {
            _logger.LogInformation("Deleting the unknown supplier with the id: " + id);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.DeleteUnknownSupplier(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete the unknown supplier with id: " + id + " Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Löschen des unbekannten Lieferanten.";
            }
            return RedirectToAction("UnknownSuppliers", new { msg = ViewBag.ErrorMessage });
        }


        /// <summary>
        /// creates a csv file with all unknown suppliers in it.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanExportUnknownCompanyLists</remarks>
        /// <returns></returns>
        [Authorize(Roles = ("CanModifyAllSettings, CanExportUnknownCompanyLists"))]
        public IActionResult ExportUnknownSuppliersCSV()
        {
            var errorMessage = "";
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var stream = facade.ExportUnknownSuppliersCSVStream();
                    var response = File(stream, "configuration/ExportUnknownSuppliersCSV", "UnknownSuppliers_CSV_Data.csv");
                    return response;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to export UnknownSuppliersCSV. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                errorMessage = "Fehler beim Exportieren der unbekannten Lieferanten als csv-Datei.";
            }
            return RedirectToAction("UnknownSuppliers", new { msg = errorMessage });
        }
        #endregion


        #region UnknownParcelServices
        /// <summary>
        /// Displays the unknown ParcelServices view.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanInspectUnknownCompanyLists</remarks>
        /// <param name="msg">Optional error message</param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanInspectUnknownCompanyLists")]
        public IActionResult UnknownParcelServices(string msg = "")
        {
            ViewBag.ErrorMessage = msg;
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetUnknownParcelServices();
                    return View(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to display the unknown ParcelServices. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "\nFehler beim Laden der unbekannten Lieferanten.";
                return View();
            }
        }


        /// <summary>
        /// Deletes the unknown ParcelService with the given id
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanInspectUnknownCompanyLists</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanInspectUnknownCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> DeleteUnknownParcelService(int id)
        {
            _logger.LogInformation("Deleting the unknown ParcelService with the id: " + id);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.DeleteUnknownParcelService(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete the unknown ParcelService with id: " + id + " Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Löschen des unbekannten Lieferanten.";
            }
            return RedirectToAction("UnknownParcelServices", new { msg = ViewBag.ErrorMessage });
        }


        /// <summary>
        /// creates a csv file with all unknown ParcelServices in it.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanExportUnknownCompanyLists</remarks>
        /// <returns></returns>
        [Authorize(Roles = ("CanModifyAllSettings, CanExportUnknownCompanyLists"))]
        public IActionResult ExportUnknownParcelServicesCSV()
        {
            var errorMessage = "";
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var stream = facade.ExportUnknownParcelServicesCSVStream();
                    var response = File(stream, "configuration/ExportUnknownParcelServicesCSV", "UnknownParcelServices_CSV_Data.csv");
                    return response;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to export UnknownParcelServicesCSV. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                errorMessage = "Fehler beim Exportieren der unbekannten Lieferanten als csv-Datei.";
            }
            return RedirectToAction("UnknownParcelServices", new { msg = errorMessage });
        }
        #endregion


        #region UnknownFitters
        /// <summary>
        /// Displays the unknown Fitters view.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanInspectUnknownCompanyLists</remarks>
        /// <param name="msg">Optional error message</param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanInspectUnknownCompanyLists")]
        public IActionResult UnknownFitters(string msg = "")
        {
            ViewBag.ErrorMessage = msg;
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetUnknownFitters();
                    return View(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to display the unknown Fitters. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "\nFehler beim Laden der unbekannten Lieferanten.";
                return View();
            }
        }


        /// <summary>
        /// Deletes the unknown Fitter with the given id
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanInspectUnknownCompanyLists</remarks>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings, CanInspectUnknownCompanyLists")]
        [HttpPost]
        public async Task<IActionResult> DeleteUnknownFitter(int id)
        {
            _logger.LogInformation("Deleting the unknown Fitter with the id: " + id);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.DeleteUnknownFitter(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete the unknown Fitter with id: " + id + " Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Löschen des unbekannten Lieferanten.";
            }
            return RedirectToAction("UnknownFitters", new { msg = ViewBag.ErrorMessage });
        }


        /// <summary>
        /// creates a csv file with all unknown Fitters in it.
        /// </summary>
        /// <remarks>Requires one of the roles CanModifyAllSettings, CanExportUnknownCompanyLists</remarks>
        /// <returns></returns>
        [Authorize(Roles = ("CanModifyAllSettings, CanExportUnknownCompanyLists"))]
        public IActionResult ExportUnknownFittersCSV()
        {
            var errorMessage = "";
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var stream = facade.ExportUnknownFittersCSVStream();
                    var response = File(stream, "configuration/ExportUnknownFittersCSV", "UnknownFitters_CSV_Data.csv");
                    return response;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to export UnknownFittersCSV. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                errorMessage = "Fehler beim Exportieren der unbekannten Lieferanten als csv-Datei.";
            }
            return RedirectToAction("UnknownFitters", new { msg = errorMessage });
        }
        #endregion


        #region Displays
        /// <summary>
        /// Displays the displays view.
        /// </summary>
        /// <remarks>Requires the role CanModifyAllSettings</remarks>
        /// <param name="msg">Optional error message</param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult Displays(string msg = "")
        {
            ViewBag.ErrorMessage = msg;
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetDisplayIndexViewModel();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to display the displays view. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "\nFehler beim Laden der Displays.";
                return View();
            }
        }


        /// <summary>
        /// Displays the view for adding a new display
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult AddDisplay()
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            return View(new DisplayConfiguration());
        }

        /// <summary>
        /// Displays the view for editing the display with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult EditDisplay(int id)
        {
            DisplayConfiguration model = null;
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    model = facade.GetDisplayConfiguration(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not show edit display View. Error: " + e.Message);
                ViewBag.ErrorMessage = "Ansicht zur Bearbeitung einer Anzeige konnte nicht geöffnet werden. Fehler: " + e.Message;
                return RedirectToAction("Displays", new { msg = ViewBag.ErrorMessage });
            }

            if (model == null)
            {
                _logger.LogError("Could not show EditDisplay view. Display with id: " + id + " couldnt be found.");
                var error = "Anzeige konnte nicht bearbeitet werden, da kein entsprechender Eintrag gefunden wurde.";
                return RedirectToAction("Displays", new { msg = error });
            }
            return View(model);
        }


        /// <summary>
        /// Adding a new display from the passed along display configuration
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> AddDisplay(DisplayConfiguration model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation("Adding a new Display.");

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.AddDisplay(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add Display. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Hinzufügen der Anzeige.";
                return View(model);
            }

            return RedirectToAction("Displays");
        }


        /// <summary>
        /// Saves the changes made to the display configuration
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> EditDisplay(DisplayConfiguration model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation("Saving changes made to the display with the id: " + model.ID);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.EditDisplay(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not edit display with id: " + model.ID + " Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Speichern der Änderungen";
                return View(model);
            }
            return RedirectToAction("Displays");
        }


        /// <summary>
        /// Deletes the Display with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> DeleteDisplay(int id)
        {
            _logger.LogInformation("Deleting the display with the id: " + id);

            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.DeleteDisplay(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete the display with id: " + id + " Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Löschen der Anzeige.";
            }
            return RedirectToAction("Displays", new { msg = ViewBag.ErrorMessage });
        }
        #endregion


        #region User
        /// <summary>
        /// Displays the UserIndex view.
        /// </summary>
        /// <param name="msg">Optional error message</param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult UserIndex(string msg = "")
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            ViewBag.ErrorMessage = msg;
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetUserIndexViewModel();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to display the user index. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "\nFehler beim laden der Benutzer.";
                return View();
            }
        }


        /// <summary>
        /// Displays the view for adding new users
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult AddUser()
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetUserViewModel();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to display the addUser view. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                return RedirectToAction("UserIndex", new { msg = "Fehler beim Anzeigen der Eingabemaske zum Hinzufügen von Benutzern" });
            }
        }


        /// <summary>
        /// Displays the view for editing the user with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public async Task<IActionResult> EditUser(string id)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var user = await facade.GetUserViewModel(id);
                    if (user != null)
                    {
                        return View(user);
                    }
                    else
                    {
                        _logger.LogError("User with id: " + id + " could not be found. Editing will be canceled.");
                        return RedirectToAction("UserIndex", new { msg = "Benutzer wurde nicht gefunden!" });
                    }
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to display the edit view for the user with the id: " + id + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                return RedirectToAction("UserIndex", new { msg = "Fehler beim Anzeigen der Bearbeitungsmaske." });
            }
        }


        /// <summary>
        /// Adds a new user to the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> AddUser(UserViewModel model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            var valid = true;
            if (model != null && String.IsNullOrEmpty(model.Password))
            {
                ModelState.AddModelError(nameof(model.Password), "Das Passwort darf nicht leer sein.");
                valid = false;
            }
            using (var scope = _serviceProvider.CreateScope())
            {
                var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                if (ModelState.IsValid && valid)
                {
                    _logger.LogInformation("Adding new user with name: " + model.UserName);
                    try
                    {
                        IdentityResult result = await facade.AddUser(model);
                        if (result.Succeeded)
                            return RedirectToAction("UserIndex");
                        foreach (IdentityError error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("Error while trying to add User. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                        ModelState.AddModelError("", e.Message);
                    }
                }
                var grps = facade.AuthorizationGroups();
                model.Groups = grps;
                return View(model);
            }
        }


        /// <summary>
        /// Deletes the user with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            _logger.LogInformation("Deleting user with the id: " + id);
            using (var scope = _serviceProvider.CreateScope())
            {
                var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                IdentityResult result = await facade.DeleteUser(id);
                if (!result.Succeeded)
                {
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
                return RedirectToAction("UserIndex");
            }
        }


        /// <summary>
        /// Saves the changes made to the user
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            _logger.LogInformation("Saving the changes made to the user with the id: " + model.Id);
            using (var scope = _serviceProvider.CreateScope())
            {
                var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                var _userValidator = scope.ServiceProvider.GetRequiredService<IUserValidator<AppUser>>();
                var _passwordValidator = scope.ServiceProvider.GetRequiredService<IPasswordValidator<AppUser>>();
                var _passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher<AppUser>>();

                if (ModelState.IsValid)
                {
                    IdentityResult result = await facade.EditUser(model, _userValidator, _passwordHasher, _passwordValidator);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("UserIndex");
                    }
                    foreach (IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                }
                var grps = facade.AuthorizationGroups();
                model.Groups = grps;
                return View(model);
            }
        }
        #endregion


        #region AuthorizationGroups
        /// <summary>
        /// Displays the AuthorizationGroups view
        /// </summary>
        /// <param name="msg">Optional error message</param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult AuthorizationGroups(string msg = "")
        {
            ViewBag.ErrorMessage = msg;
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetAuthorizationGroupsViewModel();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to display the authorization groups. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "\nFehler beim Laden der Authorisierungsgruppen.";
                return View();
            }
        }


        /// <summary>
        /// Displays the view for adding authorization groups
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult AddAuthorizationGroup()
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            return View(new GroupViewModel { Group = new AuthorizationGroup() });
        }


        /// <summary>
        /// Displays the view for editing the authorization group with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult EditAuthorizationGroup(int id)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetGroupViewModel(id);
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to display the edit view for the authorization group with the id: " + id+ ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                return RedirectToAction("AuthorizationGroups", new { msg = "Fehler beim Laden der Authorisierungsgruppe." });
            }
        }


        /// <summary>
        /// Adds a new authorization group to the DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> AddAuthorizationGroup(GroupViewModel model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation("Adding a new Authorizationgroup with name: " + model.Group.Name);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.AddAuthorizationGroup(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while adding a new Authorizationgroup. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Hinzufügen der Authorisierungsgruppe.";
            }
            return RedirectToAction("AuthorizationGroups", new { msg = ViewBag.ErrorMessage });
        }


        /// <summary>
        /// Saves the changes made to the authorization group.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> EditAuthorizationGroup(GroupViewModel model)
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            if (!ModelState.IsValid)
                return View(model);

            _logger.LogInformation("Saving the changes made to the authorization group with the id: " + model.Group.ID);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.EditAuthorizationGroup(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to save changes made to the authorization group with the id: " + model.Group.ID + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Speichern der Änderungen.";
            }
            return RedirectToAction("AuthorizationGroups", new { msg = ViewBag.ErrorMessage });
        }


        /// <summary>
        /// Deletes the authorization group with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> DeleteAuthorizationGroup(int id)
        {
            _logger.LogInformation("Deleting the authorization group with the id: " + id);
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.DeleteAuthorizationGroup(id);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to delete the authorization group with the id: " + id + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage = "Fehler beim Löschen der Authorisierungsgruppe mit der ID: " + id;
            }

            return RedirectToAction("AuthorizationGroups", new { msg = ViewBag.ErrorMessage });
        }
        #endregion


        #region Active Directory
        /// <summary>
        /// Displays the AD settings
        /// </summary>
        /// <param name="msg">Optional error message</param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult ADSettings(string msg = "")
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            ViewBag.ErrorMessage = msg;
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetADSettings();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to display the AD settings. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "\nFehler beim Laden der Active Directory Einstellungen.";
                return View();
            }
        }


        /// <summary>
        /// Saves the AD settings.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> SetADSettings(ADSettings settings)
        {
            _logger.LogInformation("Saving the Ad settings.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.SetADSettings(settings);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while setting ad settings. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                return RedirectToAction("ADSettings", new { msg = "Fehler beim Speichern der Active Directory Einstellungen." });
            }
            return RedirectToAction("ADSettings");
        }
        #endregion

        #region BarrierControlSettings

        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult BarrierControlSettings(string msg = "")
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            ViewBag.ErrorMessage = msg;
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetBarrierControlSettings();
                    return View(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to display the Barrier control settings. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "\nFehler beim Laden der Einstellungen.";
                return View();
            }
        }


        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public IActionResult SetBarrierControlSettings(BarrierControlSettingsViewModel settings)
        {
            var errMessage = "";
            _logger.LogInformation("Saving the BarrierControl settings.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    facade.SetBarrierControlSettings(settings);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set barrierControl settings. Message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                errMessage = "Fehler beim Speichern der Einstellungen.";
            }
            return RedirectToAction("BarrierControlSettings", new { msg = errMessage });
        }
        #endregion

        #region SMSSettings
        /// <summary>
        /// Displays the SMS settings.
        /// </summary>
        /// <param name="msg">Optional error message</param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        public IActionResult SMSSettings(string msg = "")
        {
            ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            ViewBag.ErrorMessage = msg;
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    var model = facade.GetSMSSettings();
                    return View(model);
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Error while trying to display the SMS settings. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                ViewBag.ErrorMessage += "\nFehler beim Laden der SMS-Einstellungen.";
                return View();
            }
        }


        /// <summary>
        /// Saves the sms settings.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        [Authorize(Roles = "CanModifyAllSettings")]
        [HttpPost]
        public async Task<IActionResult> SetSMSSettings(SMSSettings settings)
        {
            var errMessage = "";
            _logger.LogInformation("Saving the SMS settings.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IConfigurationFacade>();
                    await facade.SetSMSSettings(settings);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set sms settings. Message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                errMessage = "Fehler beim Speichern der Einstellungen.";
            }
            return RedirectToAction("SMSSettings", new { msg = errMessage });
        }
        #endregion
    }
}