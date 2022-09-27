using Microsoft.Extensions.Logging;
using MVC.Data.DBContext;
using MVC.Data.Entities;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Repositories.Implementations
{
    public class EFOpenRegistrationsRepository : IOpenRegistrationsRepository
    {
        private readonly ILogger<EFOpenRegistrationsRepository> _logger;

        private readonly ApplicationDBContext _context;


        public EFOpenRegistrationsRepository(ILogger<EFOpenRegistrationsRepository> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public string GetLanguageCode(int id)
        {
            return _context.Set<OpenRegistration>().Where(r => r.ID == id).Select(r => r.Language).FirstOrDefault();
        }


        public OpenRegistration GetWithCompressedLicensePlate(string licensePlate)
        {
            return _context.Set<OpenRegistration>().Where(r => r.CompressedLicensePlate == licensePlate).Select(r => r).FirstOrDefault();
        }

        public async Task<int> Add(Registration registration)
        {
            _logger.LogInformation("Adding registration to DB");
            try
            {
                registration.LicensePlate = registration.LicensePlate.ToUpper();
                await _context.OpenRegistrations.AddAsync((OpenRegistration)registration);
                await _context.SaveChangesAsync();
                return registration.ID;
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to add OpenRegistration to DB. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public List<OpenRegistration> GetAll()
        {
            try
            {
                return _context.OpenRegistrations.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get all OpenRegistrations. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task SetWasSendingSuccessful(int id, bool wasSendingSuccessful)
        {
            _logger.LogInformation("Setting wasSendingSuccessful in DB for id: " + id + " to: " + wasSendingSuccessful.ToString());
            try
            {
                var regist = (from o in _context.OpenRegistrations
                              where o.ID == id
                              select o).FirstOrDefault();

                if (regist == null)
                {
                    throw new Exception(string.Format("Bei dem Eintrag konnte nicht gesetzt werden, ob erfolgreich an die ERP schnittstelle gesendet wurde. Der Eintrag wurde nicht in der DB gefunden."));
                }
                regist.WasSendingSuccessful = wasSendingSuccessful;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set wasSendingSuccessful in DB. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("WasSendingSuccessful could not be set. ");
            }
        }

        public async Task SetRelease(int id, DateTime curTime)
        {
            _logger.LogInformation("Setting release in DB for id: " + id + " to time: " + curTime.ToString());
            try
            {
                var regist = (from o in _context.OpenRegistrations
                              where o.ID == id
                              select o).FirstOrDefault();
                if (regist == null)
                {
                    throw new Exception("Aufruf konnte nicht ausgeführt werden. Der Eintrag wurde nicht in der DB gefunden.");
                }
                regist.TimeOfRelease = curTime;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set release in DB. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Freigabe konnte nicht ausgeführt werden. ");
            }
        }

        public async Task Call(int id, string gate, DateTime curTime)
        {
            _logger.LogInformation("Setting call in DB for id: " + id + " to time: " + curTime.ToString());
            try
            {
                var regist = (from o in _context.OpenRegistrations
                              where o.ID == id
                              select o).FirstOrDefault();
                if (regist == null)
                {
                    throw new Exception("Aufruf konnte nicht ausgeführt werden. Der Eintrag wurde nicht in der DB gefunden.");
                }
                if (!String.IsNullOrWhiteSpace(gate))
                {
                    regist.Gate = gate;
                }

                regist.TimeOfCall = curTime;
                if (regist.StatusCall > 0)
                {
                    regist.StatusCall += 1;
                }
                else
                {
                    regist.StatusCall = 1;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set call in DB. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Aufruf konnte nicht ausgeführt werden. ");
            }
        }

        public async Task UpdateCallStatus(int id, int status, DateTime curTime)
        {
            _logger.LogInformation("Confirm the call in DB for id: " + id + " to time: " + curTime.ToString());
            try
            {
                var regist = (from o in _context.OpenRegistrations
                              where o.ID == id
                              select o).FirstOrDefault();
                if (regist == null)
                {
                    throw new Exception("Aufruf konnte nicht ausgeführt werden. Der Eintrag wurde nicht in der DB gefunden.");
                }

                regist.TimeOfEntry = curTime;
                regist.StatusCall = status;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set call in DB. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Aufruf konnte nicht ausgeführt werden. ");
            }
        }


        public async Task Confirm(int id, DateTime curTime)
        {
            _logger.LogInformation("Confirm the call in DB for id: " + id + " to time: " + curTime.ToString());
            try
            {
                var regist = (from o in _context.OpenRegistrations
                              where o.ID == id
                              select o).FirstOrDefault();
                if (regist == null)
                {
                    throw new Exception("Aufruf konnte nicht ausgeführt werden. Der Eintrag wurde nicht in der DB gefunden.");
                }

                //regist.TimeOfEntry = curTime;
                //if (regist.StatusCall < 2)
                //{
                //    regist.StatusCall = 2;
                //}else if (regist.StatusCall==4)
                //{
                //    regist.StatusCall = 5;
                //}
                //else if (regist.StatusCall == 7)
                //{
                //    regist.StatusCall =8 ;
                //}
                //else
                //{
                //    regist.StatusCall = 2;
                //}
                regist.StatusCall += 1;

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set call in DB. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Aufruf konnte nicht ausgeführt werden. ");
            }
        }


        public async Task SetCallStatus(int id, int status)
        {

            try
            {
                var regist = (from o in _context.OpenRegistrations
                              where o.ID == id
                              select o).FirstOrDefault();
                if (regist == null)
                {
                    throw new Exception("Aufruf konnte nicht ausgeführt werden. Der Eintrag wurde nicht in der DB gefunden.");
                }


                regist.StatusCall = status;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set call in DB. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Aufruf konnte nicht ausgeführt werden. ");
            }
        }

        public async Task IncCallStatus(int id)
        {

            try
            {
                var regist = (from o in _context.OpenRegistrations
                              where o.ID == id
                              select o).FirstOrDefault();
                if (regist == null)
                {
                    throw new Exception("Aufruf konnte nicht ausgeführt werden. Der Eintrag wurde nicht in der DB gefunden.");
                }


                regist.StatusCall = regist.StatusCall == null ? 1 : regist.StatusCall + 1;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set call in DB. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Aufruf konnte nicht ausgeführt werden. ");
            }
        }





        public async Task SetEntry(int id, DateTime curTime)
        {
            _logger.LogInformation("Setting entry in db for id: " + id + " to time: " + curTime.ToString());
            try
            {
                var regist = (from o in _context.OpenRegistrations
                              where o.ID == id
                              select o).FirstOrDefault();
                if (regist == null)
                {
                    throw new Exception("Einfahrt konnte nicht gesetzt werden. Der Eintrag wurde nicht in der DB gefunden.");
                }
                regist.TimeOfEntry = curTime;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set entry in DB. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Einfahrt konnte nicht gesetzt werden.");
            }
        }

        public async Task SetProcessStart(int id, DateTime curTime)
        {
            _logger.LogInformation("Setting process start in db for id: " + id + " to time: " + curTime.ToString());
            try
            {
                var regist = (from o in _context.OpenRegistrations
                              where o.ID == id
                              select o).FirstOrDefault();
                if (regist == null)
                {
                    throw new Exception("Startzeitpunkt konnte nicht gesetzt werden. Der Eintrag wurde nicht in der DB gefunden.");
                }
                regist.ProcessStart = curTime;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set processStart in DB. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Startzeitpunkt konnte nicht gesetzt werden. ");
            }
        }

        public async Task SetProcessEnd(int id, DateTime curTime)
        {
            _logger.LogInformation("Setting process end in db for id: " + id + " to time: " + curTime.ToString());
            try
            {
                var regist = (from o in _context.OpenRegistrations
                              where o.ID == id
                              select o).FirstOrDefault();
                if (regist == null)
                {
                    throw new Exception("Endzeitpunkt konnte nicht gesetzt werden. Der Eintrag wurde nicht in der DB gefunden.");
                }
                regist.StatusCall = 5;
                regist.ProcessEnd = curTime;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set processEnd in DB. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Endzeitpunkt konnte nicht gesetzt werden.");
            }
        }

        public async Task Remove(OpenRegistration registration)
        {
            _logger.LogInformation("Removing registration from openRegistrations DB table");
            try
            {
                _context.OpenRegistrations.Remove(registration);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while removing openRegistration from db. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Eintrag konnte nicht von der Liste der offenen Anmeldungen entfernt werden.");
            }
        }

        public OpenRegistration Get(int id)
        {
            try
            {
                var regist = (from r in _context.OpenRegistrations
                              where r.ID == id
                              select r).FirstOrDefault();
                return regist;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get OpenRegistration with id: " + id + " Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Anmeldung konnte nicht gefunden werden. ");
            }
        }

        public async Task SetGate(int id, string value)
        {
            try
            {
                var regist = (from r in _context.OpenRegistrations
                              where r.ID == id
                              select r).FirstOrDefault();
                if (regist == null)
                {
                    _logger.LogError("Couldnt set Gate for id: " + id + " registration couldnt be found.");
                    throw new Exception("Tor konnte nicht gesetzt werden. Die entsprechende Anmeldung wurde nicht gefunden.");
                }
                regist.Gate = value;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set gate. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task SetLoadingStation(int id, string value)
        {
            try
            {
                var regist = (from r in _context.OpenRegistrations
                              where r.ID == id
                              select r).FirstOrDefault();
                if (regist == null)
                {
                    _logger.LogError("Couldnt set LoadingStation for id: " + id + " registration couldnt be found.");
                    throw new Exception("Die Ladestation konnte nicht gesetzt werden. Die entsprechende Anmeldung wurde nicht gefunden.");
                }
                regist.LoadingStation = value;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to set LoadingStation. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task ResetGate(int id)
        {
            try
            {
                var regist = (from r in _context.OpenRegistrations
                              where r.ID == id
                              select r).FirstOrDefault();
                if (regist == null)
                {
                    _logger.LogError("Couldnt edit registration wasnt found in DB. id: " + id);
                    throw new Exception("Änderungen konnten nicht übernommenwerden. Die Anmeldung wurde nicht gefunden.");
                }

                regist.Gate = "";
                regist.TimeOfCall = new DateTime();
                regist.TimeOfEntry = new DateTime();
                regist.ProcessEnd = new DateTime();
                regist.ProcessStart = new DateTime();

                await _context.SaveChangesAsync();

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + " inner: " + e.InnerException?.Message);
                throw e;
            }
        }

        public async Task SetCompanyName(int ID, string CompanyName, string ColorCode)
        {
            try
            {
                var regist = (from r in _context.OpenRegistrations
                              where r.ID == ID
                              select r).FirstOrDefault();
                if (regist == null)
                {
                    _logger.LogError("Couldnt set companyname because the registration wasnt found in DB. id: " + ID);
                    throw new Exception("Der Firmenname konnte nicht geändert werden.");
                }

                regist.CompanyName = CompanyName;
                regist.ColorCode = ColorCode;
                regist.SupplierNumber = null;

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + " inner: " + e.InnerException?.Message);
                throw e;
            }
        }

        public async Task SetComment(int ID, string Comment)
        {
            try
            {
                var regist = (from r in _context.OpenRegistrations
                              where r.ID == ID
                              select r).FirstOrDefault();
                if (regist == null)
                {
                    _logger.LogError("Couldnt set comment because the registration wasnt found in DB. id: " + ID);
                    throw new Exception("Der Firmenname konnte nicht geändert werden.");
                }
                regist.Comment = Comment;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + " inner: " + e.InnerException?.Message);
                throw e;
            }
        }

        public async Task SetLoadReference(int ID, string LoadReference)
        {
            try
            {
                var regist = (from r in _context.OpenRegistrations
                              where r.ID == ID
                              select r).FirstOrDefault();
                if (regist == null)
                {
                    _logger.LogError("Couldnt set LoadReference because the registration wasnt found in DB. id: " + ID);
                    throw new Exception("Der Firmenname konnte nicht geändert werden.");
                }
                regist.LoadReference = LoadReference;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message + " inner: " + e.InnerException?.Message);
                throw e;
            }
        }

        public async void RemoveAllLoadingStation(int ID)
        {
            try
            {
                var regist = (from r in _context.OpenRegistrations
                              where r.ID == ID
                              select r).FirstOrDefault();

                var loadingStation = _context.Gates.Where(x => x.Name == regist.Gate).FirstOrDefault().LoadingStation;

                if (regist == null)
                {
                    _logger.LogError("Couldnt remove the \"All\" LoadingStation for id: " + ID + " registration couldnt be found.");
                    throw new Exception("Die Ladestation \"Alle\" konnte nicht entfernt werden. Die entsprechende Anmeldung wurde nicht gefunden.");
                }
                regist.LoadingStation = loadingStation;

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to remove the \"All\" LoadingStation. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

    }
}
