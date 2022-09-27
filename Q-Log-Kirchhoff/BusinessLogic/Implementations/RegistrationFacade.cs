using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Controllers.SignalR;
using MVC.Data.Entities;
using MVC.Data.Enums;
using MVC.Models;
using MVC.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Implementations
{
    public class RegistrationFacade : IRegistrationFacade
    {
        private readonly ILogger _logger;


        private readonly IServiceProvider _serviceProvider;

        public RegistrationFacade(ILogger<RegistrationFacade> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public RegistrationViewModel GetRegistrationViewModel()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _terminalSettingsRepository = scope.ServiceProvider.GetRequiredService<ITerminalSettingsRepository>();
                var _loadingStationsRepository = scope.ServiceProvider.GetRequiredService<ILoadingStationsRepository>();
                //var _smsSettingsRepository = scope.ServiceProvider.GetRequiredService<ISMSSettingsRepository>();

                var model = new RegistrationViewModel()
                {
                    LoadingStations = _loadingStationsRepository.GetAll()
                };

                //model.UseSMSService = _smsSettingsRepository.Get().UseSMSService;
                var terminalSettings = _terminalSettingsRepository.Get();
                model.TimePerLanguage = terminalSettings.TimePerLanguage;
                model.TimeTillReset = terminalSettings.TimeTillReset;

                return model;
            }
        }

        /// <summary>
        /// Creates an OpenRegistration from the Data provided by the viewmodel. 
        /// After the creation is completed, a possible connection to a special trip is checked.
        /// Afterwards the UnknownLists are Updated and the clients on the processinglist are updated.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task AddRegistrationFromViewModel(RegistrationViewModel model)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                _logger.LogInformation("Adding a new registration.");
                if (model == null)
                {
                    _logger.LogError("Could not add a new registration: the ViewModel is null");
                    throw new ArgumentNullException("Model is null");
                }
                try
                {
                    var registration = CreateOpenRegistrationFromViewModel(model);
                //   await UpdateUnknownLists(registration);
                    int id = await _openRegistrationsRepository.Add(registration);

                    registration.ID = id;

                    //await UpdateUnknownLists(registration);

                    // The order of these 2 Operations is important! 
                    await UpdateProcessingClients(registration);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while trying to add a new registration. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                    throw;
                }
            }
        }


        private OpenRegistration CreateOpenRegistrationFromViewModel(RegistrationViewModel model)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _protectionProvider = scope.ServiceProvider.GetRequiredService<IDataProtectionProvider>();

                var registration = new OpenRegistration
                {
                    LicensePlate = model.LicensePlate,
                    CompressedLicensePlate = Utility.CompressLicensePlate(model.LicensePlate),
                    Language = Thread.CurrentThread.CurrentCulture.TwoLetterISOLanguageName, // model.SelectedLanguage;
                    FirstName = model.Firstname,
                    Lastname = model.Lastname,
                    Forwarder = model.Forwarder,
                    Customer = model.Customer,
                    Phonenumber = model.Phonenumber,
                    LoadCustomerPickup = model.LoadCustomerPickup,
                    LoadEmptiesCollection = model.LoadEmptiesCollection,
                    GoodsReceiptCustomerEmpties = model.GoodsReceiptCustomerEmpties,
                    GoodsReceiptdelivery = model.GoodsReceiptdelivery,
                    WasSendingSuccessful = false,
                     
                    TimeOfRegistration = DateTime.Now
                };

                return registration;
            }
        }

        /// <summary>
        /// Checks if the forwarding agency, supplier and customer are known.
        /// If not, they will be added to the respective list of unknown agencies/suppliers/customers.
        /// If the forwarding agency is known, the colorcode of the registration will be set to the colorcode of the agency.
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        private async Task UpdateUnknownLists(OpenRegistration registration)
        {
            switch (registration.ApproachTyp)
            {
                case EApproachTyp.ForwardingAgency:
                    await UpdateUnknownForwardingAgencies(registration);
                    break;
                case EApproachTyp.Supplier:
                    await UpdateUnknownSuppliers(registration);
                    break;
                case EApproachTyp.ParcelService:
                    await UpdateUnknownParcelServices(registration);
                    break;
                case EApproachTyp.Fitter:
                    await UpdateUnknownFitters(registration);
                    break;
            }
        }

        private async Task UpdateUnknownForwardingAgencies(OpenRegistration registration)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _forwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IForwardingAgenciesRepository>();
                var _unknownForwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IUnknownForwardingAgenciesRepository>();


                if (!String.IsNullOrEmpty(registration.CompanyName))
                {
                    var forwardingAgency = _forwardingAgenciesRepository.Get(registration.CompanyName);

                    if (forwardingAgency == null)
                    {
                        await _unknownForwardingAgenciesRepository.Add(registration.CompanyName);
                    }
                    else
                    {
                        registration.ColorCode = forwardingAgency.ColorCode;
                    }
                }
            }
        }

        private async Task UpdateUnknownSuppliers(OpenRegistration registration)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _supplierRepository = scope.ServiceProvider.GetRequiredService<ISupplierRepository>();
                var _unknownSupplierRepository = scope.ServiceProvider.GetRequiredService<IUnknownSupplierRepository>();

                if (!String.IsNullOrEmpty(registration.CompanyName))
                {
                    var supplier = _supplierRepository.Get(registration.CompanyName);

                    if (supplier == null)
                    {
                        await _unknownSupplierRepository.Add(registration.CompanyName);
                    }
                    else
                    {
                        registration.ColorCode = supplier.ColorCode;
                    }
                }
            }
        }

        private async Task UpdateUnknownParcelServices(OpenRegistration registration)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _parcelServiceRepository = scope.ServiceProvider.GetRequiredService<IParcelServicesRepository>();
                var _unknownParcelServiceRepository = scope.ServiceProvider.GetRequiredService<IUnknownParcelServiceRepository>();

                if (!String.IsNullOrEmpty(registration.CompanyName))
                {
                    var parcelService = _parcelServiceRepository.Get(registration.CompanyName);

                    if (parcelService == null)
                    {
                        await _unknownParcelServiceRepository.Add(registration.CompanyName);
                    }
                    else
                    {
                        registration.ColorCode = parcelService.ColorCode;
                    }
                }
            }
        }

        private async Task UpdateUnknownFitters(OpenRegistration registration)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _fittersRepository = scope.ServiceProvider.GetRequiredService<IFittersRepository>();
                var _unknownFitters = scope.ServiceProvider.GetRequiredService<IUnknownFitterRepository>();

                if (!String.IsNullOrEmpty(registration.CompanyName))
                {
                    var fitter = _fittersRepository.Get(registration.CompanyName);

                    if (fitter == null)
                    {
                        await _unknownFitters.Add(registration.CompanyName);
                    }
                    else
                    {
                        registration.ColorCode = fitter.ColorCode;
                    }
                }
            }
        }

        private async Task UpdateProcessingClients(OpenRegistration registration)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _gatesRepository = scope.ServiceProvider.GetRequiredService<IGatesRepository>();
                var _loadingStationsRepository = scope.ServiceProvider.GetRequiredService<ILoadingStationsRepository>();
                var _hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<ProcessingHub>>();

                var processingRegistrationModel = new ProcessingRegistrationViewModel
                {
                    Registration = registration,
                    LoadingStations = _loadingStationsRepository.GetAll(),
                    Gates = _gatesRepository.GetAll() 
                };


                await _hubContext.Clients.All.SendAsync("AddRegistration", processingRegistrationModel);
            }
        }


        public void CheckForUnknownInput(RegistrationViewModel model)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                if (model.ApproachTyp == EApproachTyp.ForwardingAgency)
                {
                    var _forwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IForwardingAgenciesRepository>();
                    var forwardingAgencies = _forwardingAgenciesRepository.GetAll();
                    if (!forwardingAgencies.Any(x => x.Name == model.CompanyName))
                        model.CompanyUnknown = true;
                }
                else if (model.ApproachTyp == EApproachTyp.Supplier)
                {
                    var _supplierRepository = scope.ServiceProvider.GetRequiredService<ISupplierRepository>();
                    var suppliers = _supplierRepository.GetAll();
                    if (!suppliers.Any(x => x.Name == model.CompanyName))
                        model.CompanyUnknown = true;
                }
                else if (model.ApproachTyp == EApproachTyp.ParcelService)
                {
                    var _parcelServicesRepository = scope.ServiceProvider.GetRequiredService<IParcelServicesRepository>();
                    var parcelServices = _parcelServicesRepository.GetAll();
                    if (!parcelServices.Any(x => x.Name == model.CompanyName))
                        model.CompanyUnknown = true;
                }
                else if (model.ApproachTyp == EApproachTyp.Fitter)
                {
                    var _fittersRepository = scope.ServiceProvider.GetRequiredService<IFittersRepository>();
                    var fitters = _fittersRepository.GetAll();
                    if (!fitters.Any(x => x.Name == model.CompanyName))
                        model.CompanyUnknown = true;
                }
            }
        }
    }
}
