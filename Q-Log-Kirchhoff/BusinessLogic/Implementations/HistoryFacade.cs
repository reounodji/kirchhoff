using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Data.Entities;
using MVC.Models;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;

namespace MVC.BusinessLogic.Implementations
{
    public class HistoryFacade : IHistoryFacade
    {
        private readonly ILogger<HistoryFacade> _logger;
        private readonly IServiceProvider _serviceProvider;       


        public HistoryFacade(ILogger<HistoryFacade> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public DateTime GetDefaultHistoryStart()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _generalSettingsRepository = scope.ServiceProvider.GetRequiredService<IGeneralSettingsRepository>();
                try
                {
                    var timespan = new TimeSpan(_generalSettingsRepository.GetGeneralSettings().DefaultHistoryTimespan, 0, 0, 0);
                    var start = DateTime.Now.Subtract(timespan);

                    return start;
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while trying to get default history start. message: " + e.Message + " inner: " + e.InnerException?.Message);
                    throw new Exception("Fehler beim laden des standardmäßigen Startzeitpunktes der Historie.");
                }
            }
        }

        public FileStream ExportRegistrationHistoryCSV(List<ClosedRegistration> closesRegistrations)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _exportFacade = scope.ServiceProvider.GetRequiredService<IExportFacade>();
                    Directory.CreateDirectory("DataExport");
                    StreamWriter writer = new StreamWriter("DataExport/GesamteRegistrationHistorieExport.csv");
                    var data = _exportFacade.GenerateRegistrationHistoryCSV(closesRegistrations);
                    writer.Write(data);
                    writer.Flush();
                    writer.Close();
                    return new FileStream("DataExport/GesamteRegistrationHistorieExport.csv", FileMode.Open);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not Export RegistrationHistory.csv. Error while creating Filestream");
                throw e;
            }
        }


        public HistoryViewModel GetHistoryViewModel(DateTime start, DateTime end, string vehiculeNr, string firstname, string lastname, string phonenumber, string forwarder, string customer)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _closedRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IClosedRegistrationsRepository>();
                var _protectionProvider = scope.ServiceProvider.GetRequiredService<IDataProtectionProvider>();
                var _firstNameProtector = _protectionProvider.CreateProtector("FirstNameProtector");
                var _surNameProtector = _protectionProvider.CreateProtector("SurNameProtector");
                var _phoneProtector = _protectionProvider.CreateProtector("PhoneProtector");
                var generalSettings = scope.ServiceProvider.GetRequiredService<IGeneralSettingsRepository>().GetGeneralSettings();

                var model = new HistoryViewModel();
                var registrations = _closedRegistrationsRepository.GetAll(start, end,vehiculeNr, firstname, lastname, phonenumber,  forwarder,  customer);

                model.Registrations = registrations;
                model.Start = start;
                model.End = end;
                model.HoverColorCode = generalSettings.HoverColorCode;


                return model;
            }
        }
    }
}
