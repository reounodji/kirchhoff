using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Data.Entities;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MVC.HostedServices
{
    public class SupplierService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        private readonly ILogger _logger;
        private Timer _timer;

        private int retries = 0;

        public SupplierService(IServiceProvider serviceProvider, ILogger<SupplierService> logger, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;
        }


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Supplier Service is starting.");
            using (var scope = _serviceProvider.CreateScope())
            {
                SetTimerWithTimeFromAppSettings();

                return Task.CompletedTask;
            }
        }

        private  void SetTimerWithTimeFromAppSettings()
        {
            //DateTime givenTime = DateTime.ParseExact(_configuration["SupplierUpdateTime"], "HH:mm:ss", CultureInfo.InvariantCulture);
            TimeSpan givenTime;
            try
            {
                givenTime = TimeSpan.Parse(_configuration["SupplierUpdateTime"]);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Could not convert given Time from appsettings. Set Time to 01:00 Hour Exception: {0}", ex.Message));
                givenTime = new TimeSpan(1, 0, 0);

            }

            DateTime now = DateTime.Now;
            DateTime TimerTarget = DateTime.Today.AddHours(givenTime.Hours).AddMinutes(givenTime.Minutes).AddSeconds(givenTime.Seconds);

            if (now > TimerTarget)
            {
                TimerTarget = TimerTarget.AddDays(1.0);
            }

            int msUntilTimerTarget = (int)((TimerTarget - now).TotalMilliseconds);

            _timer = new Timer(DoWork, null, msUntilTimerTarget, Timeout.Infinite);
        }

        private async void DoWork(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _ERPSender = scope.ServiceProvider.GetRequiredService<IERPSender>();


                bool success = await _ERPSender.GetSuppliersFromERP();

                if (success || retries >= 3)
                {
                    SetTimerWithTimeFromAppSettings();
                    retries = 0;
                }
                else
                {
                    _timer = new Timer(DoWork, null, 1200000, Timeout.Infinite);
                    retries++;
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Supplier Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

    }
}
