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
    public class EFClosedRegistrationsRepository : IClosedRegistrationsRepository
    {
        private readonly ILogger<EFClosedRegistrationsRepository> _logger;

        private readonly ApplicationDBContext _context;

        public EFClosedRegistrationsRepository(ILogger<EFClosedRegistrationsRepository> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Add(ClosedRegistration registration)
        {
            _logger.LogInformation("Adding registration to history.");
            try
            {
                _context.RegistrationHistory.Add(registration);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add registration to history. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Eintrag konnte der Historie nicht hinzugefügt werden. ");
            }
        }

        public ClosedRegistration Get(int id)
        {
            _logger.LogInformation("Returning the Registrations with id: " + id);
            try
            {
                var regist = _context.RegistrationHistory.Where(x => x.ID == id).FirstOrDefault();
                return regist;
            }
            catch (Exception e)
            {
                _logger.LogError("Cannot get the Closed Registrations with id " + id + ". Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Die Registrierung mit der id " + id + " konnte nicht geladen werden. ");
            }
        }

        public List<ClosedRegistration> GetAll()
        {
            _logger.LogInformation("Returning all Registrations from the history.");
            try
            {
                var all = (from regist in _context.RegistrationHistory
                           select regist).ToList();
                return all;
            }
            catch (Exception e)
            {
                _logger.LogError("Cannot get all Closed Registrations. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Historie konnte nicht geladen werden. ");
            }
        }

        public List<ClosedRegistration> GetAll(DateTime start, DateTime end,string vehiculeNr, string firstname, string lastname, string phonenumber, string forwarder, string customer)
        {
            _logger.LogInformation("Returning all Registrations from the history.");
            try
            {
                IQueryable<ClosedRegistration> query = _context.RegistrationHistory;

                var all = (from regist in _context.RegistrationHistory
                           where regist.TimeOfRegistration >= start && regist.TimeOfRegistration <= end
                           select regist).ToList();

                query = query.Where(regist => regist.TimeOfRegistration >= start && regist.TimeOfRegistration <= end);

                if (!String.IsNullOrEmpty(vehiculeNr))
                {
                    query = query.Where(item => item.LicensePlate.Contains(vehiculeNr));
                }

                if (!String.IsNullOrEmpty(firstname))
                {
                    query = query.Where(item => item.FirstName.Contains(firstname));
                }

                if (!String.IsNullOrEmpty(lastname))
                {
                    query = query.Where(item => item.Lastname.Contains(lastname));
                }

                if (!String.IsNullOrEmpty(phonenumber))
                {
                    query = query.Where(item => item.Phonenumber.Contains(phonenumber));
                }

                if (!String.IsNullOrEmpty(forwarder))
                {
                    query = query.Where(item => item.Forwarder.Contains(forwarder));
                }

                if (!String.IsNullOrEmpty(customer))
                {
                    query = query.Where(item => item.Customer.Contains(customer));
                }

                return query.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Cannot get all Closed Registrations. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Historie konnte nicht geladen werden. ");
            }
        }

        public async Task SetWasSendingSuccessful(ClosedRegistration regist, bool wasSendingSuccessful)
        {
            _logger.LogInformation("Setting wasSendingSuccessful in DB for id: " + regist.ID + " to: " + wasSendingSuccessful.ToString());
            try
            {
                var curRegist = (from o in _context.RegistrationHistory
                              where o.ID == regist.ID
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
    }
}
