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
    public class EFLoadingStationsRepository :ILoadingStationsRepository
    {
        private readonly ILogger<EFLoadingStationsRepository> _logger;

        private readonly ApplicationDBContext _context;

        public EFLoadingStationsRepository(ILogger<EFLoadingStationsRepository> logger, ApplicationDBContext context)
        {
            _context = context;
            _logger = logger;
        }

        public void Update(LoadingStation LoadingStation)
        {
            _context.Update(LoadingStation);
            _context.SaveChanges();
        }

        public List<LoadingStation> GetAll()
        {
            try
            {
                return _context.LoadingStations.OrderBy(x => x.Name).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get all LoadingStations. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task Import(List<LoadingStation> LoadingStations)
        {
            var oldData = new List<LoadingStation>();
            var uniqueNames = CheckForUniqueNames(LoadingStations);
            if (!uniqueNames)
            {
                throw new Exception("Die Namen der Ladestationen müssen einzigartig sein!");
            }
            try
            {
                oldData = (from g in _context.LoadingStations
                           select g).ToList();
                oldData.Remove(oldData.Where(x => x.ShowAll).FirstOrDefault());
                _context.LoadingStations.RemoveRange(oldData);
                await _context.LoadingStations.AddRangeAsync(LoadingStations);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                try
                {
                    await _context.LoadingStations.AddRangeAsync(oldData);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Could not add the old data back to db. Message: " + ex.Message);
                }
                _logger.LogError("Error while trying to import LoadingStations into db. Message: " + e.Message);
                throw new Exception("Fehler beim Speichern der Daten in der Datenbank. Fehler: " + e.Message);
            }
        }

        private bool CheckForUniqueNames(List<LoadingStation> LoadingStations)
        {
            for (int i = 0; i < LoadingStations.Count; i++)
            {
                for (int j = i + 1; j < LoadingStations.Count; j++)
                {
                    if (LoadingStations[i].Name == LoadingStations[j].Name)
                        return false;
                }
            }

            return true;
        }

        public async Task Add(LoadingStation LoadingStation)
        {
            if (LoadingStation == null)
            {
                _logger.LogWarning("Tried to add LoadingStation = null");
                return;
            }

            try
            {
                await _context.LoadingStations.AddAsync(LoadingStation);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add LoadingStation to DB. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Ladestation konnte nicht hinzugefügt werden. Stellen Sie sicher, dass der Name nicht bereits vergeben ist.");
            }
        }

        public async Task Delete(int id)
        {
            var LoadingStation = Get(id);
            if (LoadingStation == null)
            {
                _logger.LogWarning("Cannot remove LoadingStation. No LoadingStation with id: " + id + " could be found.");
                return;
            }
            try
            {
                _context.LoadingStations.Remove(LoadingStation);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete LoadingStation. id: " + id + " Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public LoadingStation Get(int id)
        {
            try
            {
                var LoadingStation = (from g in _context.LoadingStations
                            where g.ID == id
                            select g).FirstOrDefault();
                return LoadingStation;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get LoadingStation with id: " + id + ". Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Ladestation konnte nicht geladen werden.");
            }
        }


        public LoadingStation Get(string name)
        {
            return _context.LoadingStations.Where(g => g.Name.ToUpper() == name.ToUpper()).FirstOrDefault();
        }


        public async Task Set(LoadingStation LoadingStation)
        {
            if (LoadingStation == null)
            {
                _logger.LogWarning("Could not set LoadingStation. Param was null.");
                return;
            }
            var dbLoadingStation = Get(LoadingStation.ID);
            if (dbLoadingStation == null)
            {
                _logger.LogWarning("Could not set LoadingStation. No LoadingStation with id: " + LoadingStation.ID + " exists in the db.");
                return;
            }
            try
            {
                dbLoadingStation.Name = LoadingStation.Name;
                dbLoadingStation.Description = LoadingStation.Description;
                dbLoadingStation.ShowAll = false;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not set LoadingStation. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Änderungen konnten nicht gespeichert werden. Stellen Sie sicher, dass der Name nicht bereits verwendet wird.");
            }

        }
    }
}
