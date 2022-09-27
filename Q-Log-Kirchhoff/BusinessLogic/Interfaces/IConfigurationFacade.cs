using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using MVC.Data.Entities;
using MVC.Models.ConfigurationViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Interfaces
{
    public interface IConfigurationFacade
    {

        #region GeneralSettings
        /// <summary>
        /// Returns the GeneralSettings object from the DB
        /// </summary>
        /// <returns></returns>
        GeneralSettings GetGeneralSettings();

        /// <summary>
        /// Generates the GeneralSettingsViewModel from the GeneralSettings
        /// </summary>
        /// <returns></returns>
        GeneralSettingsViewModel GetGeneralSettingsViewModel();

        /// <summary>
        /// Accquires the GeneralSettings from the Viewmodel and saves them to the DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task SetGeneralSettings(GeneralSettingsViewModel model);
        #endregion

        #region TerminalSettings
        /// <summary>
        /// Gets the TerminalSettings object from the DB and returns it
        /// </summary>
        /// <returns></returns>
        TerminalSettings GetTerminalSettingsViewModel();

        /// <summary>
        /// Saves the Terminalsettings to the DB
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        Task SetTerminalSettings(TerminalSettings settings);
        #endregion


        #region LoadingStations
        /// <summary>
        /// Gets the current LoadingStation settings from the repository
        /// and generates the ConfigurationLoadingStationsViewModel with the accquired 
        /// Information.
        /// </summary>
        /// <returns></returns>
        LoadingStationIndexViewModel GetLoadingStationsViewModel();

        /// <summary>
        /// Tells the ILoadingStationsRepository to add the passed along LoadingStation.
        /// </summary>
        /// <param name="LoadingStation"></param>
        /// <returns></returns>
        Task AddLoadingStation(LoadingStation LoadingStation);


        /// <summary>
        /// Tells the ILoadingStationsRepositoy to delete the LoadingStation with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteLoadingStation(int id);

        /// <summary>
        /// Tells the ILoadingStationsRepository to update the information
        /// in the database to match the new values of the passed along LoadingStation.
        /// </summary>
        /// <param name="LoadingStation"></param>
        /// <returns></returns>
        Task EditLoadingStation(LoadingStation LoadingStation);

        /// <summary>
        /// Gets and returns the LoadingStation with the given id from the
        /// ILoadingStationsRepository.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>LoadingStation</returns>
        LoadingStation GetLoadingStation(int id);

        /// <summary>
        /// Gets and returns a list of all LoadingStations from the
        /// ILoadingStationsRepository.
        /// </summary>
        /// <returns></returns>
        List<LoadingStation> GetAllLoadingStations();

        /// <summary>
        /// Utilizes the ICsvReader to generate LoadingStations from the
        /// csv-file and tells the ILoadingStationsRepository to add the LoadingStations to the DB.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task ImportLoadingStationsFromCSV(IFormFile file);
        #endregion

        #region Gates
        /// <summary>
        /// Gets the current Gate settings from the repository
        /// and generates the ConfigurationGatesViewModel with the accquired 
        /// Information.
        /// </summary>
        /// <returns></returns>
        ConfigurationGatesViewModel GetGatesViewModel();

        /// <summary>
        /// Tells the IGatesRepository to add the passed along Gate.
        /// </summary>
        /// <param name="gate"></param>
        /// <returns></returns>
        Task AddGate(Gate gate);


        /// <summary>
        /// Tells the IGatesRepositoy to delete the Gate with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteGate(int id);

        /// <summary>
        /// Tells the IGatesRepository to update the information
        /// in the database to match the new values of the passed along gate.
        /// </summary>
        /// <param name="gate"></param>
        /// <returns></returns>
        Task EditGate(Gate gate);

        /// <summary>
        /// Gets and returns the gate with the given id from the
        /// IGatesRepository.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Gate</returns>
        Gate GetGate(int id);

        /// <summary>
        /// Gets and returns a list of all gates from the
        /// IGatesRepository.
        /// </summary>
        /// <returns></returns>
        List<Gate> GetAllGates();

        /// <summary>
        /// Utilizes the ICsvReader to generate gates from the
        /// csv-file and tells the IGatesRepository to add the gates to the DB.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task ImportGatesFromCSV(IFormFile file);
        #endregion

        #region ForwardingAgencies
        /// <summary>
        /// Accquires the list of all ForwardingAgencies from the IForwardingRepository
        /// to create the ForwardingAgenciesIndexViewModel.
        /// </summary>
        /// <returns></returns>
        ForwardingAgenciesIndexViewModel GetForwardingAgenciesIndexViewModel();

        /// <summary>
        /// Creates a ForwardingAgencyViewModel with a new but empty ForwardingAgency 
        /// and the List of all Gates from the IGatesRepository.
        /// </summary>
        /// <returns></returns>
        ForwardingAgencyViewModel GetForwardingAgencyViewModel();

        /// <summary>
        /// Creates a ForwardingAgencyViewModel, filled with the
        /// ForwardingAgency with the given ID, as accquired from the IForwardingAgenciesRepository, and the list of all Gates
        /// from the IGatesRepository.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ForwardingAgencyViewModel GetForwardingAgencyViewModel(int id);

        /// <summary>
        /// Takes the ForwardingAgency from the model and tells the
        /// IForwardingAgenciesRepository to add it to the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AddForwardingAgency(ForwardingAgencyViewModel model);

        /// <summary>
        /// Tells the IForwardingAgenciesRepository to delete the
        /// ForwardingAgency with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteForwardingAgency(int id);

        /// <summary>
        /// Gets the forwardingAgency from the model and tells
        /// the IForwardingAgenciesRepository to update the data for that Agency.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task EditForwardingAgency(ForwardingAgencyViewModel model);

        /// <summary>
        /// Uses the ICsvReader to create ForwardingAgencies from the 
        /// CSV-file and tells the IForwardingRepository to add the agencies 
        /// to the db.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task ImportForwardingAgenciesFromCSV(IFormFile file);
        #endregion

        #region Suppliers
        Task<bool> getSupplierFromERP();

        SuppliersIndexViewModel GetSuppliersIndexViewModel();

        SupplierViewModel GetSupplierViewModel();

        SupplierViewModel GetSupplier(int id);

        Task AddSupplier(SupplierViewModel supplier);

        Task EditSupplier(SupplierViewModel supplier);

        Task DeleteSupplier(int id);

        Task ImportSuppliersFromCSV(IFormFile file);

        #endregion

        #region Parcelservices
        /// <summary>
        /// Accquires the list of all Parcelservices from the IParcelservicesRepository
        /// to create the Parcelservices IndexViewModel.
        /// </summary>
        /// <returns></returns>
        ParcelServicesIndexViewModel GetParcelservicesIndexViewModel();

        /// <summary>
        /// Creates a ParcelServiceViewModel with a new but empty ForwardParcelServiceingAgency 
        /// </summary>
        /// <returns></returns>
        ParcelServiceViewModel GetParcelServiceViewModel();

        /// <summary>
        /// Creates a ParcelServiceViewModel, filled with the
        /// ParcelService with the given ID, as accquired from the IParcelServicesRepository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ParcelServiceViewModel GetParcelServiceViewModel(int id);

        /// <summary>
        /// Takes the ParcelService from the model and tells the
        /// IParcelServicesRepository to add it to the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AddParcelService(ParcelServiceViewModel model);

        /// <summary>
        /// Tells the IParcelServiceRepository to delete the
        /// ParcelService with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteParcelService(int id);

        /// <summary>
        /// Gets the ParcelService from the model and tells
        /// the IParcelServiceRepository to update the data for that Agency.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task EditParcelService(ParcelServiceViewModel model);

        /// <summary>
        /// Uses the ICsvReader to create ParcelServices from the 
        /// CSV-file and tells the IParcelService to add the ParcelService 
        /// to the db.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task ImportParcelServiceFromCSV(IFormFile file);
        #endregion

        #region Fitters
        /// <summary>
        /// Accquires the list of all Fitters from the IFittersRepository
        /// to create the Fitters IndexViewModel.
        /// </summary>
        /// <returns></returns>
        FittersIndexViewModel GetFittersIndexViewModel();

        /// <summary>
        /// Creates a FitterViewModel with a new but empty ForwardFitteringAgency 
        /// </summary>
        /// <returns></returns>
        FitterViewModel GetFitterViewModel();

        /// <summary>
        /// Creates a FitterViewModel, filled with the
        /// Fitter with the given ID, as accquired from the IFittersRepository
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        FitterViewModel GetFitterViewModel(int id);

        /// <summary>
        /// Takes the Fitter from the model and tells the
        /// IFittersRepository to add it to the DB.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AddFitter(FitterViewModel model);

        /// <summary>
        /// Tells the IFitterRepository to delete the
        /// Fitter with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteFitter(int id);

        /// <summary>
        /// Gets the Fitter from the model and tells
        /// the IFitterRepository to update the data for that Agency.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task EditFitter(FitterViewModel model);

        /// <summary>
        /// Uses the ICsvReader to create Fitters from the 
        /// CSV-file and tells the IFitter to add the Fitter 
        /// to the db.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task ImportFittersFromCSV(IFormFile file);
        #endregion

        #region UnknownForwardingAgencies
        /// <summary>
        /// Gets the list of all unknownForwardingAgencies from the IUnknownForwardingAgenciesRepository
        /// to create the ViewModel.
        /// </summary>
        /// <returns></returns>
        UnknownForwardingAgenciesViewModel GetUnknownForwardingAgenciesViewModel();

        /// <summary>
        /// Creates the unkownSuppliers csv file and creates a filestream with access to it.
        /// </summary>
        /// <exception cref="Exception"></exception>
        /// <returns>FileStream</returns>
        FileStream ExportUnknownForwardingAgenciesCSVStream();

        /// <summary>
        /// Creates the unkownSuppliers xml file and creates a filestream with access to it.
        /// </summary>
        /// <exception cref="Exception"></exception>
        /// <returns>FileStream</returns>
        FileStream ExportUnknownForwardingAgenciesXMLStream();

        Task DeleteUnknownForwardingAgency(int id);
        #endregion

        #region UnknownSuppliers
        List<UnknownSupplier> GetUnknownSuppliers();

        Task DeleteUnknownSupplier(int id);

        FileStream ExportUnknownSuppliersCSVStream();
        #endregion

        #region UnknownParcelServices
        List<UnknownParcelService> GetUnknownParcelServices();

        Task DeleteUnknownParcelService(int id);

        FileStream ExportUnknownParcelServicesCSVStream();
        #endregion

        #region UnknownFitters
        List<UnknownFitter> GetUnknownFitters();

        Task DeleteUnknownFitter(int id);

        FileStream ExportUnknownFittersCSVStream();
        #endregion

        #region Displays
        /// <summary>
        /// Gets the List of all DisplayConfigurations from the IDisplayConfigurationRepository
        /// to create the ViewModel.
        /// </summary>
        /// <returns></returns>
        DisplayIndexViewModel GetDisplayIndexViewModel();

        /// <summary>
        /// Tells the IDisplayConfigurationrepository to add the new displayConfig.
        /// </summary>
        /// <param name="displayConfig"></param>
        /// <returns></returns>
        Task AddDisplay(DisplayConfiguration displayConfig);

        /// <summary>
        /// Tells the IDisplayConfigurationRepository to remove the displayConfig with
        /// the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteDisplay(int id);

        /// <summary>
        /// Calls the DisplayConfigurationRepository to edit
        /// the DB values for the passed along displayConfig.
        /// </summary>
        /// <param name="displayConfiguration"></param>
        /// <returns></returns>
        Task EditDisplay(DisplayConfiguration displayConfiguration);

        /// <summary>
        /// Gets the DisplayConfiguration with the given ID from the
        /// IDisplayConfigurationRepository.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DisplayConfiguration GetDisplayConfiguration(int id);

        #endregion

        #region Users

        /// <summary>
        /// Gets the List of all Users from the UserManager and 
        /// creates the ViewModel with it.
        /// </summary>
        /// <returns></returns>
        UserIndexViewModel GetUserIndexViewModel();

        /// <summary>
        /// Creates an AppUser from the data provided by the 
        /// model and tell the UserManager to add the new user.
        /// Also sets the Roles of the user based on the AuthorizationGroup.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<IdentityResult> AddUser(UserViewModel model);

        /// <summary>
        /// Uses the UserManager to find the user with the given id
        /// and tells the manager to delete it.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<IdentityResult> DeleteUser(string id);

        /// <summary>
        /// Gets the user with the given id from the UserManager
        /// and adds the list of AuthorizationGroups from the IAuthorizationGroupsRepository
        /// to the ViewModel.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserViewModel> GetUserViewModel(string id);

        /// <summary>
        /// Creates a UserViewModel where the user Data is empty
        /// but the List of AuthorizationGroups is acquired from the
        /// IAuthorizationGroupsRepository.
        /// </summary>
        /// <returns></returns>
        UserViewModel GetUserViewModel();


        /// <summary>
        /// Validates the Data.
        /// Gets the AppUser from the UserManager and updates the info. Sets the roles 
        /// and finally tells the UserManager to perform the update on the user.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userValidator"></param>
        /// <param name="passwordHasher"></param>
        /// <param name="passwordValidator"></param>
        /// <returns></returns>
        Task<IdentityResult> EditUser(UserViewModel model, IUserValidator<AppUser> userValidator, IPasswordHasher<AppUser> passwordHasher, IPasswordValidator<AppUser> passwordValidator);

        #endregion

        #region Authorization groups
        /// <summary>
        /// Gets the list of all the names from the AuthorizationGroups
        /// provided by the IAuthorizationGroupsRepository.
        /// </summary>
        /// <returns></returns>
        List<string> AuthorizationGroups();

        /// <summary>
        /// Creates the new Viewmodel and fills it with all groups accquired
        /// from the IAuthorizationGroupsRepository.
        /// </summary>
        /// <returns></returns>
        AuthorizationGroupsViewModel GetAuthorizationGroupsViewModel();

        /// <summary>
        /// Gets the AuthorizationGroup from the model and tells
        /// the AuthorizationGroupsRepository to add the group.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AddAuthorizationGroup(GroupViewModel model);

        /// <summary>
        /// Tells the AuthorizationGroupsRepository to apply the new values
        /// and updates the roles for all users of the group.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task EditAuthorizationGroup(GroupViewModel model);

        /// <summary>
        /// Tells the AuthorizationGroupsRepository to remve the group.
        /// Also sets the AuthorizationGroup of all the users of the group to "" and updates
        /// the roles of the users. This will lead to all users of that group having no roles at all.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAuthorizationGroup(int id);

        /// <summary>
        /// Gets the group with the given id from the IAuthorizationGroupsRepository
        /// and creates the ViewModel with it.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GroupViewModel GetGroupViewModel(int id);

        #endregion

        #region Active Directory
        /// <summary>
        /// Gets and returns the ADSettings from the IADSettingsRepository
        /// </summary>
        /// <returns></returns>
        ADSettings GetADSettings();

        /// <summary>
        /// Tells the IADSettingsRepository to set the
        /// settings to those passed along.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        Task SetADSettings(ADSettings settings);

        #endregion


        #region BarrierControlSettings
        BarrierControlSettingsViewModel GetBarrierControlSettings();

        void SetBarrierControlSettings(BarrierControlSettingsViewModel settings);
        #endregion

        #region SMSSettings
        SMSSettings GetSMSSettings();

        Task SetSMSSettings(SMSSettings settings);
        #endregion
    }
}
