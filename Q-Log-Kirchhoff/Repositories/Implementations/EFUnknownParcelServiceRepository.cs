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
    public class EFUnknownParcelServiceRepository : IUnknownParcelServiceRepository
    {
        private readonly ILogger<EFUnknownParcelServiceRepository> _logger;

        private readonly ApplicationDBContext _context;

        public EFUnknownParcelServiceRepository(ILogger<EFUnknownParcelServiceRepository> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Add(string name)
        {
            _logger.LogInformation("Adding unknown parcel service. name: " + name);
            try
            {
                var existing = (from u in _context.UnknownParcelServices
                                where u.Name.ToUpper() == name.ToUpper()
                                select u).FirstOrDefault();
                if (existing != null)
                {
                    _logger.LogInformation("UnknownParcelService was already in db. Increasing number of appereances.");
                    existing.NumberOfAppereances++;
                    await _context.SaveChangesAsync();
                    return;
                }
                else
                {
                    var _unkown = new UnknownParcelService();
                    _unkown.Name = name;
                    _unkown.NumberOfAppereances = 1;
                    _unkown.FirstAppereance = DateTime.Now;
                    _context.UnknownParcelServices.Add(_unkown);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error adding unkown parcel service. Message: " + e.Message + " inner: " + e.InnerException?.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var parcelService = (from s in _context.UnknownParcelServices
                                 where s.ID == id
                                 select s).FirstOrDefault();
                if (parcelService != null)
                {
                    _context.UnknownParcelServices.Remove(parcelService);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning("Did not delte UnknownParcelService with id: " + id + " because no such entry was found.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete UnknownParcelService. id: " + id + " Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public List<UnknownParcelService> GetAll()
        {
            try
            {
                return (from u in _context.UnknownParcelServices
                        select u).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get all UnknownParcelServices. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }
    }
}