using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Data.Entities;
using MVC.Models.ConfigurationViewModels;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Implementations
{

    public class ConfigurationFacade : IConfigurationFacade
    {
        private readonly ILogger<ConfigurationFacade> _logger;
        private readonly IServiceProvider _serviceProvider;

        private readonly IAuthorizationGroupsRepository _groupsRepository;
        private readonly UserManager<AppUser> _userManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public ConfigurationFacade(ILogger<ConfigurationFacade> logger, IServiceProvider serviceProvider, IAuthorizationGroupsRepository authorizationGroupsRepository, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _groupsRepository = authorizationGroupsRepository;
            _userManager = userManager;
        }


        #region GeneralSettings

        /// <summary>
        /// Gets the general settings object from the repository and returns it.
        /// </summary>
        /// <returns>GeneralSettings</returns>
        public GeneralSettings GetGeneralSettings()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _generalSettingsRepository = scope.ServiceProvider.GetRequiredService<IGeneralSettingsRepository>();
                return _generalSettingsRepository.GetGeneralSettings();
            }
        }

        /// <summary>
        /// Creates the ViewModel with the data from the generalsettings.
        /// </summary>
        /// <returns>GeneralSettingsViewModel</returns>
        public GeneralSettingsViewModel GetGeneralSettingsViewModel()
        {
            var model = new GeneralSettingsViewModel();
            var generalSettings = GetGeneralSettings();
            model.RegistrationTimeThreshold = generalSettings.RegistrationTimeThreshold;
            model.DefaultHistoryTimespan = generalSettings.DefaultHistoryTimespan;
            model.UpdateDisplayInterval = generalSettings.DisplayUpdateInterval;
            model.ExceededWaitTimeColorCode = generalSettings.ExceededWaitTimeColorCode;
            model.ExitColorCode = generalSettings.ExitColorCode;
            model.HoverColorCode = generalSettings.HoverColorCode;
            model.NewEntryColorCode = generalSettings.NewEntryColorCode;
            model.RecentChangeColorCode = generalSettings.RecentChangeColorCode;
            return model;
        }

        /// <summary>
        /// Tells the repository to save the general settings passed along in the param.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task SetGeneralSettings(GeneralSettingsViewModel model)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _generalSettingsRepository = scope.ServiceProvider.GetRequiredService<IGeneralSettingsRepository>();
                await _generalSettingsRepository.SetGeneralSettings(model);
            }
        }
        #endregion


        #region TerminalSettings

        /// <summary>
        /// Gets the TerminalSettings from the repository and returns the object.
        /// </summary>
        /// <returns>TerminalSettings</returns>
        public TerminalSettings GetTerminalSettingsViewModel()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _terminalSettingsRepository = scope.ServiceProvider.GetRequiredService<ITerminalSettingsRepository>();
                return _terminalSettingsRepository.Get();
            }
        }

        /// <summary>
        /// Tells the repository to save the passed along settings for the terminal.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public async Task SetTerminalSettings(TerminalSettings settings)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _terminalSettingsRepository = scope.ServiceProvider.GetRequiredService<ITerminalSettingsRepository>();
                await _terminalSettingsRepository.Set(settings);
            }
        }
        #endregion


        #region LoadingStations

        /// <summary>
        /// Tells the repository to add the new LoadingStation.
        /// </summary>
        /// <param name="LoadingStation"></param>
        /// <returns></returns>
        public async Task AddLoadingStation(LoadingStation LoadingStation)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _LoadingStationsRepository = scope.ServiceProvider.GetRequiredService<ILoadingStationsRepository>();
                await _LoadingStationsRepository.Add(LoadingStation);
            }
        }

        /// <summary>
        /// Tells the repository to delete the LoadingStation with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteLoadingStation(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _LoadingStationsRepository = scope.ServiceProvider.GetRequiredService<ILoadingStationsRepository>();
                await _LoadingStationsRepository.Delete(id);
            }
        }

        public LoadingStation GetLoadingStation(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _LoadingStationsRepository = scope.ServiceProvider.GetRequiredService<ILoadingStationsRepository>();
                return _LoadingStationsRepository.Get(id);
            }
        }

        public List<LoadingStation> GetAllLoadingStations()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _LoadingStationsRepository = scope.ServiceProvider.GetRequiredService<ILoadingStationsRepository>();
                return _LoadingStationsRepository.GetAll();
            }
        }

        public LoadingStationIndexViewModel GetLoadingStationsViewModel()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _LoadingStationsRepository = scope.ServiceProvider.GetRequiredService<ILoadingStationsRepository>();
                var model = new LoadingStationIndexViewModel();
                model.LoadingStations = _LoadingStationsRepository.GetAll();
                return model;
            }
        }


        public async Task EditLoadingStation(LoadingStation LoadingStation)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _LoadingStationsRepository = scope.ServiceProvider.GetRequiredService<ILoadingStationsRepository>();
                await _LoadingStationsRepository.Set(LoadingStation);
            }
        }

        public async Task ImportLoadingStationsFromCSV(IFormFile file)
        {
            if (file == null)
                throw new Exception("Es wurde keine Datei ausgewählt, oder die Datei ist leer.");
            using (var scope = _serviceProvider.CreateScope())
            {
                var _csvReader = scope.ServiceProvider.GetRequiredService<ICsvReader>();
                var _LoadingStationsRepository = scope.ServiceProvider.GetRequiredService<ILoadingStationsRepository>();

                _csvReader.Read(file);
                string[] requiredKeys = { "Name" };
                if (_csvReader.IsValid && _csvReader.ContainsKeys(requiredKeys))
                {
                    var keys = _csvReader.Keys;
                    var values = _csvReader.Values;

                    List<LoadingStation> LoadingStations = new List<LoadingStation>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        LoadingStation LoadingStation = new LoadingStation();
                        for (int j = 0; j < keys.Length; j++)
                        {
                            switch (keys[j].ToUpper())
                            {
                                case "NAME":
                                    LoadingStation.Name = values[i][j];
                                    break;
                                case "DESCRIPTION":
                                    LoadingStation.Description = values[i][j];
                                    break;
                            }
                        }
                        LoadingStations.Add(LoadingStation);
                    }

                    try
                    {
                        await _LoadingStationsRepository.Import(LoadingStations);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                else
                {
                    _logger.LogError("Could not import LoadingStations from CSV. File was invalid or did not contain the required keys.");
                    throw new Exception("Datei konnte nicht importiert werden. Stellen Sie sicher, dass sie der Beschreibung entspricht.");
                }
            }
        }
        #endregion

        #region Gates

        /// <summary>
        /// Tells the repository to add the new gate.
        /// </summary>
        /// <param name="gate"></param>
        /// <returns></returns>
        public async Task AddGate(Gate gate)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _gatesRepository = scope.ServiceProvider.GetRequiredService<IGatesRepository>();
                await _gatesRepository.Add(gate);
            }
        }

        /// <summary>
        /// Tells the repository to delete the gate with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteGate(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _gatesRepository = scope.ServiceProvider.GetRequiredService<IGatesRepository>();
                await _gatesRepository.Delete(id);
            }
        }

        public Gate GetGate(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _gatesRepository = scope.ServiceProvider.GetRequiredService<IGatesRepository>();
                return _gatesRepository.Get(id);
            }
        }

        public List<Gate> GetAllGates()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _gatesRepository = scope.ServiceProvider.GetRequiredService<IGatesRepository>();
                return _gatesRepository.GetAll();
            }
        }

        public ConfigurationGatesViewModel GetGatesViewModel()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _gatesRepository = scope.ServiceProvider.GetRequiredService<IGatesRepository>();
                var model = new ConfigurationGatesViewModel();
                model.Gates = _gatesRepository.GetAll();
                return model;
            }
        }

        public async Task EditGate(Gate gate)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _gatesRepository = scope.ServiceProvider.GetRequiredService<IGatesRepository>();
                await _gatesRepository.Set(gate);
            }
        }

        public async Task ImportGatesFromCSV(IFormFile file)
        {
            if (file == null)
                throw new Exception("Es wurde keine Datei ausgewählt, oder die Datei ist leer.");
            using (var scope = _serviceProvider.CreateScope())
            {
                var _csvReader = scope.ServiceProvider.GetRequiredService<ICsvReader>();
                var _gatesRepository = scope.ServiceProvider.GetRequiredService<IGatesRepository>();
                var _loadingStationsRepository = scope.ServiceProvider.GetRequiredService<ILoadingStationsRepository>();

                var loadingstations = _loadingStationsRepository.GetAll();
                _csvReader.Read(file);
                string[] requiredKeys = { "Name" };
                if (_csvReader.IsValid && _csvReader.ContainsKeys(requiredKeys))
                {
                    var keys = _csvReader.Keys;
                    var values = _csvReader.Values;

                    List<Gate> gates = new List<Gate>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        Gate gate = new Gate();
                        for (int j = 0; j < keys.Length; j++)
                        {
                            switch (keys[j].ToUpper())
                            {
                                case "NAME":
                                    gate.Name = values[i][j];
                                    break;
                                case "DESCRIPTION":
                                    gate.Description = values[i][j];
                                    break;
                                case "LOADINGSTATION":
                                    if (loadingstations.Contains(loadingstations.Find(x => x.Name == values[i][j])))
                                        gate.LoadingStation = values[i][j];
                                    else
                                        gate.LoadingStation = string.Empty;
                                    break;
                            }
                        }
                        gates.Add(gate);
                    }

                    try
                    {
                        await _gatesRepository.Import(gates);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                else
                {
                    _logger.LogError("Could not import Gates from CSV. File was invalid or did not contain the required keys.");
                    throw new Exception("Datei konnte nicht importiert werden. Stellen Sie sicher, dass sie der Beschreibung entspricht.");
                }
            }
        }
        #endregion

        #region ForwardingAgencies
        public ForwardingAgenciesIndexViewModel GetForwardingAgenciesIndexViewModel()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _forwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IForwardingAgenciesRepository>();
                var model = new ForwardingAgenciesIndexViewModel();
                model.ForwardingAgencies = _forwardingAgenciesRepository.GetAll();
                return model;
            }
        }

        public ForwardingAgencyViewModel GetForwardingAgencyViewModel()
        {
            var model = new ForwardingAgencyViewModel();
            model.ForwardingAgency = new ForwardingAgency();
            return model;
        }

        public ForwardingAgencyViewModel GetForwardingAgencyViewModel(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _forwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IForwardingAgenciesRepository>();
                var model = new ForwardingAgencyViewModel();
                model.ForwardingAgency = _forwardingAgenciesRepository.Get(id);
                return model;
            }
        }

        public async Task AddForwardingAgency(ForwardingAgencyViewModel model)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _forwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IForwardingAgenciesRepository>();
                if (model == null)
                {
                    throw new ArgumentNullException("ForwardingAgencyViewModel == null");
                }
                if (model.ForwardingAgency == null)
                {
                    throw new ArgumentNullException("ForwardingAgencyViewModel.ForwardingAgency == null");
                }
                await _forwardingAgenciesRepository.Add(model.ForwardingAgency);
            }
        }

        public async Task EditForwardingAgency(ForwardingAgencyViewModel model)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _forwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IForwardingAgenciesRepository>();
                if (model == null)
                {
                    throw new ArgumentNullException("ForwardingAgencyViewModel == null");
                }
                if (model.ForwardingAgency == null)
                {
                    throw new ArgumentNullException("ForwardingAgencyViewModel.ForwardingAgency == null");
                }
                await _forwardingAgenciesRepository.Edit(model.ForwardingAgency);
            }
        }

        public async Task DeleteForwardingAgency(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _forwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IForwardingAgenciesRepository>();
                await _forwardingAgenciesRepository.Delete(id);
            }
        }


        /// <summary>
        /// Imports forwarding agencies from a csv file. All forwarding agencies that
        /// are not in the file will be removed from the DB.
        /// The following Keys are required:
        /// - Name
        /// - PostalCode
        /// 
        /// The Name is a string. The PostalCode is a string as well.
        /// </summary>
        /// <param name="file">The CSV file containing the forwariding agencies</param>
        /// <returns></returns>
        public async Task ImportForwardingAgenciesFromCSV(IFormFile file)
        {
            if (file == null)
                throw new Exception("Es wurde keine Datei ausgewählt, oder die Datei ist leer.");

            _logger.LogInformation("Importing forwarding agencies from a csv file.");
            using (var scope = _serviceProvider.CreateScope())
            {
                var _csvReader = scope.ServiceProvider.GetRequiredService<ICsvReader>();
                var _forwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IForwardingAgenciesRepository>();

                _csvReader.Read(file);
                string[] requiredKeys = { "Name" };
                if (_csvReader.IsValid && _csvReader.ContainsKeys(requiredKeys))
                {
                    var keys = _csvReader.Keys;
                    var values = _csvReader.Values;

                    List<ForwardingAgency> agencies = new List<ForwardingAgency>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        ForwardingAgency agency = new ForwardingAgency();
                        try
                        {
                            for (int j = 0; j < keys.Length; j++)
                            {
                                switch (keys[j].ToUpper())
                                {
                                    case "NAME":
                                        agency.Name = values[i][j];
                                        break;
                                    case "COLORCODE":
                                        agency.ColorCode = values[i][j];
                                        break;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError("Could not parse csv values into forwarding agency values. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                            throw new Exception("Daten konnten nicht importiert werden. Die Datentypen stimmen nicht.");
                        }
                        if (string.IsNullOrWhiteSpace(agency.ColorCode))
                            agency.ColorCode = "#ffffff";
                        agencies.Add(agency);
                    }

                    try
                    {
                        await _forwardingAgenciesRepository.Import(agencies);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                else
                {
                    _logger.LogError("Could not import forwarding agencies from CSV. File was invalid or did not contain the required keys.");
                    throw new Exception("Datei konnte nicht importiert werden. Stellen Sie sicher, dass sie der Beschreibung entspricht.");
                }
            }
        }
        #endregion

        #region Suppliers



        public async Task<bool> getSupplierFromERP()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _erpSender = scope.ServiceProvider.GetRequiredService<IERPSender>();

                    bool wasSendingSuccessful = await _erpSender.GetSuppliersFromERP();
                    return wasSendingSuccessful;
                }
            }
            catch (Exception)
            {
                // throw the exception further, so that the user will see the message
                throw;
            }
        }

        public SuppliersIndexViewModel GetSuppliersIndexViewModel()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _supplierRepository = scope.ServiceProvider.GetRequiredService<ISupplierRepository>();
                var model = new SuppliersIndexViewModel();
                model.Suppliers = _supplierRepository.GetAll();
                model.SupplierNumbers = _supplierRepository.GetAllSupplierNumber();
                return model;
            }
        }

        public SupplierViewModel GetSupplierViewModel()
        {
            var model = new SupplierViewModel
            {
                Supplier = new Supplier(),
                SupplierNumbers = new List<SupplierNumber>(),
                Numbers = string.Empty
            };

            return model;
        }

        public SupplierViewModel GetSupplier(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _supplierRepository = scope.ServiceProvider.GetRequiredService<ISupplierRepository>();
                var model = new SupplierViewModel();
                model.Supplier = _supplierRepository.Get(id);
                model.SupplierNumbers = _supplierRepository.GetAllSupplierNumbersFromSupplier(model.Supplier.Name);
                model.OldName = model.Supplier.Name;

                if (model.SupplierNumbers == null)
                    model.SupplierNumbers = new List<SupplierNumber>();

                foreach (SupplierNumber number in model.SupplierNumbers)
                {
                    model.Numbers += number.Number.ToString() + ",";
                }

                return model;
            }
        }


        public async Task AddSupplier(SupplierViewModel supplierViewModel)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _supplierRepository = scope.ServiceProvider.GetRequiredService<ISupplierRepository>();

                if (supplierViewModel == null)
                {
                    throw new ArgumentNullException("Supplier == null");
                }

                await _supplierRepository.Add(supplierViewModel);
            }
        }

        public async Task EditSupplier(SupplierViewModel supplierViewModel)
        {
            if (supplierViewModel == null)
            {
                throw new ArgumentNullException("SupplierViewModel == null");
            }
            using (var scope = _serviceProvider.CreateScope())
            {
                var _supplierRepository = scope.ServiceProvider.GetRequiredService<ISupplierRepository>();
                await _supplierRepository.EditSupplier(supplierViewModel);
            }
        }

        public async Task DeleteSupplier(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _supplierRepository = scope.ServiceProvider.GetRequiredService<ISupplierRepository>();

                await _supplierRepository.Delete(id);
            }
        }

        public async Task ImportSuppliersFromCSV(IFormFile file)
        {
            if (file == null)
                throw new Exception("Es wurde keine Datei ausgewählt, oder die Datei ist leer.");

            using (var scope = _serviceProvider.CreateScope())
            {
                var _csvReader = scope.ServiceProvider.GetRequiredService<ICsvReader>();
                var _supplierRepository = scope.ServiceProvider.GetRequiredService<ISupplierRepository>();

                _csvReader.Read(file);
                string[] requiredKeys = { "Name" };
                if (_csvReader.IsValid && _csvReader.ContainsKeys(requiredKeys))
                {
                    var keys = _csvReader.Keys;
                    var values = _csvReader.Values;

                    List<Supplier> supplier = new List<Supplier>();
                    List<SupplierNumber> supplierNumbers = new List<SupplierNumber>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        Supplier sup = new Supplier();
                        List<SupplierNumber> iterSupplierNumber = new List<SupplierNumber>();
                        try
                        {
                            for (int j = 0; j < keys.Length; j++)
                            {
                                switch (keys[j].ToUpper())
                                {
                                    case "NAME":
                                        sup.Name = values[i][j];
                                        break;
                                    case "COLORCODE":
                                        sup.ColorCode = values[i][j];
                                        break;
                                    case "SUPPLIERNUMBERS":
                                        foreach (string number in values[i][j].Split(','))
                                        {
                                            iterSupplierNumber.Add(new SupplierNumber() { Number = number });
                                        }
                                        break;
                                }
                            }
                            foreach (SupplierNumber supplierNumber in iterSupplierNumber)
                            {
                                supplierNumber.SupplierName = sup.Name;
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError("Could not parse csv values into supplier values. Message: " + e.Message);
                            throw new Exception("Daten konnten nicht importiert werden. Die Datentypen stimmen nicht.");
                        }
                        if (string.IsNullOrWhiteSpace(sup.ColorCode))
                            sup.ColorCode = "#ffffff";
                        supplier.Add(sup);
                        supplierNumbers.AddRange(iterSupplierNumber);
                    }

                    try
                    {
                        await _supplierRepository.Import(supplier, supplierNumbers);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                else
                {
                    _logger.LogError("Could not import supplier from CSV. File was invalid or did not contain the required keys.");
                    throw new Exception("Datei konnte nicht importiert werden. Stellen Sie sicher, dass sie der Beschreibung entspricht.");
                }
            }
        }

        #endregion

        #region ParcelService
        public ParcelServicesIndexViewModel GetParcelservicesIndexViewModel()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _parcelServicesRepository = scope.ServiceProvider.GetRequiredService<IParcelServicesRepository>();
                var model = new ParcelServicesIndexViewModel();
                model.ParcelServices = _parcelServicesRepository.GetAll();
                return model;
            }
        }

        public ParcelServiceViewModel GetParcelServiceViewModel()
        {
            var model = new ParcelServiceViewModel();
            model.ParcelService = new ParcelService();
            return model;
        }

        public ParcelServiceViewModel GetParcelServiceViewModel(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _parcelServicesRepository = scope.ServiceProvider.GetRequiredService<IParcelServicesRepository>();
                var model = new ParcelServiceViewModel();
                model.ParcelService = _parcelServicesRepository.Get(id);
                return model;
            }
        }

        public async Task AddParcelService(ParcelServiceViewModel model)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _parcelServicesRepository = scope.ServiceProvider.GetRequiredService<IParcelServicesRepository>();
                if (model == null)
                {
                    throw new ArgumentNullException("ParcelServiceViewModel == null");
                }
                if (model.ParcelService == null)
                {
                    throw new ArgumentNullException("ParcelServiceViewModel.ParcelService == null");
                }
                await _parcelServicesRepository.Add(model.ParcelService);
            }
        }

        public async Task EditParcelService(ParcelServiceViewModel model)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _parcelServicesRepository = scope.ServiceProvider.GetRequiredService<IParcelServicesRepository>();
                if (model == null)
                {
                    throw new ArgumentNullException("ParcelServiceViewModel == null");
                }
                if (model.ParcelService == null)
                {
                    throw new ArgumentNullException("ParcelServiceViewModel.ParcelService == null");
                }
                await _parcelServicesRepository.Edit(model.ParcelService);
            }
        }

        public async Task DeleteParcelService(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _parcelServiceRepository = scope.ServiceProvider.GetRequiredService<IParcelServicesRepository>();
                await _parcelServiceRepository.Delete(id);
            }
        }


        /// <summary>
        /// Imports Parcelservices from a csv file. All Parcelservices that
        /// are not in the file will be removed from the DB.
        /// The following Keys are required:
        /// - Name
        /// The Name is a string.
        /// </summary>
        /// <param name="file">The CSV file containing the parcial services</param>
        /// <returns></returns>
        public async Task ImportParcelServiceFromCSV(IFormFile file)
        {
            if (file == null)
                throw new Exception("Es wurde keine Datei ausgewählt, oder die Datei ist leer.");

            _logger.LogInformation("Importing Parcial services from a csv file.");
            using (var scope = _serviceProvider.CreateScope())
            {
                var _csvReader = scope.ServiceProvider.GetRequiredService<ICsvReader>();
                var _parcelServicesRepository = scope.ServiceProvider.GetRequiredService<IParcelServicesRepository>();

                _csvReader.Read(file);
                string[] requiredKeys = { "Name" };
                if (_csvReader.IsValid && _csvReader.ContainsKeys(requiredKeys))
                {
                    var keys = _csvReader.Keys;
                    var values = _csvReader.Values;

                    List<ParcelService> parcelServices = new List<ParcelService>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        ParcelService parcelService = new ParcelService();
                        try
                        {
                            for (int j = 0; j < keys.Length; j++)
                            {
                                switch (keys[j].ToUpper())
                                {
                                    case "NAME":
                                        parcelService.Name = values[i][j];
                                        break;
                                    case "COLORCODE":
                                        parcelService.ColorCode = values[i][j];
                                        break;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError("Could not parse csv values into Parcelservices values. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                            throw new Exception("Daten konnten nicht importiert werden. Die Datentypen stimmen nicht.");
                        }
                        if (string.IsNullOrWhiteSpace(parcelService.ColorCode))
                            parcelService.ColorCode = "#ffffff";
                        parcelServices.Add(parcelService);
                    }

                    try
                    {
                        await _parcelServicesRepository.Import(parcelServices);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                else
                {
                    _logger.LogError("Could not import forwarding agencies from CSV. File was invalid or did not contain the required keys.");
                    throw new Exception("Datei konnte nicht importiert werden. Stellen Sie sicher, dass sie der Beschreibung entspricht.");
                }
            }
        }
        #endregion

        #region Fitter
        public FittersIndexViewModel GetFittersIndexViewModel()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _FittersRepository = scope.ServiceProvider.GetRequiredService<IFittersRepository>();
                var model = new FittersIndexViewModel();
                model.Fitters = _FittersRepository.GetAll();
                return model;
            }
        }

        public FitterViewModel GetFitterViewModel()
        {
            var model = new FitterViewModel();
            model.Fitter = new Fitter();
            return model;
        }

        public FitterViewModel GetFitterViewModel(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _FittersRepository = scope.ServiceProvider.GetRequiredService<IFittersRepository>();
                var model = new FitterViewModel();
                model.Fitter = _FittersRepository.Get(id);
                return model;
            }
        }

        public async Task AddFitter(FitterViewModel model)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _FittersRepository = scope.ServiceProvider.GetRequiredService<IFittersRepository>();
                if (model == null)
                {
                    throw new ArgumentNullException("FitterViewModel == null");
                }
                if (model.Fitter == null)
                {
                    throw new ArgumentNullException("FitterViewModel.Fitter == null");
                }
                await _FittersRepository.Add(model.Fitter);
            }
        }

        public async Task EditFitter(FitterViewModel model)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _FittersRepository = scope.ServiceProvider.GetRequiredService<IFittersRepository>();
                if (model == null)
                {
                    throw new ArgumentNullException("FitterViewModel == null");
                }
                if (model.Fitter == null)
                {
                    throw new ArgumentNullException("FitterViewModel.Fitter == null");
                }
                await _FittersRepository.Edit(model.Fitter);
            }
        }

        public async Task DeleteFitter(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _FitterRepository = scope.ServiceProvider.GetRequiredService<IFittersRepository>();
                await _FitterRepository.Delete(id);
            }
        }


        /// <summary>
        /// Imports Fitters from a csv file. All Fitters that
        /// are not in the file will be removed from the DB.
        /// The following Keys are required:
        /// - Name
        /// The Name is a string.
        /// </summary>
        /// <param name="file">The CSV file containing the parcial services</param>
        /// <returns></returns>
        public async Task ImportFittersFromCSV(IFormFile file)
        {
            if (file == null)
                throw new Exception("Es wurde keine Datei ausgewählt, oder die Datei ist leer.");

            _logger.LogInformation("Importing Parcial services from a csv file.");
            using (var scope = _serviceProvider.CreateScope())
            {
                var _csvReader = scope.ServiceProvider.GetRequiredService<ICsvReader>();
                var _FittersRepository = scope.ServiceProvider.GetRequiredService<IFittersRepository>();

                _csvReader.Read(file);
                string[] requiredKeys = { "Name" };
                if (_csvReader.IsValid && _csvReader.ContainsKeys(requiredKeys))
                {
                    var keys = _csvReader.Keys;
                    var values = _csvReader.Values;

                    List<Fitter> Fitters = new List<Fitter>();

                    for (int i = 0; i < values.Length; i++)
                    {
                        Fitter Fitter = new Fitter();
                        try
                        {
                            for (int j = 0; j < keys.Length; j++)
                            {
                                switch (keys[j].ToUpper())
                                {
                                    case "NAME":
                                        Fitter.Name = values[i][j];
                                        break;
                                    case "COLORCODE":
                                        Fitter.ColorCode = values[i][j];
                                        break;
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            _logger.LogError("Could not parse csv values into Fitters values. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                            throw new Exception("Daten konnten nicht importiert werden. Die Datentypen stimmen nicht.");
                        }
                        if (string.IsNullOrWhiteSpace(Fitter.ColorCode))
                            Fitter.ColorCode = "#ffffff";
                        Fitters.Add(Fitter);
                    }

                    try
                    {
                        await _FittersRepository.Import(Fitters);
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                else
                {
                    _logger.LogError("Could not import forwarding agencies from CSV. File was invalid or did not contain the required keys.");
                    throw new Exception("Datei konnte nicht importiert werden. Stellen Sie sicher, dass sie der Beschreibung entspricht.");
                }
            }
        }
        #endregion

        #region UnknownForwardingAgencies
        public UnknownForwardingAgenciesViewModel GetUnknownForwardingAgenciesViewModel()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _unknownForwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IUnknownForwardingAgenciesRepository>();
                var model = new UnknownForwardingAgenciesViewModel();
                model.UnknownForwardingAgencies = _unknownForwardingAgenciesRepository.GetAll();
                return model;
            }
        }

        public FileStream ExportUnknownForwardingAgenciesCSVStream()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _exportFacade = scope.ServiceProvider.GetRequiredService<IExportFacade>();
                    Directory.CreateDirectory("DataExport");
                    StreamWriter writer = new StreamWriter("DataExport/UnknownForwardingAgencies.csv");
                    var data = _exportFacade.GenerateUnknownForwardingAgenciesCSVData();
                    writer.Write(data);
                    writer.Flush();
                    writer.Close();
                    return new FileStream("DataExport/UnknownForwardingAgencies.csv", FileMode.Open);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not Export UnknownForwardingAgencies. Error while creating Filestream");
                throw e;
            }
        }

        public FileStream ExportUnknownForwardingAgenciesXMLStream()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _exportFacade = scope.ServiceProvider.GetRequiredService<IExportFacade>();
                    Directory.CreateDirectory("DataExport");
                    StreamWriter writer = new StreamWriter("DataExport/UnknownForwardingAgencies.xml");
                    var data = _exportFacade.GenerateUnknownForwardingAgenciesXMLData();
                    writer.Write(data);
                    writer.Flush();
                    writer.Close();
                    return new FileStream("DataExport/UnknownForwardingAgencies.xml", FileMode.Open);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not export UnknownForwardingAgencies. Error while creating the filestream.");
                throw e;
            }
        }

        public async Task DeleteUnknownForwardingAgency(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _unknownForwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IUnknownForwardingAgenciesRepository>();
                await _unknownForwardingAgenciesRepository.Delete(id);
            }
        }
        #endregion

        #region UnknownSuppliers
        public List<UnknownSupplier> GetUnknownSuppliers()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _unknownSupplierRepository = scope.ServiceProvider.GetRequiredService<IUnknownSupplierRepository>();
                return _unknownSupplierRepository.GetAll();
            }
        }

        public async Task DeleteUnknownSupplier(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _unknownSupplierRepository = scope.ServiceProvider.GetRequiredService<IUnknownSupplierRepository>();
                await _unknownSupplierRepository.Delete(id);
            }
        }

        public FileStream ExportUnknownSuppliersCSVStream()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _exportFacade = scope.ServiceProvider.GetRequiredService<IExportFacade>();
                    Directory.CreateDirectory("DataExport");
                    StreamWriter writer = new StreamWriter("DataExport/UnknownSuppliers.csv");
                    var data = _exportFacade.GenerateUnknownSuppliersCSVData();
                    writer.Write(data);
                    writer.Flush();
                    writer.Close();
                    return new FileStream("DataExport/UnknownSuppliers.csv", FileMode.Open);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not Export unknown suppliers. Error while creating Filestream");
                throw e;
            }
        }
        #endregion

        #region UnknownParcelServices
        public List<UnknownParcelService> GetUnknownParcelServices()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _unknownParcelServiceRepository = scope.ServiceProvider.GetRequiredService<IUnknownParcelServiceRepository>();
                return _unknownParcelServiceRepository.GetAll();
            }
        }

        public async Task DeleteUnknownParcelService(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _unknownParcelServiceRepository = scope.ServiceProvider.GetRequiredService<IUnknownParcelServiceRepository>();
                await _unknownParcelServiceRepository.Delete(id);
            }
        }

        public FileStream ExportUnknownParcelServicesCSVStream()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _exportFacade = scope.ServiceProvider.GetRequiredService<IExportFacade>();
                    Directory.CreateDirectory("DataExport");
                    StreamWriter writer = new StreamWriter("DataExport/UnknownParcelServices.csv");
                    var data = _exportFacade.GenerateUnknownParcelServicesCSVData();
                    writer.Write(data);
                    writer.Flush();
                    writer.Close();
                    return new FileStream("DataExport/UnknownParcelServices.csv", FileMode.Open);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not Export unknown ParcelServices. Error while creating Filestream");
                throw e;
            }
        }
        #endregion

        #region UnknownFitters
        public List<UnknownFitter> GetUnknownFitters()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _unknownFitterRepository = scope.ServiceProvider.GetRequiredService<IUnknownFitterRepository>();
                return _unknownFitterRepository.GetAll();
            }
        }

        public async Task DeleteUnknownFitter(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _unknownFitterRepository = scope.ServiceProvider.GetRequiredService<IUnknownFitterRepository>();
                await _unknownFitterRepository.Delete(id);
            }
        }

        public FileStream ExportUnknownFittersCSVStream()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _exportFacade = scope.ServiceProvider.GetRequiredService<IExportFacade>();
                    Directory.CreateDirectory("DataExport");
                    StreamWriter writer = new StreamWriter("DataExport/UnknownFitters.csv");
                    var data = _exportFacade.GenerateUnknownFittersCSVData();
                    writer.Write(data);
                    writer.Flush();
                    writer.Close();
                    return new FileStream("DataExport/UnknownFitters.csv", FileMode.Open);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not Export unknown Fitters. Error while creating Filestream");
                throw e;
            }
        }
        #endregion

        #region Displays

        public DisplayIndexViewModel GetDisplayIndexViewModel()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _displayConfigurationRepository = scope.ServiceProvider.GetRequiredService<IDisplayConfigurationRepository>();
                var model = new DisplayIndexViewModel();
                model.Displays = _displayConfigurationRepository.GetAll();
                return model;
            }
        }

        public async Task AddDisplay(DisplayConfiguration displayConfig)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _displayConfigurationRepository = scope.ServiceProvider.GetRequiredService<IDisplayConfigurationRepository>();
                await _displayConfigurationRepository.Add(displayConfig);
            }
        }

        public async Task EditDisplay(DisplayConfiguration displayConfiguration)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _displayConfigurationRepository = scope.ServiceProvider.GetRequiredService<IDisplayConfigurationRepository>();
                await _displayConfigurationRepository.EditAsync(displayConfiguration);
            }
        }

        public async Task DeleteDisplay(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _displayConfigurationRepository = scope.ServiceProvider.GetRequiredService<IDisplayConfigurationRepository>();
                await _displayConfigurationRepository.Delete(id);
            }
        }

        public DisplayConfiguration GetDisplayConfiguration(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _displayConfigurationRepository = scope.ServiceProvider.GetRequiredService<IDisplayConfigurationRepository>();
                return _displayConfigurationRepository.Get(id);
            }
        }

        #endregion

        #region Users

        public UserIndexViewModel GetUserIndexViewModel()
        {
            return new UserIndexViewModel() { Users = _userManager.Users.ToList() };
        }

        public async Task<IdentityResult> AddUser(UserViewModel model)
        {
            AppUser user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email,
                AuthorizationGroup = model.Group,

            };
            IdentityResult result = null;

            try
            {
                result = await _userManager.CreateAsync(user, model.Password);
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying _userManager.CreateAsync. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Fehler beim Erstellen des Benutzers.");
            }
            if (result.Succeeded)
                await SetUserRoles(user);
            return result;

        }

        public async Task<IdentityResult> DeleteUser(string id)
        {

            AppUser user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                var error = new IdentityError();
                error.Description = "Benutzer nicht gefunden!";
                var result = IdentityResult.Failed(error);
                return result;
            }
            return await _userManager.DeleteAsync(user);

        }

        public async Task<UserViewModel> GetUserViewModel(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return null;
            var model = new UserViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
                Groups = AuthorizationGroups(),
                Group = user.AuthorizationGroup
            };
            return model;

        }

        public UserViewModel GetUserViewModel()
        {
            var model = new UserViewModel
            {
                Groups = AuthorizationGroups()
            };
            return model;
        }

        public List<string> AuthorizationGroups()
        {
            var all = _groupsRepository.GetAll();
            var list = new List<string>();

            foreach (var g in all)
            {
                list.Add(g.Name);
            }

            return list;
        }

        public async Task<IdentityResult> EditUser(UserViewModel model, IUserValidator<AppUser> _userValidator, IPasswordHasher<AppUser> _passwordHasher, IPasswordValidator<AppUser> _passwordValidator)
        {

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.Email = model.Email;
                user.UserName = model.UserName;
                user.AuthorizationGroup = model.Group;

                IdentityResult validEmail = await _userValidator.ValidateAsync(_userManager, user);
                if (!validEmail.Succeeded)
                {
                    return validEmail;
                }

                if (!String.IsNullOrEmpty(model.Password))
                {
                    var validPass = await _passwordValidator.ValidateAsync(_userManager, user, model.Password);
                    if (validPass.Succeeded)
                    {
                        user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                    }
                    else
                    {
                        return validPass;
                    }
                }
                await SetUserRoles(user);
                return await _userManager.UpdateAsync(user);
            }
            else
            {
                var error = new IdentityError();
                error.Description = "Benutzer wurde nicht gefunden!";
                return IdentityResult.Failed(error);
            }
        }

        private async Task SetUserRoles(AppUser user)
        {
            // INFO: does not work when i use scopes on the usermanager because the same user will be tracked by multipe usermanagers which will result in an error. 

            try
            {
                var roles = await _userManager.GetRolesAsync(user);
                IdentityResult resultRemove = null;
                if (String.IsNullOrEmpty(user.AuthorizationGroup))
                {
                    resultRemove = await _userManager.RemoveFromRolesAsync(user, roles);
                    return;
                }

                var group = _groupsRepository.Get(user.AuthorizationGroup);
                List<string> addRoles = new List<string>();
                // Processing
                if (group.CanUseProcessingList)
                    addRoles.Add("CanUseProcessingList");

                if (group.CanModifyProcessingList)
                    addRoles.Add("CanModifyProcessingList");

                if (group.CanSetLoadingStation)
                    addRoles.Add("CanSetLoadingStation");

                if (group.CanSetGate)
                    addRoles.Add("CanSetGate");

                if (group.CanSetRelease)
                    addRoles.Add("CanSetRelease");

                if (group.CanSetCall)
                    addRoles.Add("CanSetCall");

                if (group.CanSetEntrance)
                    addRoles.Add("CanSetEntrance");

                if (group.CanSetProcessStart)
                    addRoles.Add("CanSetProcessStart");

                if (group.CanSetProcessEnd)
                    addRoles.Add("CanSetProcessEnd");

                if (group.CanSetExit)
                    addRoles.Add("CanSetExit");

                // History
                if (group.CanUseHistory)
                    addRoles.Add("CanUseHistory");

                if (group.CanExportHistory)
                    addRoles.Add("CanExportHistory");

                // Config
                if (group.CanUseConfig)
                    addRoles.Add("CanUseConfig");

                if (group.CanModifyAllSettings)
                    addRoles.Add("CanModifyAllSettings");

                if (group.CanInspectApproachTyps)
                    addRoles.Add("CanInspectApproachTyps");

                if (group.CanModifyApproachTyps)
                    addRoles.Add("CanModifyApproachTyps");

                if (group.CanInspectUnknownApproachTyps)
                    addRoles.Add("CanInspectUnknownApproachTyps");

                if (group.CanModifyUnknownApproachTyps)
                    addRoles.Add("CanModifyUnknownApproachTyps");

                if (roles != null && roles.Count > 0)
                    resultRemove = await _userManager.RemoveFromRolesAsync(user, roles);
                if (addRoles != null && addRoles.Count > 0)
                {
                    var resultAdd = await _userManager.AddToRolesAsync(user, addRoles);
                }

            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set userRoles. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Fehler beim setzen der Benutzerrollen.");
            }
        }

        #endregion

        #region AuthorizationGroups

        public AuthorizationGroupsViewModel GetAuthorizationGroupsViewModel()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _groupsRepository = scope.ServiceProvider.GetRequiredService<IAuthorizationGroupsRepository>();
                return new AuthorizationGroupsViewModel { Groups = _groupsRepository.GetAll() };
            }
        }

        public async Task AddAuthorizationGroup(GroupViewModel model)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _groupsRepository = scope.ServiceProvider.GetRequiredService<IAuthorizationGroupsRepository>();
                await _groupsRepository.Add(model.Group);
            }
        }

        public async Task EditAuthorizationGroup(GroupViewModel model)
        {
            //using (var scope = _serviceProvider.CreateScope())
            //{
            //    var _groupsRepository = scope.ServiceProvider.GetRequiredService<IAuthorizationGroupsRepository>();
            //    var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            var oldVersion = _groupsRepository.Get(model.Group.ID);
            if (oldVersion == null)
            {
                _logger.LogInformation("Could not edit Authorization group because the premodified version could not be found.");
                throw new Exception("Fehler beim Bearbeiten der Berechtigungsgruppe.");
            }
            var users = (from u in _userManager.Users
                         where u.AuthorizationGroup == oldVersion.Name
                         select u).ToList();

            await _groupsRepository.Edit(model.Group);

            foreach (var user in users)
            {
                user.AuthorizationGroup = model.Group.Name;
                await SetUserRoles(user);
                await _userManager.UpdateAsync(user);
            }
            //}
        }

        public async Task DeleteAuthorizationGroup(int id)
        {
            //using (var scope = _serviceProvider.CreateScope())
            //{
            //    var _groupsRepository = scope.ServiceProvider.GetRequiredService<IAuthorizationGroupsRepository>();
            //    var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            var oldVersion = _groupsRepository.Get(id);
            if (oldVersion == null)
            {
                _logger.LogInformation("Could not delete Authorization group because the group could not be found.");
                throw new Exception("Fehler beim Löschen der Berechtigungsgruppe.");
            }
            var users = (from u in _userManager.Users
                         where u.AuthorizationGroup == oldVersion.Name
                         select u).ToList();

            foreach (var user in users)
            {
                user.AuthorizationGroup = "";
                await SetUserRoles(user);
                await _userManager.UpdateAsync(user);
            }

            await _groupsRepository.Delete(id);
            //}
        }

        public GroupViewModel GetGroupViewModel(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _groupsRepository = scope.ServiceProvider.GetRequiredService<IAuthorizationGroupsRepository>();
                return new GroupViewModel { Group = _groupsRepository.Get(id) };
            }
        }
        #endregion

        #region Active Directory

        public ADSettings GetADSettings()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _adSettingsRepository = scope.ServiceProvider.GetRequiredService<IADSettingsRepository>();
                return _adSettingsRepository.Get();
            }
        }

        public async Task SetADSettings(ADSettings settings)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _adSettingsRepository = scope.ServiceProvider.GetRequiredService<IADSettingsRepository>();
                await _adSettingsRepository.Set(settings);
            }
        }

        #endregion

        #region BarrierControlSettings
        public BarrierControlSettingsViewModel GetBarrierControlSettings()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IBarrierControlSettingsRepository>();
                var settings = repo.Get();

                var vm = new BarrierControlSettingsViewModel
                {
                    EntryBarrierAPIUrl = settings.EntryBarrierAPIUrl,
                    ExitBarrierAPIUrl = settings.ExitBarrierAPIUrl,
                    UseBarrierControl = settings.UseBarrierControl
                };

                return vm;
            }
        }

        public void SetBarrierControlSettings(BarrierControlSettingsViewModel vm)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<IBarrierControlSettingsRepository>();
                var settings = repo.Get();
                settings.EntryBarrierAPIUrl = vm.EntryBarrierAPIUrl;
                settings.ExitBarrierAPIUrl = vm.ExitBarrierAPIUrl;
                settings.UseBarrierControl = vm.UseBarrierControl;

                repo.Update(settings);
            }
        }

        #endregion

        #region SMSSettings
        public SMSSettings GetSMSSettings()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _smsSettingsRepository = scope.ServiceProvider.GetRequiredService<ISMSSettingsRepository>();
                var _protectionProvider = scope.ServiceProvider.GetRequiredService<IDataProtectionProvider>();
                var _smsUserNameProtector = _protectionProvider.CreateProtector("SMSUsernameProtector");
                var _smsPasswordProtector = _protectionProvider.CreateProtector("SMSPasswordProtector");
                var _smsAccountReferenceProtector = _protectionProvider.CreateProtector("SMSAccountReferenceProtector");

                var settings = _smsSettingsRepository.Get();

                var unprotectedSettings = new SMSSettings
                {
                    Username = String.IsNullOrEmpty(settings.Username) ? "" : _smsUserNameProtector.Unprotect(settings.Username),
                    Password = String.IsNullOrEmpty(settings.Password) ? "" : _smsPasswordProtector.Unprotect(settings.Password),
                    AccountReference = String.IsNullOrEmpty(settings.AccountReference) ? "" : _smsAccountReferenceProtector.Unprotect(settings.AccountReference),
                    ID = settings.ID,
                    UseSMSService = settings.UseSMSService
                };

                return unprotectedSettings;
            }
        }

        public async Task SetSMSSettings(SMSSettings settings)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _smsSettingsRepository = scope.ServiceProvider.GetRequiredService<ISMSSettingsRepository>();
                var _protectionProvider = scope.ServiceProvider.GetRequiredService<IDataProtectionProvider>();
                var _smsUserNameProtector = _protectionProvider.CreateProtector("SMSUsernameProtector");
                var _smsPasswordProtector = _protectionProvider.CreateProtector("SMSPasswordProtector");
                var _smsAccountReferenceProtector = _protectionProvider.CreateProtector("SMSAccountReferenceProtector");

                settings.Username = _smsUserNameProtector.Protect(settings.Username);
                settings.Password = _smsPasswordProtector.Protect(settings.Password);
                settings.AccountReference = _smsAccountReferenceProtector.Protect(settings.AccountReference);
                await _smsSettingsRepository.Set(settings);
            }
        }

        #endregion
    }
}
