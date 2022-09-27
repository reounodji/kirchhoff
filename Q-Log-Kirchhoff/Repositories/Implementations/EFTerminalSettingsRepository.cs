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
    public class EFTerminalSettingsRepository : ITerminalSettingsRepository
    {
        private readonly ILogger<EFTerminalSettingsRepository> _logger;

        private readonly ApplicationDBContext _context;

        public EFTerminalSettingsRepository(ILogger<EFTerminalSettingsRepository> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public TerminalSettings Get()
        {
            try
            {
                var settings = (from s in _context.TerminalSettings
                                select s).FirstOrDefault();
                return settings;
            }
            catch(Exception e)
            {
                _logger.LogError("Could not get TerminalSettings. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task Set(TerminalSettings settings)
        {
            if(settings == null)
            {
                _logger.LogError("Could not save terminalSettings. The param settings was null.");
                throw new ArgumentNullException("Settings ist leer. Einstellungen wurden nicht übernommen.");
            }
               
            if(settings.TimePerLanguage <= 0 || settings.TimeTillReset <= 0)
            {
                _logger.LogError("Cannot set terminal settings. The TimePerLanguage or TimeTillReset are <= 0.");
                throw new ArgumentOutOfRangeException("Die Zeit pro Sprache und die Zeit zum Rücksprung müssen >= 1 sekunde sein.");
            }

            try
            {
                var curSettings = (from s in _context.TerminalSettings
                                   select s).FirstOrDefault();
                if (curSettings != null)
                {
                    curSettings.TimePerLanguage = settings.TimePerLanguage;
                    curSettings.TimeTillReset = settings.TimeTillReset;
                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception e)
            {
                _logger.LogError("Could not set TerminalSettings. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }
    }
}
