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
    public class EFSMSSettingsRepository : ISMSSettingsRepository
    {
        private readonly ILogger<EFSMSSettingsRepository> _logger;

        private readonly ApplicationDBContext _context;

        public EFSMSSettingsRepository(ILogger<EFSMSSettingsRepository> logger, ApplicationDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public SMSSettings Get()
        {
            try
            {
                var settings = (from s in _context.SMSSettings
                                select s).FirstOrDefault();
                return settings;
            }
            catch(Exception e)
            {
                _logger.LogError("Could not get SMSSettings. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw;
            }
        }

        public async Task Set(SMSSettings settings)
        {
            if(settings == null)
            {
                _logger.LogError("Could not save smssettings. The param settings was null.");
                throw new ArgumentNullException("Settings ist leer. Einstellungen wurden nicht übernommen.");
            }
            try
            {
                var curSettings = (from s in _context.SMSSettings
                                   select s).FirstOrDefault();
                if (curSettings != null)
                {
                    curSettings.UseSMSService = settings.UseSMSService;
                    curSettings.Password = settings.Password;
                    curSettings.Username = settings.Username;
                    curSettings.AccountReference = settings.AccountReference;
                    _context.Update(curSettings);
                    await _context.SaveChangesAsync();
                }
            }
            catch(Exception e)
            {
                _logger.LogError(e.Message + " inner: " + e.InnerException?.Message);
            }
        }
    }
}
