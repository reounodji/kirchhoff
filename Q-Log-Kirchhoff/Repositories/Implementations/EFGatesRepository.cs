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
    public class EFGatesRepository : IGatesRepository
    {
        private readonly ILogger<EFGatesRepository> _logger;

        private readonly ApplicationDBContext _context;

        public EFGatesRepository(ILogger<EFGatesRepository> logger, ApplicationDBContext context)
        {
            _context = context;
            _logger = logger;
        }

        public void Update(Gate gate)
        {
            _context.Update(gate);
            _context.SaveChanges();
        }

        public List<Gate> GetAll()
        {
            try
            {
                return _context.Gates.OrderBy(x => x.Name).ToList();
            }
            catch(Exception e)
            {
                _logger.LogError("Could not get all Gates. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task Import(List<Gate> gates)
        {
            var oldData = new List<Gate>();
            var uniqueNames = CheckForUniqueNames(gates);
            if (!uniqueNames)
            {
                throw new Exception("Die Namen der Tore müssen einzigartig sein!");
            }
            try
            {
                oldData = (from g in _context.Gates
                           select g).ToList();
                _context.Gates.RemoveRange(oldData);
                await _context.Gates.AddRangeAsync(gates);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                try
                {
                    await _context.Gates.AddRangeAsync(oldData);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError("Could not add the old data back to db. Message: " + ex.Message);
                }
                _logger.LogError("Error while trying to import gates into db. Message: " + e.Message);
                throw new Exception("Fehler beim Speichern der Daten in der Datenbank. Fehler: " + e.Message);
            }
        }

        private bool CheckForUniqueNames(List<Gate> gates)
        {
            for (int i = 0; i < gates.Count; i++)
            {
                for (int j = i + 1; j < gates.Count; j++)
                {
                    if (gates[i].Name == gates[j].Name)
                        return false;
                }
            }

            return true;
        }

        public async Task Add(Gate gate)
        {
            if (gate == null)
            {
                _logger.LogWarning("Tried to add gate = null");
                return;
            }

            try
            {
                await _context.Gates.AddAsync(gate);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not add gate to DB. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Tor konnte nicht hinzugefügt werden. Stellen Sie sicher, dass der Name nicht bereits vergeben ist.");
            }
        }

        public async Task Delete(int id)
        {
            var gate = Get(id);
            if (gate == null)
            {
                _logger.LogWarning("Cannot remove gate. No gate with id: " + id + " could be found.");
                return;
            }
            try
            {
                _context.Gates.Remove(gate);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not delete Gate. id: " + id + " Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public Gate Get(int id)
        {
            try
            {
                var gate = (from g in _context.Gates
                            where g.ID == id
                            select g).FirstOrDefault();
                return gate;
            }
            catch(Exception e)
            {
                _logger.LogError("Could not get gate with id: " + id + ". Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Tor konnte nicht geladen werden.");
            }
        }


        public Gate Get(string name)
        {
            return _context.Gates.Where(g => g.Name.ToUpper() == name.ToUpper()).FirstOrDefault();
        }


        public async Task Set(Gate gate)
        {
            if (gate == null)
            {
                _logger.LogWarning("Could not set gate. Param was null.");
                return;
            }
            var dbGate = Get(gate.ID);
            if (dbGate == null)
            {
                _logger.LogWarning("Could not set gate. No gate with id: " + gate.ID + " exists in the db.");
                return;
            }
            try
            {
                dbGate.Name = gate.Name;
                dbGate.Description = gate.Description;
                dbGate.LoadingStation = gate.LoadingStation;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not set gate. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Änderungen konnten nicht gespeichert werden. Stellen Sie sicher, dass der Name nicht bereits verwendet wird.");
            }

        }


        public List<Gate> GetAllGatesFromLoadingStation(LoadingStation loadingStation)
        {
            if (loadingStation.ShowAll)
                return GetAll();
            else 
                return GetAll().Where(x => x.LoadingStation == loadingStation.Name).ToList();
        }

        public string GetLoadingStationFromGate(string gate)
        {
            try
            {
                var loadingStation = _context.Gates.Where(x => x.Name == gate).FirstOrDefault().LoadingStation;

                return loadingStation;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get loadingstaion from gate with name: " + gate + ". Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Die Ladestation vom Tor " + gate + " konnte nicht gefunden werden.");
            }
        }
    }
}
