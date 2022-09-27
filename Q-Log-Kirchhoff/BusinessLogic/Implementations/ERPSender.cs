using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Data.Entities;
using MVC.Models.APIDtos;
using MVC.Repositories.Interfaces;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading;
using System.Globalization;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;

namespace MVC.BusinessLogic.Implementations
{
    public class ERPSender : IERPSender
    {
        private readonly ILogger<ProcessingHubFacade> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly string authentifikation = "Schauf-ws:*e&p8f=9SDRt5%/";

        public ERPSender(ILogger<ProcessingHubFacade> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }


        public async Task<bool> SendRegistrationToERP(Registration registration, string loadingStation = null)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    if (_configuration["ERPSenderDestinationUrl"].ToUpper() == "Disabled".ToUpper())
                    {
                        _logger.LogInformation("API disabled did not do 'lkw_anmelden'");
                        return false;
                    }
                    _logger.LogInformation("Start sending Registration to ERP");

                    var regist = GetERPRegistrationDTO(registration, loadingStation);

                    var json = JsonConvert.SerializeObject(regist);

                    HttpClient client = new HttpClient();

                    var httpContent = new StringContent(json, Encoding.UTF8, "application/json");


                    using (var httpClient = new HttpClient())
                    {
                        var byteArray = Encoding.ASCII.GetBytes(authentifikation);
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                        var httpResponse = await httpClient.PostAsync(_configuration["ERPSenderDestinationUrl"] + "lkw_anmelden/", httpContent);

                        if (httpResponse.Content == null)
                            return false;

                        if (httpResponse.StatusCode != System.Net.HttpStatusCode.NoContent)
                        {
                            var responseContent = await httpResponse.Content.ReadAsStringAsync();

                            _logger.LogError(string.Format("ERP-System 'lkw_anmelden/' responsecode: {0}. Error: {1}", httpResponse.StatusCode, responseContent));

                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception occured while 'lkw_anmelden/' {0} to ERP. Exception {1}", registration.ID, ex.Message));
                throw ex;
            }
        }

        private ERPRegistrationDTO GetERPRegistrationDTO(Registration regist, string LoadingStation = null)
        {
            ERPRegistrationDTO registrationDTO = new ERPRegistrationDTO()
            {
                kennzeichen = regist.LicensePlate,
                anz_personen = regist.NumberOfPeople,
                zpt_anmeldung = ConvertTime(regist.TimeOfRegistration),
                zpt_freigabe = ConvertTime(regist.TimeOfRelease),
                ladestelle = LoadingStation ?? regist.LoadingStation,
                anliefer_tor = regist.Gate
            };

            switch (regist.ApproachTyp)
            {
                case Data.Enums.EApproachTyp.ForwardingAgency:
                    registrationDTO.lade_typ = "spedition";
                    registrationDTO.name_firma = regist.CompanyName;
                    registrationDTO.lade_referenz = regist.LoadReference;
                    break;
                case Data.Enums.EApproachTyp.Supplier:
                    registrationDTO.lade_typ = "lieferanten";
                    registrationDTO.lade_referenz = regist.LoadReference;
                    registrationDTO.lieferanten = regist.SupplierNumber;
                    registrationDTO.name_firma = regist.CompanyName;
                    break;
                case Data.Enums.EApproachTyp.ParcelService:
                    registrationDTO.name_firma = regist.CompanyName;
                    registrationDTO.lade_typ = "zusteller";
                    break;
                case Data.Enums.EApproachTyp.Fitter:
                    registrationDTO.lade_typ = "monteure";
                    registrationDTO.name_firma = regist.CompanyName;
                    registrationDTO.name_ansprechpartner = regist.LoadReference;
                    break;
            }

            if (regist.IsSmallVehicle)
                registrationDTO.gewichtsklasse = "< 7,5t";
            else
                registrationDTO.gewichtsklasse = "> 7,5t";

            if (string.IsNullOrWhiteSpace(registrationDTO.name_firma))
                registrationDTO.name_firma = "null";
            if (string.IsNullOrWhiteSpace(registrationDTO.lieferanten))
                registrationDTO.lieferanten = "null";
            if (string.IsNullOrWhiteSpace(registrationDTO.lade_referenz))
                registrationDTO.lade_referenz = "null";
            if (string.IsNullOrWhiteSpace(registrationDTO.name_ansprechpartner))
                registrationDTO.name_ansprechpartner = "null";

            return registrationDTO;
        }

        private string ConvertTime(DateTime time)
        {
            //2019-12-13T13:30:28Z
            return string.Format("{0}-{1}-{2}T{3}:{4}:{5}Z", time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
        }


        public async Task<bool> GetSuppliersFromERP()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    if (_configuration["ERPSenderDestinationUrl"].ToUpper() == "Disabled".ToUpper())
                    {
                        _logger.LogInformation("Dont use APIs, because sending is disabled");
                        return false;
                    }
                    _logger.LogInformation("Start sending to ERP");

                    using (var httpClient = new HttpClient())
                    {

                        var byteArray = Encoding.ASCII.GetBytes(authentifikation);
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

                        var httpResponse = await httpClient.GetAsync(_configuration["ERPSenderDestinationUrl"] + "lieferanten");

                        if (httpResponse.Content == null)
                            return false;

                        if (httpResponse.StatusCode != System.Net.HttpStatusCode.OK)
                        {
                            var responseContent = await httpResponse.Content.ReadAsStringAsync();

                            _logger.LogError(string.Format("ERP-System get 'lieferanten' responsecode: {0}. Error: {1}", httpResponse.StatusCode, responseContent));

                            return false;
                        }
                        else
                        {
                            var supplierRepo = scope.ServiceProvider.GetRequiredService<ISupplierRepository>();

                            var apiSupplierNumbers = convertHttpResponseToSuppliernumbers(httpResponse);

                            return await supplierRepo.fullUpdateSupplier(apiSupplierNumbers);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception occured while get 'lieferanten'. Exception {0}", ex.Message));
                throw ex;
            }
        }

        private List<SupplierNumber> convertHttpResponseToSuppliernumbers(HttpResponseMessage response)
        {
            try
            {
                _logger.LogDebug(response.Content.ToString());

                var content = response.Content.ReadAsStringAsync();

                List<SupplierNumber> newSup = JsonConvert.DeserializeObject<List<SupplierNumber>>(content.Result);

                return newSup;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("Exception occured while converting HTTPresponse to SupplierNumber. Exception {0}", ex.Message));
                throw ex;
            }
        }
    }
}
