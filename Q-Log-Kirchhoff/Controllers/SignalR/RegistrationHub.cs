using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using System;
using System.Threading.Tasks;

namespace MVC.Controllers.SignalR
{
    /// <summary>
    /// Handles the dynamic suggestion of ForwardingAgencies using SignalR
    /// </summary>
    public class RegistrationHub : Hub
    {
        private readonly ILogger<RegistrationHub> _logger;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public RegistrationHub(ILogger<RegistrationHub> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Sends the list of forwarding agencies of given name, 
        /// to the client that called this method
        /// </summary>
        /// <param name="postalCode">Postalcode of the forwarding agency</param>
        /// <param name="input">Name of the forwarding agency</param>
        /// <returns></returns>
        public async Task GetForwardingAgencies(string input)
        {
           // _logger.LogInformation("Returning list of forwarding agencies that include the postalCode: " + postalCode + " and the name: " + input + " to the client, that is currently in a registration process.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IRegistrationHubFacade>();
                    var companyNameList = facade.GetForwardingAgencies(input.ToUpper());
                    await Clients.Caller.SendAsync("SetSuggestions", companyNameList);
                }          
            }
            catch (Exception e)
            {
                _logger.LogError("Error while getting the list of forwarding agencies, name: " + input + " . Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message );
                await Clients.Caller.SendAsync("Error", "Speditionen konnten nicht geladen werden");
            }
        }


        /// <summary>
        /// Returns a list of suppliers, that include the given name, 
        /// to the client that called this method
        /// </summary>
        /// <param name="input">Name of the supplier</param>
        /// <returns></returns>
        public async Task GetSuppliers(string input)
        {
           // _logger.LogInformation("Returning list of suppliers that include the name: " + input + " to the client, that is currently in a registration process.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IRegistrationHubFacade>();
                    var companyNameList = facade.GetSuppliers(input.ToUpper());
                    await Clients.Caller.SendAsync("SetSupplierSuggestions", companyNameList);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while getting the list of suppliers, that include the name: " + input +". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                await Clients.Caller.SendAsync("Error", "Lieferanten konnten nicht geladen werden");
            }
        }

        /// <summary>
        /// Returns a list of ParcelServices, that include the given name, 
        /// to the client that called this method
        /// </summary>
        /// <param name="input">Name of the ParcelService</param>
        /// <returns></returns>
        public async Task GetParcelServices(string input)
        {
            // _logger.LogInformation("Returning list of ParcelServices that include the name: " + input + " to the client, that is currently in a registration process.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IRegistrationHubFacade>();
                    var companyNameList = facade.GetParcelServices(input.ToUpper());
                    await Clients.Caller.SendAsync("SetSuggestions", companyNameList);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while getting the list of ParcelServices, that include the name: " + input + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                await Clients.Caller.SendAsync("Error", "Lieferanten konnten nicht geladen werden");
            }
        }

        /// <summary>
        /// Returns a list of Fitters, that include the given name, 
        /// to the client that called this method
        /// </summary>
        /// <param name="input">Name of the Fitter</param>
        /// <returns></returns>
        public async Task GetFitters(string input)
        {
            // _logger.LogInformation("Returning list of Fitters that include the name: " + input + " to the client, that is currently in a registration process.");
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var facade = scope.ServiceProvider.GetRequiredService<IRegistrationHubFacade>();
                    var companyNameList = facade.GetFitters(input.ToUpper());
                    await Clients.Caller.SendAsync("SetSuggestions", companyNameList);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while getting the list of Fitters, that include the name: " + input + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                await Clients.Caller.SendAsync("Error", "Lieferanten konnten nicht geladen werden");
            }
        }
    }
}
