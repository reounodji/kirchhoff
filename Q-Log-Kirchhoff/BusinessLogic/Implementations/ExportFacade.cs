using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Data.DBContext;
using MVC.Data.Entities;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVC.BusinessLogic.Implementations
{
    public class ExportFacade : IExportFacade
    {
        private readonly ILogger<ExportFacade> _logger;

        private readonly IServiceProvider _serviceProvider;


        public ExportFacade(ILogger<ExportFacade> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public string GenerateRegistrationHistoryCSV(List<ClosedRegistration> closesRegistrations)
        {
            _logger.LogInformation("Generating registration history csv");
            using (var scope = _serviceProvider.CreateScope())
            {
                var _closedRegistrationRepository = scope.ServiceProvider.GetRequiredService<IClosedRegistrationsRepository>();

                try
                {
                    //var history = (from r in _closedRegistrationRepository.GetAll()
                    //               orderby r.TimeOfRegistration descending
                    //               select r).ToList();
                    var csv = "";
                    //\"Name\";
                    var keys = "\"Kennzeichen\";\"Name\";\"Absender\";\"Kunde\";\"Ziele\";\"Tor\";\"Anmeldung\";\"Aufruf\";\"Anzeige\";\"Abschluss\"";

                     

                     csv += keys;
                    var newDateTime = new DateTime();

                    foreach (var regist in closesRegistrations)
                    {
                        //UnprotectData(regist);

                        var row = "\n";

                        row += "\"" + regist.LicensePlate + "\"" + ";";
                        row += "\"" + regist.FirstName+" "+regist.Lastname + "\"" + ";";
                        row += "\"" + regist.Forwarder + "\"" + ";";
                        row += "\"" + regist.Customer + "\"" + ";";


                        string ziel = "";
                        if ((regist.LoadCustomerPickup || regist.LoadEmptiesCollection))
                        {
                            ziel = "1";

                        }
                        else
                        {
                            ziel = "0";
                        }

                        if (regist.GoodsReceiptdelivery)
                        {
                            ziel += "1";
                        }
                        else
                        {
                            ziel += "0";
                        }


                        if (regist.GoodsReceiptCustomerEmpties)
                        {
                            ziel += "1";
                        }
                        else
                        {
                            ziel += "0";
                        }



                        row += "\"" + ziel + "\"" + ";";
                        row += "\"" + regist.Gate + "\"" + ";";
                        row += "\"" + regist.TimeOfRegistration + "\"" + ";";
                        row += "\"" + regist.TimeOfCall.ToShortTimeString()+ "\"" + ";";
                        row += "\"" + regist.TimeOfCall.ToShortTimeString() + "\"" + ";";
                        row += "\"" + regist.TimeOfExit.ToShortTimeString() + "\"" + "";

                        
                        csv += row;
                    }
                    return csv;
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while generating registration history csv. Message: " + e.Message);
                    throw;
                }
            }
        }

        public string GenerateUnknownForwardingAgenciesCSVData()
        {
            _logger.LogInformation("Generating unknown forwarding agencies csv data");
            using (var scope = _serviceProvider.CreateScope())
            {
                var _unknownForwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IUnknownForwardingAgenciesRepository>();
                try
                {
                    var agencies = _unknownForwardingAgenciesRepository.GetAll();

                    var csvData = "Name;FirstAppereance;NumberOfAppereances";
                    foreach (var a in agencies)
                    {
                        csvData += "\n";
                        csvData += "\"" + a.Name + "\"";
                        csvData += ";";
                        csvData += "\"" + a.FirstAppereance.ToString() + "\"";
                        csvData += ";";
                        csvData += "\"" + a.NumberOfAppereances.ToString() + "\"";
                    }
                    return csvData;
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while trying to generate csv data. Message: " + e.Message);
                    throw new Exception("Es ist ein Fehler bei der Erzeugung der csv Daten aufgetreten. Fehler: " + e.Message);
                }
            }
        }

        public string GenerateUnknownForwardingAgenciesXMLData()
        {
            _logger.LogInformation("Generating unknown forwarding agencies xml data");
            using (var scope = _serviceProvider.CreateScope())
            {
                var _unknownForwardingAgenciesRepository = scope.ServiceProvider.GetRequiredService<IUnknownForwardingAgenciesRepository>();
                try
                {
                    var agencies = _unknownForwardingAgenciesRepository.GetAll();

                    var xmlData = "<UnknownForwardingAgencies>";

                    foreach (var a in agencies)
                    {
                        xmlData += "\n";
                        xmlData += "<UnknownForwardingAgency>";
                        xmlData += "\n<Name>" + a.Name + "</Name>";
                        xmlData += "\n<FirstAppereance>" + a.FirstAppereance + "</FirstAppereance>";
                        xmlData += "\n<NumberOfAppereances>" + a.NumberOfAppereances + "</NumberOfAppereances>";
                        xmlData += "\n</UnknownForwardingAgency>";
                    }

                    xmlData += "\n</UnknownForwardingAgencies>";
                    return xmlData;
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while trying to generate xml data. Message: " + e.Message);
                    throw new Exception("Es ist ein Fehler bei der Erzeugung der xml Daten aufgetreten. Fehler: " + e.Message);
                }
            }
        }

        public string GenerateUnknownSuppliersCSVData()
        {
            _logger.LogInformation("Generating unknown suppliers csv data");
            using (var scope = _serviceProvider.CreateScope())
            {
                var unknownSuppliersRepository = scope.ServiceProvider.GetRequiredService<IUnknownSupplierRepository>();
                try
                {
                    var suppliers = unknownSuppliersRepository.GetAll();

                    var csvData = "Name;FirstAppereance;NumberOfAppereances";
                    foreach (var a in suppliers)
                    {
                        csvData += "\n";
                        csvData += "\"" + a.Name + "\"";
                        csvData += ";";
                        csvData += "\"" + a.FirstAppereance.ToString() + "\"";
                        csvData += ";";
                        csvData += "\"" + a.NumberOfAppereances.ToString() + "\"";
                    }
                    return csvData;
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while trying to generate csv data. Message: " + e.Message);
                    throw new Exception("Es ist ein Fehler bei der Erzeugung der csv Daten aufgetreten. Fehler: " + e.Message);
                }
            }
        }

        public string GenerateUnknownParcelServicesCSVData()
        {
            _logger.LogInformation("Generating unknown ParcelServices csv data");
            using (var scope = _serviceProvider.CreateScope())
            {
                var unknownParcelServicesRepository = scope.ServiceProvider.GetRequiredService<IUnknownParcelServiceRepository>();
                try
                {
                    var ParcelServices = unknownParcelServicesRepository.GetAll();

                    var csvData = "Name;FirstAppereance;NumberOfAppereances";
                    foreach (var a in ParcelServices)
                    {
                        csvData += "\n";
                        csvData += "\"" + a.Name + "\"";
                        csvData += ";";
                        csvData += "\"" + a.FirstAppereance.ToString() + "\"";
                        csvData += ";";
                        csvData += "\"" + a.NumberOfAppereances.ToString() + "\"";
                    }
                    return csvData;
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while trying to generate csv data. Message: " + e.Message);
                    throw new Exception("Es ist ein Fehler bei der Erzeugung der csv Daten aufgetreten. Fehler: " + e.Message);
                }
            }
        }

        public string GenerateUnknownFittersCSVData()
        {
            _logger.LogInformation("Generating unknown Fitters csv data");
            using (var scope = _serviceProvider.CreateScope())
            {
                var unknownFittersRepository = scope.ServiceProvider.GetRequiredService<IUnknownFitterRepository>();
                try
                {
                    var Fitters = unknownFittersRepository.GetAll();

                    var csvData = "Name;FirstAppereance;NumberOfAppereances";
                    foreach (var a in Fitters)
                    {
                        csvData += "\n";
                        csvData += "\"" + a.Name + "\"";
                        csvData += ";";
                        csvData += "\"" + a.FirstAppereance.ToString() + "\"";
                        csvData += ";";
                        csvData += "\"" + a.NumberOfAppereances.ToString() + "\"";
                    }
                    return csvData;
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while trying to generate csv data. Message: " + e.Message);
                    throw new Exception("Es ist ein Fehler bei der Erzeugung der csv Daten aufgetreten. Fehler: " + e.Message);
                }
            }
        }
    }
}
