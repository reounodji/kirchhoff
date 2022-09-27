using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Controllers.SignalR;
using MVC.Data.Entities;
using MVC.Models;
using MVC.Repositories.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Implementations
{
    public class ProcessingFacade : IProcessingFacade
    {
        private readonly ILogger<ProcessingFacade> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDataProtectionProvider _dataProtectionProvider;


        public ProcessingFacade(ILogger<ProcessingFacade> logger, IServiceProvider serviceProvider, IDataProtectionProvider dataProtectionProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _dataProtectionProvider = dataProtectionProvider;
        }

        public ProcessingViewModel GetProcessingViewModel()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _generalSettingsRepository = scope.ServiceProvider.GetRequiredService<IGeneralSettingsRepository>();
                var _gatesRepository = scope.ServiceProvider.GetRequiredService<IGatesRepository>();
                var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                var _forwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IForwardingAgenciesRepository>();
                var _loadingStationsRepository = scope.ServiceProvider.GetRequiredService<ILoadingStationsRepository>();

                var _firstNameProtector = _dataProtectionProvider.CreateProtector("FirstNameProtector");
                var _surNameProtector = _dataProtectionProvider.CreateProtector("SurNameProtector");

                var forwardingAgencies = _forwardingAgenciesRepository.GetAll();
                var gates = _gatesRepository.GetAll();
                var regists = _openRegistrationsRepository.GetAll();
                var generalSettings = _generalSettingsRepository.GetGeneralSettings();
                var loadingstations = _loadingStationsRepository.GetAll();
                var model = new ProcessingViewModel()
                {
                    Gates = gates,
                    Registrations = regists,
                    maxWaitingTime = generalSettings.RegistrationTimeThreshold,
                    LoadingStations = loadingstations,
                    ExceededWaitTimeColorCode = generalSettings.ExceededWaitTimeColorCode,
                    HoverColorCode = generalSettings.HoverColorCode,
                    ExitColorCode = generalSettings.ExitColorCode,
                    NewEntryColorCode = generalSettings.NewEntryColorCode,
                    RecentChangeColorCode = generalSettings.RecentChangeColorCode
                };
                return model;
            }
        }
    }
}
