using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic;
using MVC.BusinessLogic.Interfaces;
using MVC.Repositories.Interfaces;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVC.Controllers.SignalR
{
    public class HistoryHub : Hub
    {
        private readonly ILogger<ProcessingHub> _logger;
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public HistoryHub(ILogger<ProcessingHub> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Sets the time of release of the registration with the given id 
        /// to the current server time.
        /// </summary>
        /// <remarks>Requires the role "CanSetRelease"</remarks>
        /// <param name="id">ID of the registration</param>
        /// <returns></returns>
        public async Task ResendToERP(int id)
        {
            _logger.LogInformation("Resend to ERP for registration with id: " + id);

            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var _historyHubFacade = scope.ServiceProvider.GetRequiredService<IHistoryHubFacade>();

                    await _historyHubFacade.ResendToERP(id);

                    await Clients.All.SendAsync("UpdateSendToERP", id);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while while resending to ERP for the registration with id: " + id + ". Message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                    await Clients.Caller.SendAsync("Error", "Das erneute Senden ans ERP konnte nicht durchgeführt werden. Fehler: " + e.Message);
                }
            }
        }
    }
}
