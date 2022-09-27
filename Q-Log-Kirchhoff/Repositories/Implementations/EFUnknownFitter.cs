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
    public class EFUnknownFitter : IUnknownFitterRepository
    {
        private readonly ILogger<EFUnknownFitter> _logger;

        private readonly ApplicationDBContext _context;

        public EFUnknownFitter(ILogger<EFUnknownFitter> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Add(string name)
        {
            _logger.LogInformation("Adding unknown fitter. name: " + name);
            try
            {
                var existing = (from u in _context.UnknownFitters
                                where u.Name.ToUpper() == name.ToUpper()
                                select u).FirstOrDefault();
                if (existing != null)
                {
                    _logger.LogInformation("Fitter was already in db. Increasing number of appereances.");
                    existing.NumberOfAppereances++;
                    await _context.SaveChangesAsync();
                    return;
                }
                else
                {
                    var _unkown = new UnknownFitter();
                    _unkown.Name = name;
                    _unkown.NumberOfAppereances = 1;
                    _unkown.FirstAppereance = DateTime.Now;
                    _context.UnknownFitters.Add(_unkown);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error adding unkown fitter. Message: " + e.Message + " inner: " + e.InnerException?.Message);
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var fitter = (from s in _context.UnknownFitters
                                 where s.ID == id
                                 select s).FirstOrDefault();
                if (fitter != null)
                {
                    _context.UnknownFitters.Remove(fitter);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning("Did not delete UnknownFitter with id: " + id + " because no such entry was found.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete UnknownFitter. id: " + id + " Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public List<UnknownFitter> GetAll()
        {
            try
            {
                return (from u in _context.UnknownFitters
                        select u).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get all UnknownFitter. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }
    }
}
