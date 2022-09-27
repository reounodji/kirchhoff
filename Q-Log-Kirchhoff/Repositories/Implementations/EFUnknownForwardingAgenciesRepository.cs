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
    public class EFUnknownForwardingAgenciesRepository : IUnknownForwardingAgenciesRepository
    {
        private readonly ILogger<EFUnknownForwardingAgenciesRepository> _logger;

        private readonly ApplicationDBContext _context;

        public EFUnknownForwardingAgenciesRepository(ILogger<EFUnknownForwardingAgenciesRepository> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Add(string name)
        {
            _logger.LogInformation("Adding unknown forwarding agency. name: " + name);
            try
            {
                var existing = (from u in _context.UnknownForwardingAgencies
                                where u.Name.ToUpper() == name.ToUpper()
                                select u).FirstOrDefault();
                if (existing != null)
                {
                    _logger.LogInformation("ForwardingAgency was already in db. Increasing number of appereances.");
                    existing.NumberOfAppereances++;
                    await _context.SaveChangesAsync();
                    return;
                }
                else
                {
                    var _unkown = new UnknownForwardingAgency();
                    _unkown.Name = name;
                    _unkown.NumberOfAppereances = 1;
                    _unkown.FirstAppereance = DateTime.Now;
                    _context.UnknownForwardingAgencies.Add(_unkown);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error adding unkown forwarding agency. Message: " + e.Message + " inner: " + e.InnerException?.Message);
            }
        }

        public List<UnknownForwardingAgency> GetAll()
        {
            try
            {
                return (from u in _context.UnknownForwardingAgencies
                        select u).ToList();
            }
            catch(Exception e)
            {
                _logger.LogError("Could not get all UnknownForwardingAgencies. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var forwarder = (from s in _context.UnknownForwardingAgencies
                            where s.ID == id
                            select s).FirstOrDefault();
                if (forwarder != null)
                {
                    _context.UnknownForwardingAgencies.Remove(forwarder);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning("Did not delte UnknownForwardingAgency with id: " + id + " because no such entry was found.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete UnknownForwardingAgency. id: " + id + " Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }
    }
}
