using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Data.Entities;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MVC.HostedServices
{
    public class DisplayService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly ILogger _logger;
        private Timer _timer;

        private List<List<OpenRegistration>> previousOpenRegistrations = new List<List<OpenRegistration>>();

        public DisplayService(IServiceProvider serviceProvider, ILogger<DisplayService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
     

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Display Service is starting.");
            using (var scope = _serviceProvider.CreateScope())
            {
                var _generalSettingsRepository = scope.ServiceProvider.GetRequiredService<IGeneralSettingsRepository>();
                var settings = _generalSettingsRepository.GetGeneralSettings();
                var displayUpdateInterval = 10;
                if (settings != null)
                {
                    displayUpdateInterval = settings.DisplayUpdateInterval;
                }

                _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(displayUpdateInterval));

                return Task.CompletedTask;
            }
        }

        private void DoWork(object state)
        {
            //if you want to debug the display
            //_timer.Change(Timeout.Infinite, Timeout.Infinite);

            using (var scope = _serviceProvider.CreateScope())
            {
                var displayFacade = scope.ServiceProvider.GetRequiredService<IDisplayFacade>();
                previousOpenRegistrations = displayFacade.Update(previousOpenRegistrations);
            }

            //if you wnat to debug the display
            //_timer.Change(TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Display Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}
