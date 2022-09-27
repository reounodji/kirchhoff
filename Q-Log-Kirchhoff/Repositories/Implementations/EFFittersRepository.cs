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
    public class EFFittersRepository : IFittersRepository
    {
        private readonly ILogger<EFFittersRepository> _logger;

        private readonly ApplicationDBContext _context;

        public EFFittersRepository(ILogger<EFFittersRepository> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Add(Fitter fitter)
        {
            if (fitter == null)
            {
                throw new ArgumentNullException("Monteur ist null");
            }
            _logger.LogInformation("Adding Fitter.");
            try
            {
                _context.Add(fitter);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add fitter. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task Delete(int id)
        {
            var fitter = Get(id);
            if (fitter == null)
            {
                _logger.LogWarning("Did not remove fitter with id: " + id + " because it wasnt found in the db.");
                return;
            }
            try
            {

                _context.Fitters.Remove(fitter);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete fitter. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Monteur konnte nicht gelöscht werden.");

            }
        }

        public async Task Edit(Fitter fitter)
        {
            if (fitter == null)
            {
                _logger.LogError("Could not edit fitter. param was null.");
                throw new ArgumentNullException("fitter is null");
            }
            try
            {
                var curFitter = (from a in _context.Fitters
                                 where a.ID == fitter.ID
                                 select a).FirstOrDefault();
                if (curFitter == null)
                {
                    _logger.LogWarning("Could not edit fitter. Fitter wasnt found in db. id: " + fitter.ID);
                    return;
                }
                curFitter.Name = fitter.Name;
                curFitter.ColorCode = fitter.ColorCode;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not edit fitter. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Monteur konnte nicht bearbeitet werden. Stellen Sie sicher, dass der Name nicht bereits verwendet wird.");
            }
        }

        public Fitter Get(string name)
        {
            try
            {
                var fitter = (from a in _context.Fitters
                              where a.Name == name
                              select a).FirstOrDefault();
                return fitter;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add get fitter with name: " + name + ". Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Monteur konnte nicht geladen werden.");

            }
        }

        public Fitter Get(int id)
        {
            try
            {
                var fitter = (from a in _context.Fitters
                              where a.ID == id
                              select a).FirstOrDefault();
                return fitter;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add get fitter with id: " + id + ". Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Monteur konnte nicht geladen werden.");

            }
        }

        public List<Fitter> GetAll()
        {
            try
            {
                return _context.Fitters.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get all Fitters. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task Import(List<Fitter> fitters)
        {
            var oldData = new List<Fitter>();
            var uniqueNames = CheckForUniqueNames(fitters);
            if (!uniqueNames)
            {
                throw new Exception("Die Namen der Monteure müssen einzigartig sein!");
            }
            try
            {
                oldData = (from f in _context.Fitters
                           select f).ToList();
                _context.Fitters.RemoveRange(oldData);

                await _context.Fitters.AddRangeAsync(fitters);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                try
                {
                    await _context.Fitters.AddRangeAsync(oldData);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Could not add the old data back to db. Message: " + ex.Message + " inner: " + e.InnerException?.Message);
                }
                _logger.LogError("Error while trying to import fitters into db. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Fehler beim Speichern der Daten in der Datenbank.");
            }
        }

        private bool CheckForUniqueNames(List<Fitter> fitters)
        {
            for (int i = 0; i < fitters.Count; i++)
            {
                for (int j = i + 1; j < fitters.Count; j++)
                {
                    if (fitters[i].Name == fitters[j].Name)
                        return false;
                }
            }

            return true;
        }

        public string GetColorCode(string Name)
        {
            try
            {
                var fitter = (from a in _context.Fitters
                              where a.Name == Name
                              select a).FirstOrDefault();
                return fitter?.ColorCode ?? "";
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get fitter ColorCode with name: " + Name + ". Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Monteur konnte nicht geladen werden.");
            }
        }
    }
}
