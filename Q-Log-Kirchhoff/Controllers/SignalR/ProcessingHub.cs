using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace MVC.Controllers.SignalR
{
    /// <summary>
    /// Handles SignalR actions performed in the processing view.
    /// </summary>
    /// <remarks> 
    /// The following actions are included:
    ///     UpdateSupplier
    ///     UpdateCustomer
    ///     SetGate
    ///     SetEntry
    ///     SetProcessStart
    ///     SetProcessEnd
    ///     SetExit
    /// </remarks>
    public class ProcessingHub : Hub
    {
        private readonly ILogger<ProcessingHub> _logger;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public ProcessingHub(ILogger<ProcessingHub> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Sets the companyname of the registration with the given id
        /// </summary>
        /// <remarks>Requires the role "CanModifySupplier"</remarks>
        /// <param name="ID">ID of the registration</param>
        /// <param name="CompanyName">Name of the Company</param>
        /// <returns></returns>
        //[Authorize(Roles = "CanModifyProcessingList")]
        public async Task UpdateCompanyName(int ID, string CompanyName)
        {
            _logger.LogInformation("ProcessingHub, UpdateCompanyName: Method called for the registration with id: " + ID);

            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _processingHubFacade = scope.ServiceProvider.GetRequiredService<IProcessingHubFacade>();
                    string ColorCode = await _processingHubFacade.UpdateCompanyName(ID, CompanyName);
                    await Clients.All.SendAsync("UpdateCompanyName", ID, CompanyName, ColorCode);
                }
                catch (Exception e)
                {
                    _logger.LogError("ProcessingHub, UpdateCompanyName: Error! registration id: " + ID + " error message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                    await Clients.Caller.SendAsync("Error", "Der Firmenname konnte nicht gesetzt werden. Fehler: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Sets the comment of the registration with the given id
        /// </summary>
        /// <remarks>Requires the role "CanModifySupplier"</remarks>
        /// <param name="ID">ID of the registration</param>
        /// <param name="Comment">Name of the Company</param>
        /// <returns></returns>
        //[Authorize(Roles = "CanModifyProcessingList")]
        public async Task UpdateComment(int ID, string Comment)
        {
            _logger.LogInformation("ProcessingHub, UpdateComment: Method called for the registration with id: " + ID);

            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _processingHubFacade = scope.ServiceProvider.GetRequiredService<IProcessingHubFacade>();
                    await _processingHubFacade.UpdateComment(ID, Comment);
                    await Clients.All.SendAsync("UpdateComment", ID, Comment);
                }
                catch (Exception e)
                {
                    _logger.LogError("ProcessingHub, UpdateComment: Error! registration id: " + ID + " error message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                    await Clients.Caller.SendAsync("Error", "Der Kommentar konnte nicht gesetzt werden. Fehler: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Sets the loadReference of the registration with the given id
        /// </summary>
        /// <remarks>Requires the role "CanModifySupplier"</remarks>
        /// <param name="ID">ID of the registration</param>
        /// <param name="LoadReference">Name of the Company</param>
        /// <returns></returns>
        //[Authorize(Roles = "CanModifyProcessingList")]
        public async Task UpdateLoadingReference(int ID, string LoadReference)
        {
            _logger.LogInformation("ProcessingHub, UpdateLoadReference: Method called for the registration with id: " + ID);

            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _processingHubFacade = scope.ServiceProvider.GetRequiredService<IProcessingHubFacade>();
                    await _processingHubFacade.UpdateLoadReference(ID, LoadReference);
                    await Clients.All.SendAsync("UpdateLoadingReference", ID, LoadReference);
                }
                catch (Exception e)
                {
                    _logger.LogError("ProcessingHub, UpdateLoadReference: Error! registration id: " + ID + " error message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                    await Clients.Caller.SendAsync("Error", "Die Ladereferenz konnte nicht gesetzt werden. Fehler: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Sets the gate of the registration with the given id to 
        /// the passed along name of the gate
        /// </summary>
        /// <remarks>Requires the role "CanSetGate"</remarks>
        /// <param name="id">ID of the registration</param>
        /// <param name="value">Name of the gate</param>
        /// <returns></returns>
        //[Authorize(Roles = "CanSetGate")]
        public async Task SetGate(int id, string value, string selectedLoadingstation)
        {
            _logger.LogInformation("Setting the gate of the registration with id: " + id + " to: " + value);

            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _processingHubFacade = scope.ServiceProvider.GetRequiredService<IProcessingHubFacade>();
                    await _processingHubFacade.SetGate(id, value);
                    // string gateNames = _processingHubFacade.GetGatesAsString(selectedLoadingstation);

                    await Clients.All.SendAsync("SetGate", id, value, "");
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while setting the gate of the registration with id: " + id + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                    await Clients.Caller.SendAsync("Error", "Tor konnte nicht gesetzt werden. Fehler: " + e.Message);
                }
            }
        }

        public async Task SetGateWithNewLoadingstation(int id, string selectedGate, string selectedLoadingstation)
        {
            _logger.LogInformation("Setting the gate of the registration with id: " + id + " to: " + selectedGate + " after Loadingstation was set to " + selectedLoadingstation);

            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _processingHubFacade = scope.ServiceProvider.GetRequiredService<IProcessingHubFacade>();
                    await _processingHubFacade.SetGate(id, selectedGate);
                    string gateNames = _processingHubFacade.GetGatesAsString(selectedLoadingstation);

                    await Clients.All.SendAsync("SetGateWithNewLoadingstation", id, selectedGate, gateNames);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while setting the gate of the registration with id: " + id + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message + " after Loadingstation was set to " + selectedLoadingstation);
                    await Clients.Caller.SendAsync("Error", "Tor konnte nicht gesetzt werden. Fehler: " + e.Message);
                }
            }
        }

        //[Authorize(Roles = "CanSetLoadingStation")]
        public async Task SetLoadingStation(int id, string selectedLoadingstation, string selectedGate)
        {
            _logger.LogInformation("Setting the Loadingstation of the registration with id: " + id + " to: " + selectedLoadingstation);

            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _processingHubFacade = scope.ServiceProvider.GetRequiredService<IProcessingHubFacade>();
                    await _processingHubFacade.SetLoadingStation(id, selectedLoadingstation);
                    string loadingstations = _processingHubFacade.GetLoadingstationsAsString();

                    await Clients.All.SendAsync("SetLoadingStation", id, selectedLoadingstation, selectedGate, loadingstations);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while setting the Loadingstation of the registration with id: " + id + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                    await Clients.Caller.SendAsync("Error", "Ladestation konnte nicht gesetzt werden. Fehler: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Sets the time of release of the registration with the given id 
        /// to the current server time.
        /// </summary>
        /// <remarks>Requires the role "CanSetRelease"</remarks>
        /// <param name="id">ID of the registration</param>
        /// <returns></returns>
        //[Authorize(Roles = "CanSetRelease")]
        public async Task SetRelease(int id)
        {
            _logger.LogInformation("Setting time of release for registration with id: " + id);

            var curTime = DateTime.Now;
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _processingHubFacade = scope.ServiceProvider.GetRequiredService<IProcessingHubFacade>();
                    var barrierSettingsRepo = scope.ServiceProvider.GetRequiredService<IBarrierControlSettingsRepository>();

                    await _processingHubFacade.SetRelease(id, curTime);
                    await Clients.All.SendAsync("SetRelease", id, curTime);

                    await _processingHubFacade.SendOpenRegistrationToERP(id);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while setting the time of release for the registration with id: " + id + ". Message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                    await Clients.Caller.SendAsync("Error", "Einfahrt konnte nicht gesetzt werden. Fehler: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Sets the time of call of the registration with the given id 
        /// to the current server time.
        /// </summary>
        /// <remarks>Requires the role "CanSetCall"</remarks>
        /// <param name="id">ID of the registration</param>
        /// <returns></returns>
        //[Authorize(Roles = "CanSetCall")]
        public async Task SetCall(int id, string gate, string comment)
        {
            _logger.LogInformation("Setting time of call for registration with id: " + id);

            var curTime = DateTime.Now;
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _processingHubFacade = scope.ServiceProvider.GetRequiredService<IProcessingHubFacade>();
                    var barrierSettingsRepo = scope.ServiceProvider.GetRequiredService<IBarrierControlSettingsRepository>();
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();

                    await _processingHubFacade.UpdateComment(id, comment);
                    await _processingHubFacade.SetCall(id, gate, curTime);
                    var model = _openRegistrationsRepository.Get(id);
                    await Clients.All.SendAsync("SetCallWithModel", id, curTime, model);
                    //   await Clients.All.SendAsync("SetCall", id, curTime);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while setting the time of call for the registration with id: " + id + ". Message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                    await Clients.Caller.SendAsync("Error", "Der Aufruf konnte nicht gesetzt werden. Fehler: " + e.Message);
                }
            }
        }


        //[Authorize(Roles = "CanSetCall")]
        public async Task SetCloseForEmptyGoods(int id, string comment)
        {
            _logger.LogInformation("Setting time of call for registration with id: " + id);

            var curTime = DateTime.Now;
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _processingHubFacade = scope.ServiceProvider.GetRequiredService<IProcessingHubFacade>();
                    var barrierSettingsRepo = scope.ServiceProvider.GetRequiredService<IBarrierControlSettingsRepository>();
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();

                    await _processingHubFacade.UpdateComment(id, comment);
                    await _processingHubFacade.SetUpdateCallStatus(id, 3, curTime);
                    var model = _openRegistrationsRepository.Get(id);
                    await Clients.All.SendAsync("SetCallWithModel", id, curTime, model);
                    //   await Clients.All.SendAsync("SetCall", id, curTime);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while setting the time of call for the registration with id: " + id + ". Message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                    await Clients.Caller.SendAsync("Error", "Der Aufruf konnte nicht gesetzt werden. Fehler: " + e.Message);
                }
            }
        }





        //[Authorize(Roles = "CanSetCall")]
        public async Task SetConfirm(int id, string comment)
        {
            _logger.LogInformation("Setting time of call for registration with id: " + id);

            var curTime = DateTime.Now;
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _processingHubFacade = scope.ServiceProvider.GetRequiredService<IProcessingHubFacade>();
                    var barrierSettingsRepo = scope.ServiceProvider.GetRequiredService<IBarrierControlSettingsRepository>();
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();

                    await _processingHubFacade.UpdateComment(id, comment);
                    await _processingHubFacade.SetConfirm(id, curTime);
                    var model = _openRegistrationsRepository.Get(id);

                    await Clients.All.SendAsync("SetCallWithModel", id, curTime, model);

                    //    await Clients.All.SendAsync("SetCall", id, curTime);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while setting the time of call for the registration with id: " + id + ". Message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                    await Clients.Caller.SendAsync("Error", "Der Aufruf konnte nicht gesetzt werden. Fehler: " + e.Message);
                }
            }
        }

        /// <summary>
        /// Sets the time of entry of the registration with the given id 
        /// to the current server time.
        /// </summary>
        /// <remarks>Requires the role "CanSetEntrance"</remarks>
        /// <param name="id">ID of the registration</param>
        /// <returns></returns>
        //[Authorize(Roles = "CanSetEntrance")]
        public async Task SetEntry(int id)
        {
            _logger.LogInformation("Setting time of entry for registration with id: " + id);

            var curTime = DateTime.Now;
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _processingHubFacade = scope.ServiceProvider.GetRequiredService<IProcessingHubFacade>();
                    var barrierSettingsRepo = scope.ServiceProvider.GetRequiredService<IBarrierControlSettingsRepository>();

                    await _processingHubFacade.SetEntry(id, curTime);
                    await Clients.All.SendAsync("SetEntry", id, curTime);

                    var barrierSettings = barrierSettingsRepo.Get();

                    //if (barrierSettings.UseBarrierControl)
                    //{
                    //    if (!String.IsNullOrEmpty(barrierSettings.EntryBarrierAPIUrl))
                    //    {
                    //        try
                    //        {
                    //            var httpClient = scope.ServiceProvider.GetRequiredService<HttpClientWrapper>();
                    //            var result = httpClient.PostAsync(barrierSettings.EntryBarrierAPIUrl, null).Result;
                    //        }
                    //        catch (Exception e)
                    //        {
                    //            _logger.LogWarning("Could not open entry barrier. Message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                    //        }
                    //    }
                    //}
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while setting the time of entry for the registration with id: " + id + ". Message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                    await Clients.Caller.SendAsync("Error", "Einfahrt konnte nicht gesetzt werden. Fehler: " + e.Message);
                }
            }
        }


        /// <summary>
        /// Sets the time of the process start for the registration with the given id
        /// to the current server time.
        /// </summary>
        /// <remarks>Requires the role "CanSetProcessStart"</remarks>
        /// <param name="id">ID of the registration</param>
        /// <returns></returns>
        //[Authorize(Roles = "CanSetProcessStart")]
        public async Task SetProcessStart(int id)
        {
            _logger.LogInformation("Setting the process start time for the registration with id:" + id + " ot the current server time.");
            var curTime = DateTime.Now;

            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _processingHubFacade = scope.ServiceProvider.GetRequiredService<IProcessingHubFacade>();
                    await _processingHubFacade.SetProcessStart(id, curTime);
                    await Clients.All.SendAsync("SetProcessStart", id, curTime);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while setting process start for the registration with id: " + id + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                    await Clients.Caller.SendAsync("Error", "Der Startzeitpunkt konnte nicht gesetzt werden. Feler: " + e.Message);
                }
            }
        }


        /// <summary>
        /// Sets the process end time for the registration with the given
        /// id to the current server time.
        /// </summary>
        /// <remarks>Requires role "CanSetProcessEnd"</remarks>
        /// <param name="id">ID of the registration</param>
        /// <returns></returns>
        //[Authorize(Roles = "CanSetProcessEnd")]
        public async Task SetProcessEnd(int id)
        {
            _logger.LogInformation("Setting the process end time for the registration with id:" + id + " to the current server time.");
            var curTime = DateTime.Now;

            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _processingHubFacade = scope.ServiceProvider.GetRequiredService<IProcessingHubFacade>();
                    await _processingHubFacade.SetProcessEnd(id, curTime);
                    await Clients.All.SendAsync("SetProcessEnd", id, curTime);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while setting process end time for registration with id: " + id + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                    await Clients.Caller.SendAsync("Error", "Der Endzeitpunkt konnte nicht gesetzt werden. Feler: " + e.Message);
                }
            }
        }


        /// <summary>
        /// Sets the exit time of the registration with the given id
        /// to the current server time.
        /// </summary>
        /// <remarks>Required role "CanSetExit"</remarks>
        /// <param name="id">ID of the registration</param>
        /// <returns></returns>
        //[Authorize(Roles = "CanSetExit")]
        public async Task SetExit(int id, string comment)
        {
            _logger.LogInformation("Setting the exit time of the registration with id: " + id + " to the current server time.");

            var curTime = DateTime.Now;
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    bool canExitProcess = await CanExitTheProcess(id);
                    if (canExitProcess)
                    {
                        var _processingHubFacade = scope.ServiceProvider.GetRequiredService<IProcessingHubFacade>();
                        await _processingHubFacade.UpdateComment(id, comment);
                        await _processingHubFacade.SetExit(id, curTime);
                    }
                    await Clients.All.SendAsync("SetExit", id, curTime);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while setting exit time of the registration with id: " + id + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                    await Clients.Caller.SendAsync("Error", "Ausfahrt konnte nicht gesetzt werden. Eventuell werden auf der Anzeige noch Fahrzeuge angezeigt, welche sich nicht mehr auf dem Gelände befinden. Fehler: " + e.Message);
                }
            }
        }


        public async Task<bool> CanExitTheProcess(int id)
        {
            _logger.LogInformation("Setting the exit time of the registration with id: " + id + " to the current server time.");


            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    int optionCheckedCount = 0;
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                    var model = _openRegistrationsRepository.Get(id);

                    if (model.LoadCustomerPickup) { optionCheckedCount++; }
                    if (model.GoodsReceiptCustomerEmpties) { optionCheckedCount++; }
                    if (model.GoodsReceiptdelivery) { optionCheckedCount++; }

                    if (optionCheckedCount == 1) return true;
                    if (optionCheckedCount == 2 && model.StatusCall == 4) return true;
                    if (optionCheckedCount == 3 && model.StatusCall == 6) return true;
                    await _openRegistrationsRepository.IncCallStatus(id);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while setting exit time of the registration with id: " + id + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                }
            }

            return false;
        }
    }
}
