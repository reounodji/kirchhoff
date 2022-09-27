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
    public class EFDisplayConfigurationRepository : IDisplayConfigurationRepository
    {
        private readonly ILogger<EFDisplayConfigurationRepository> _logger;

        private readonly ApplicationDBContext _context;

        public EFDisplayConfigurationRepository(ILogger<EFDisplayConfigurationRepository> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public DisplayConfiguration Get(int id)
        {
            _logger.LogInformation("Getting Displayconfig with id: " + id);
            try
            {
                var display = (from d in _context.Displays
                               where d.ID == id
                               select d).FirstOrDefault();
                return display;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get displayConfig with id: " + id + " from db. Error: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Display mit der id: " + id + " konnte nicht geladen werden. ");
            }
        }

        public List<DisplayConfiguration> GetAll()
        {
            _logger.LogTrace("Returning All DisplayConfigurations from DB.");

            try
            {
                var all = (from d in _context.Displays
                           select d).ToList();
                return all;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not accquire list of all displayConfigurations from db. Error: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Fehler beim laden der Displaykonfigurationen aus der Datenbank.");
            }
        }

        public async Task Add(DisplayConfiguration displayConfig)
        {
            _logger.LogInformation("Adding displayConfiguration to DB");
            try
            {
                _context.Displays.Add(displayConfig);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while adding displayCOnfiguration to the DB. Error: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Fehler beim hinzufügen einer Displaykonfiguration zur Datenbank.");
            }
        }

        public async Task Delete(int id)
        {
            _logger.LogInformation("Deleting displayConfiguration from DB. id: " + id);
            try
            {
                var display = (from d in _context.Displays
                               where d.ID == id
                               select d).FirstOrDefault();
                if (display == null)
                {
                    _logger.LogWarning("Didnt remove displayConfig from Db because no display with the id: " + id + " could be found.");
                    return;
                }
                _context.Displays.Remove(display);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Error while deleting displayConfiguration from db. Error: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Fehler beim Löschen des Displays.");
            }
        }

        public async Task EditAsync(DisplayConfiguration displayConfiguration)
        {
            _logger.LogInformation("Editing displayConfiguration with id: " + displayConfiguration.ID);
            try
            {
                var display = (from d in _context.Displays
                               where d.ID == displayConfiguration.ID
                               select d).FirstOrDefault();
                if (display == null)
                {
                    _logger.LogError("Could not edit display. no display with id: " + displayConfiguration.ID + " could be found in db.");
                    throw new Exception("Display mit der id: " + displayConfiguration.ID + " wurde nicht gefunden.");
                }
                display.Name = displayConfiguration.Name;
                display.IPAddress = displayConfiguration.IPAddress;
                display.Port = displayConfiguration.Port;
                display.TcpTimeoutInMs = displayConfiguration.TcpTimeoutInMs;
                display.ModeBreakInMs = displayConfiguration.ModeBreakInMs;
                display.Type = displayConfiguration.Type;
                display.Rows = displayConfiguration.Rows;
                display.CharsPerLine = displayConfiguration.CharsPerLine;
                display.EntryRemovalType = displayConfiguration.EntryRemovalType;
                display.curDisplayedStartingIndex = displayConfiguration.curDisplayedStartingIndex;

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not edit display. Error: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Display konnte nicht bearbeitet werden.");
            }
        }

        /// <summary>
        /// Can be called from the configuration view when editing a display.
        /// Is also called after every update of the display to change startingIndex.
        /// </summary>
        /// <param name="displayConfiguration"></param>
        public void Edit(DisplayConfiguration displayConfiguration)
        {
            //_logger.LogInformation("Editing displayConfiguration with id: " + displayConfiguration.ID);
            try
            {
                var display = (from d in _context.Displays
                               where d.ID == displayConfiguration.ID
                               select d).FirstOrDefault();
                if (display == null)
                {
                    _logger.LogError("Could not edit display. no display with id: " + displayConfiguration.ID + " could be found in db.");
                    throw new Exception("Display mit der id: " + displayConfiguration.ID + " wurde nicht gefunden.");
                }
                display.Name = displayConfiguration.Name;
                display.IPAddress = displayConfiguration.IPAddress;
                display.Port = displayConfiguration.Port;
                display.TcpTimeoutInMs = displayConfiguration.TcpTimeoutInMs;
                display.ModeBreakInMs = displayConfiguration.ModeBreakInMs;
                display.Type = displayConfiguration.Type;
                display.Rows = displayConfiguration.Rows;
                display.CharsPerLine = displayConfiguration.CharsPerLine;
                display.EntryRemovalType = displayConfiguration.EntryRemovalType;
                display.curDisplayedStartingIndex = displayConfiguration.curDisplayedStartingIndex;

                _context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError("Could not edit display. Error: " + e.Message + " inner: " + e.InnerException?.Message);
                throw new Exception("Display konnte nicht bearbeitet werden.");
            }
        }
    }
}
