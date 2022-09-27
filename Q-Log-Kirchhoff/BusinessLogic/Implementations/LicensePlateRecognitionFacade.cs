using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Controllers.SignalR;
using MVC.Data.Entities;
using MVC.Models.APIDtos;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Implementations
{
    public class LicensePlateRecognitionFacade: HttpClient, ILicensePlateRecognitionFacade
    {
        private readonly ILogger<LicensePlateRecognitionFacade> _logger;
        private readonly IServiceProvider _serviceProvider;


        public LicensePlateRecognitionFacade(ILogger<LicensePlateRecognitionFacade> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public async Task DetectedEntry(LicensePlateRecognitionDto dto)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var openRegistrationRepo = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                var barrierSettingsRepo = scope.ServiceProvider.GetRequiredService<IBarrierControlSettingsRepository>();

                var processingHub = scope.ServiceProvider.GetRequiredService<IHubContext<ProcessingHub>>();

                var barrierSettings = barrierSettingsRepo.Get();

                var compressedLicensePlate = Utility.CompressLicensePlate(dto.License);
                var regist = openRegistrationRepo.GetWithCompressedLicensePlate(compressedLicensePlate);
                if(regist == null)
                {
                    await processingHub.Clients.All.SendAsync("EntryLicenseUnknown", compressedLicensePlate ,DateTime.Now);
                }
                else
                {
                    if(regist.TimeOfEntry != new DateTime())
                    {
                        _logger.LogWarning("The licenseplate: " + compressedLicensePlate + " was recognized by the camera system but that registration has already entered. Will display warning to the employees.");
                        await processingHub.Clients.All.SendAsync("LicenseAlreadyEntered", compressedLicensePlate, DateTime.Now);
                        return;
                    }
                    var curTime = DateTime.Now;
                    await openRegistrationRepo.SetEntry(regist.ID, curTime);
                    await processingHub.Clients.All.SendAsync("SetEntry", regist.ID, curTime);

                    if(barrierSettings.UseBarrierControl)
                    {
                        if(!String.IsNullOrEmpty( barrierSettings.EntryBarrierAPIUrl) )
                        {
                            try
                            {
                                var result = PostAsync(barrierSettings.EntryBarrierAPIUrl,null).Result;
                            }
                            catch(Exception e)
                            {
                                _logger.LogWarning("Could not open entry barrier. Message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                            }
                        }
                    }
                }
            }
        }

        public async Task DetectedExit(LicensePlateRecognitionDto dto)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var openRegistrationRepo = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                var closedRegistrationRepo = scope.ServiceProvider.GetRequiredService<IClosedRegistrationsRepository>();
                var barrierSettingsRepo = scope.ServiceProvider.GetRequiredService<IBarrierControlSettingsRepository>();

                var processingHub = scope.ServiceProvider.GetRequiredService<IHubContext<ProcessingHub>>();

                var barrierSettings = barrierSettingsRepo.Get();

                var compressedLicensePlate = Utility.CompressLicensePlate(dto.License);
                var regist = openRegistrationRepo.GetWithCompressedLicensePlate(compressedLicensePlate);
                if (regist == null)
                {
                    await processingHub.Clients.All.SendAsync("ExitLicenseUnknown", compressedLicensePlate, DateTime.Now);
                }
                else
                {
                    if (regist.TimeOfExit != new DateTime())
                    {
                        _logger.LogWarning("The licenseplate: " + compressedLicensePlate + " was recognized by the camera system but that registration has already left. Will display warning to the employees.");
                        await processingHub.Clients.All.SendAsync("LicenseAlreadyExited", compressedLicensePlate, DateTime.Now);
                        return;
                    }
                    var curTime = DateTime.Now;

                    await openRegistrationRepo.Remove(regist);
                    regist.TimeOfExit = curTime;

                    var closed = new ClosedRegistration(regist);
                    await closedRegistrationRepo.Add(closed);

                    await processingHub.Clients.All.SendAsync("SetExit", regist.ID, curTime);

                    if (barrierSettings.UseBarrierControl)
                    {
                        if (!String.IsNullOrEmpty(barrierSettings.ExitBarrierAPIUrl))
                        {
                            try
                            {
                                var result = PostAsync(barrierSettings.ExitBarrierAPIUrl, null).Result;
                            }
                            catch (Exception e)
                            {
                                _logger.LogWarning("Could not open exit barrier. Message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                            }
                        }
                    }
                }
            }
        }
    }
}
