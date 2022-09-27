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
    public class EFUnknownSupplierRepository : IUnknownSupplierRepository
    {
        private readonly ILogger<EFUnknownForwardingAgenciesRepository> _logger;

        private readonly ApplicationDBContext _context;

        public EFUnknownSupplierRepository(ILogger<EFUnknownForwardingAgenciesRepository> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Add(string name)
        {
            _logger.LogInformation("Adding unknown supplier. name: " + name);
            try
            {
                var existing = (from u in _context.UnknownSuppliers
                                where u.Name.ToUpper() == name.ToUpper()
                                select u).FirstOrDefault();
                if (existing != null)
                {
                    _logger.LogInformation("Supplier was already in db. Increasing number of appereances.");
                    existing.NumberOfAppereances++;
                    await _context.SaveChangesAsync();
                    return;
                }
                else
                {
                    var _unkown = new UnknownSupplier();
                    _unkown.Name = name;
                    _unkown.NumberOfAppereances = 1;
                    _unkown.FirstAppereance = DateTime.Now;
                    _context.UnknownSuppliers.Add(_unkown);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error adding unkown supplier. Message: " + e.Message + " inner: " + e.InnerException?.Message);
            }
        }

        public List<UnknownSupplier> GetAll()
        {
            try
            {
                return (from u in _context.UnknownSuppliers
                        select u).ToList();
            }
            catch(Exception e)
            {
                _logger.LogError("Could not get all UnknownSuppliers. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task Delete(int id)
        {
            try
            {
                var supp = (from s in _context.UnknownSuppliers
                            where s.ID == id
                            select s).FirstOrDefault();
                if(supp != null)
                {
                    _context.UnknownSuppliers.Remove(supp);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    _logger.LogWarning("Did not delte UnknownSupplier with id: " + id + " because no such entry was found.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete UnknownSupplier. id: " + id + " Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }
    }
}
