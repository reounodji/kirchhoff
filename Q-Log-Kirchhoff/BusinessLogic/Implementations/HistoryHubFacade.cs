using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Implementations
{
    public class HistoryHubFacade : IHistoryHubFacade
    {
        private readonly ILogger<ProcessingHubFacade> _logger;

        private readonly IServiceProvider _serviceProvider;


        public HistoryHubFacade(ILogger<ProcessingHubFacade> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task ResendToERP(int id)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _closedRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IClosedRegistrationsRepository>();
                    var _gatesRepository = scope.ServiceProvider.GetRequiredService<IGatesRepository>();
                    var _erpSender = scope.ServiceProvider.GetRequiredService<IERPSender>();

                    var regist = _closedRegistrationsRepository.Get(id);

                    string loadingStation = null;
                    if (regist.LoadingStation == "All" || regist.LoadingStation == "Alle")
                    {
                        loadingStation = _gatesRepository.GetLoadingStationFromGate(regist.Gate);
                    }

                    bool wasSendingSuccessful = await _erpSender.SendRegistrationToERP(regist, loadingStation);
                    await _closedRegistrationsRepository.SetWasSendingSuccessful(regist, wasSendingSuccessful);
                }
            }
            catch (Exception)
            {
                // throw the exception further, so that the user will see the message
                throw;
            }
        }
    }
}

