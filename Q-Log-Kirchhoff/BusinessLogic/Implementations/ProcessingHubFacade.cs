using com.esendex.sdk.messaging;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Controllers.SignalR;
using MVC.Data.Entities;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Implementations
{
    public class ProcessingHubFacade : IProcessingHubFacade
    {
        private readonly ILogger<ProcessingHubFacade> _logger;

        private readonly IServiceProvider _serviceProvider;


        public ProcessingHubFacade(ILogger<ProcessingHubFacade> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }


        public async Task SetRelease(int id, DateTime curTime)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();

                    await _openRegistrationsRepository.SetRelease(id, curTime);
                }
            }
            catch (Exception)
            {
                // throw the exception further, so that the user will see the message
                throw;
            }
        }


        public async Task SetCall(int id,string gate, DateTime curTime)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                    await _openRegistrationsRepository.Call(id,gate, curTime);
                }
            }
            catch (Exception)
            {
                // throw the exception further, so that the user will see the message
                throw;
            }
        }



        public async Task SetUpdateCallStatus(int id, int status, DateTime curTime)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                    await _openRegistrationsRepository.UpdateCallStatus(id, status, curTime);
                }
            }
            catch (Exception)
            {
                // throw the exception further, so that the user will see the message
                throw;
            }
        }


        public async Task SetConfirm(int id, DateTime curTime)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                    await _openRegistrationsRepository.Confirm(id,curTime);
                }
            }
            catch (Exception)
            {
                // throw the exception further, so that the user will see the message
                throw;
            }
        }

        public async Task SetEntry(int id, DateTime curTime)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                    await _openRegistrationsRepository.SetEntry(id, curTime);
                }
            }
            catch (Exception)
            {
                // throw the exception further, so that the user will see the message
                throw;
            }
        }

        public async Task SetProcessStart(int id, DateTime curTime)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                    await _openRegistrationsRepository.SetProcessStart(id, curTime);
                }
            }
            catch (Exception)
            {
                // throw the exception further, so that the user will see the message
                throw;
            }
        }

        public async Task SetProcessEnd(int id, DateTime curTime)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                    await _openRegistrationsRepository.SetProcessEnd(id, curTime);
                }
            }
            catch (Exception)
            {
                // throw the exception further, so that the user will see the message
                throw;
            }
        }

        public async Task SetExit(int id, DateTime curTime)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                    var _closedRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IClosedRegistrationsRepository>();

                    var regist = _openRegistrationsRepository.Get(id);


                    if (regist.LoadingStation == "All" || regist.LoadingStation == "Alle")
                    {
                        _openRegistrationsRepository.RemoveAllLoadingStation(regist.ID);
                    }

                    await _openRegistrationsRepository.Remove(regist);
                    regist.TimeOfExit = curTime;
                    regist.ProcessEnd = curTime;
                    regist.StatusCall = 8;
                    var closed = new ClosedRegistration(regist);

                    await _closedRegistrationsRepository.Add(closed);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SetGate(int id, string value)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                    var smsSettings = scope.ServiceProvider.GetRequiredService<ISMSSettingsRepository>().Get();

                    await _openRegistrationsRepository.SetGate(id, value);
}
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SetLoadingStation(int id, string value)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();

                    await _openRegistrationsRepository.SetLoadingStation(id, value);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task ResetGate(int id)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var openRegistrationRepos = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                await openRegistrationRepos.ResetGate(id);
            }
        }

        public List<Gate> GetAllGates()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var gatesRepos = scope.ServiceProvider.GetRequiredService<IGatesRepository>();
                return gatesRepos.GetAll();
            }
        }

        public string GetGatesAsString(string selectedLoadingStation)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _gatesRepository = scope.ServiceProvider.GetRequiredService<IGatesRepository>();
                var _loadingStationsRepository = scope.ServiceProvider.GetRequiredService<ILoadingStationsRepository>();

                LoadingStation loadingStation = _loadingStationsRepository.Get(selectedLoadingStation);
                List<Gate> gates = _gatesRepository.GetAllGatesFromLoadingStation(loadingStation);

                string gatesString = string.Empty;

                for (int i = 0; i < gates.Count; i++)
                {
                    gatesString += gates[i].Name;

                    if (i < gates.Count - 1)
                        gatesString += ",";
                }

                return gatesString;
            }
        }

        public string GetLoadingstationsAsString()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _loadingStationsRepository = scope.ServiceProvider.GetRequiredService<ILoadingStationsRepository>();

                List<LoadingStation> loadingStations = _loadingStationsRepository.GetAll();

                string loadingstations = string.Empty;

                for (int i = 0; i < loadingStations.Count; i++)
                {
                    loadingstations += loadingStations[i].Name;

                    if (i < loadingStations.Count - 1)
                        loadingstations += ",";
                }

                return loadingstations;
            }
        }

        public async Task<string> UpdateCompanyName(int ID, string CompanyName)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _companyFacade = scope.ServiceProvider.GetRequiredService<ICompanyFacade>();

                    return await _companyFacade.ChangeCompany(ID, CompanyName);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateComment(int ID, string Comment)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();

                    await _openRegistrationsRepository.SetComment(ID, Comment);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateLoadReference(int ID, string LoadReference)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();

                    await _openRegistrationsRepository.SetLoadReference(ID, LoadReference);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendOpenRegistrationToERP(int id)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _openRegistrationsRepository = scope.ServiceProvider.GetRequiredService<IOpenRegistrationsRepository>();
                    var _gatesRepository = scope.ServiceProvider.GetRequiredService<IGatesRepository>();
                    var _erpSender = scope.ServiceProvider.GetRequiredService<IERPSender>();

                    var regist = _openRegistrationsRepository.Get(id);

                    string loadingStation = null;
                    if (regist.LoadingStation == "All" || regist.LoadingStation == "Alle")
                    {
                        loadingStation = _gatesRepository.GetLoadingStationFromGate(regist.Gate);
                    }

                    bool wasSendingSuccessful = await _erpSender.SendRegistrationToERP(regist, loadingStation);
                    await _openRegistrationsRepository.SetWasSendingSuccessful(id, wasSendingSuccessful);
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
