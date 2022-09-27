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
    public class EFParcelServicesRepository : IParcelServicesRepository
    {
        private readonly ILogger<EFParcelServicesRepository> _logger;

        private readonly ApplicationDBContext _context;

        public EFParcelServicesRepository(ILogger<EFParcelServicesRepository> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Add(ParcelService parcelService)
        {
            if (parcelService == null)
            {
                throw new ArgumentNullException("Paketdienst ist null");
            }
            _logger.LogInformation("Adding ParcelService.");
            try
            {
                _context.Add(parcelService);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add ParcelService. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task Delete(int id)
        {
            var parcelService = Get(id);
            if (parcelService == null)
            {
                _logger.LogWarning("Did not remove ParcelService with id: " + id + " because it was not found in the db.");
                return;
            }
            try
            {

                _context.ParcelServices.Remove(parcelService);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete parcel service. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Paketdienst konnte nicht gelöscht werden.");

            }
        }

        public async Task Edit(ParcelService parcelService)
        {
            if (parcelService == null)
            {
                _logger.LogError("Could not edit parcel service. param was null.");
                throw new ArgumentNullException("ParcelService is null");
            }
            try
            {
                var curparcelService = (from a in _context.ParcelServices
                                 where a.ID == parcelService.ID
                                 select a).FirstOrDefault();
                if (curparcelService == null)
                {
                    _logger.LogWarning("Could not edit parcel service. Forwarding agency wasnt found in db. id: " + parcelService.ID);
                    return;
                }

                curparcelService.Name = parcelService.Name;
                curparcelService.ColorCode = parcelService.ColorCode;

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not edit parcel service. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Paketdienst konnte nicht bearbeitet werden. Stellen Sie sicher, dass der Name nicht bereits verwendet wird.");
            }
        }

        public ParcelService Get(string name)
        {
            try
            {
                var service = (from a in _context.ParcelServices
                              where a.Name == name
                              select a).FirstOrDefault();
                return service;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add get parcel service with name: " + name + ". Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Paketdienst konnte nicht geladen werden.");

            }
        }

        public ParcelService Get(int id)
        {
            try
            {
                var service = (from a in _context.ParcelServices
                              where a.ID == id
                              select a).FirstOrDefault();
                return service;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add get parcel service with id: " + id + ". Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Paketdienst konnte nicht geladen werden.");

            }
        }

        public List<ParcelService> GetAll()
        {
            try
            {
                return _context.ParcelServices.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get all parcel services. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task Import(List<ParcelService> parcelServices)
        {
            var oldData = new List<ParcelService>();
            var uniqueNames = CheckForUniqueNames(parcelServices);

            if (!uniqueNames)
            {
                throw new Exception("Die Namen der Paketdienste müssen einzigartig sein!");
            }
            try
            {
                oldData = (from f in _context.ParcelServices
                           select f).ToList();
                _context.ParcelServices.RemoveRange(oldData);
                await _context.ParcelServices.AddRangeAsync(parcelServices);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                try
                {
                    await _context.ParcelServices.AddRangeAsync(oldData);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Could not add the old data back to db. Message: " + ex.Message + " inner: " + e.InnerException?.Message);
                }
                _logger.LogError("Error while trying to import parcel services into db. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Fehler beim Speichern der Daten in der Datenbank.");
            }
        }

        private bool CheckForUniqueNames(List<ParcelService> services)
        {
            for (int i = 0; i < services.Count; i++)
            {
                for (int j = i + 1; j < services.Count; j++)
                {
                    if (services[i].Name == services[j].Name)
                        return false;
                }
            }

            return true;
        }

        public string GetColorCode(string Name)
        {
            try
            {
                var parcelService = (from a in _context.ParcelServices
                              where a.Name == Name
                              select a).FirstOrDefault();
                return parcelService?.ColorCode ?? "";
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get parcelService ColorCode with name: " + Name + ". Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Paketdienst konnte nicht geladen werden.");
            }
        }
    }
}