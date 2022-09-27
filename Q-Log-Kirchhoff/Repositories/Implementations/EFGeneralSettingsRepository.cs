using Microsoft.Extensions.Logging;
using MVC.Data.DBContext;
using MVC.Data.Entities;
using MVC.Models.ConfigurationViewModels;
using MVC.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Repositories.Implementations
{
    public class EFGeneralSettingsRepository : IGeneralSettingsRepository
    {
        private readonly ILogger<EFGeneralSettingsRepository> _logger;

        private readonly ApplicationDBContext _context;

        public EFGeneralSettingsRepository(ILogger<EFGeneralSettingsRepository> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public GeneralSettings GetGeneralSettings()
        {
            try
            {
                return _context.GeneralSettings.FirstOrDefault();
            }
            catch(Exception e)
            {
                _logger.LogError("Could not get GeneralSettings. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task SetGeneralSettings(GeneralSettingsViewModel model)
        {
            _logger.LogInformation("Setting general settings");
            if (model == null)
            {
                _logger.LogError("Could not set generalSettings. model == null.");
                throw new ArgumentNullException("Fehler beim Speichern der Einstellungen. Das Einstellungsobjekt ist leer.");
            }
            var settings = _context.GeneralSettings.FirstOrDefault();
            if (settings == null)
            {
                _logger.LogError("Could not set general Settings. Settings == null. Does the generalsettings object exist in the db?");
                throw new ArgumentNullException("Fehler beim Speichern der Einstellungen.");
            }
            try
            {
                settings.RegistrationTimeThreshold = model.RegistrationTimeThreshold;
                settings.DefaultHistoryTimespan = model.DefaultHistoryTimespan;
                settings.DisplayUpdateInterval = model.UpdateDisplayInterval;
                settings.ExceededWaitTimeColorCode = model.ExceededWaitTimeColorCode;
                settings.HoverColorCode = model.HoverColorCode;
                settings.NewEntryColorCode = model.NewEntryColorCode;
                settings.RecentChangeColorCode = model.RecentChangeColorCode;
                settings.ExitColorCode = model.ExitColorCode;

                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                _logger.LogError("Could not set general Settings. Message: "+ e.Message + " inner: " + e.InnerException?.Message);
                throw new ArgumentNullException("Fehler beim Speichern der Einstellungen.");
                
            }
        }

    }
}
