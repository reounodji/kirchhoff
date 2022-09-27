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
    public class EFForwardingAgenciesRepository : IForwardingAgenciesRepository
    {
        private readonly ILogger<EFForwardingAgenciesRepository> _logger;

        private readonly ApplicationDBContext _context;

        public EFForwardingAgenciesRepository(ILogger<EFForwardingAgenciesRepository> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Add(ForwardingAgency forwardingAgency)
        {
            if (forwardingAgency == null)
            {
                throw new ArgumentNullException("Spedition ist null");
            }
            _logger.LogInformation("Adding ForwardingAgency.");
            try
            {
                _context.Add(forwardingAgency);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add ForwardingAgency. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public ForwardingAgency Get(string name)
        {
            try
            {
                var agency = (from a in _context.ForwardingAgencies
                              where a.Name == name
                              select a).FirstOrDefault();
                return agency;
            }
            catch(Exception e)
            {
                _logger.LogError("Could not add get agency with name: " + name + ". Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Spedition konnte nicht geladen werden.");
                
            }
        }


        public ForwardingAgency Get(int id)
        {
            try
            {
                var agency = (from a in _context.ForwardingAgencies
                              where a.ID == id
                              select a).FirstOrDefault();
                return agency;
            }
            catch(Exception e)
            {
                _logger.LogError("Could not add get agency with id: " + id + ". Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Spedition konnte nicht geladen werden.");

            }
        }


        public string GetColorCode(string Name)
        {
            try
            {
                var agency = (from a in _context.ForwardingAgencies
                              where a.Name == Name
                              select a).FirstOrDefault();
                return agency?.ColorCode ?? "";
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get agencies ColorCode with name: " + Name + ". Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Spedition konnte nicht geladen werden.");
            }
        }

        public List<ForwardingAgency> GetAll()
        {
            try
            {
                return _context.ForwardingAgencies.ToList();
            }
            catch(Exception e)
            {
                _logger.LogError("Could not get all ForwardingAgencies. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task Edit(ForwardingAgency forwardingAgency)
        {
            if (forwardingAgency == null)
            {
                _logger.LogError("Could not edit forwarding agency. param was null.");
                throw new ArgumentNullException("forwardingAgency is null");
            }
            try
            {
                var curAgency = (from a in _context.ForwardingAgencies
                                 where a.ID == forwardingAgency.ID
                                 select a).FirstOrDefault();
                if (curAgency == null)
                {
                    _logger.LogWarning("Could not edit forwarding agency. Forwarding agency wasnt found in db. id: " + forwardingAgency.ID);
                    return;
                }
                curAgency.Name = forwardingAgency.Name;
                curAgency.ColorCode = forwardingAgency.ColorCode;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not edit forwarding agency. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Spedition konnte nicht bearbeitet werden. Stellen Sie sicher, dass der Name nicht bereits verwendet wird.");
            }
        }

        public async Task Delete(int id)
        {
            var agency = Get(id);
            if (agency == null)
            {
                _logger.LogWarning("Did not remove ForwardingAgency with id: " + id + " because it wasnt found in the db.");
                return;
            }
            try
            {

                _context.ForwardingAgencies.Remove(agency);
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                _logger.LogError("Could not delete forwarding agency. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Spedition konnte nicht gelöscht werden.");

            }
        }

        public async Task Import(List<ForwardingAgency> agencies)
        {
            var oldData = new List<ForwardingAgency>();
            var uniqueNames = CheckForUniqueNames(agencies);
            if (!uniqueNames)
            {
                throw new Exception("Die Namen der Tore müssen einzigartig sein!");
            }
            try
            {
                oldData = (from f in _context.ForwardingAgencies
                           select f).ToList();
                _context.ForwardingAgencies.RemoveRange(oldData);
                await _context.ForwardingAgencies.AddRangeAsync(agencies);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                try
                {
                    await _context.ForwardingAgencies.AddRangeAsync(oldData);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Could not add the old data back to db. Message: " + ex.Message + " inner: " + e.InnerException?.Message);
                }
                _logger.LogError("Error while trying to import gates into db. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Fehler beim Speichern der Daten in der Datenbank.");
            }
        }

        private bool CheckForUniqueNames(List<ForwardingAgency> agencies)
        {
            for (int i = 0; i < agencies.Count; i++)
            {
                for (int j = i + 1; j < agencies.Count; j++)
                {
                    if (agencies[i].Name == agencies[j].Name)
                        return false;
                }
            }

            return true;
        }
    }
}
