using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.Data.DBContext;
using MVC.Data.Entities;
using MVC.Repositories.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Repositories.Implementations
{
    public class EFADSettingsRepository : IADSettingsRepository
    {
        private readonly ILogger<EFADSettingsRepository> _logger;
        private readonly IServiceProvider _serviceProvider;

        public EFADSettingsRepository(ILogger<EFADSettingsRepository> logger, IServiceProvider serviceProvider )
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public ADSettings Get()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _protectionProvider = scope.ServiceProvider.GetRequiredService<IDataProtectionProvider>();
                var _protector = _protectionProvider.CreateProtector("ADDomainPassword");
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();

                try
                {
                    var settings = context.ADSettings.FirstOrDefault();

                    if (!String.IsNullOrEmpty(settings.DomainUserPassword))
                        settings.DomainUserPassword = _protector.Unprotect(settings.DomainUserPassword);
                    return settings;
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while trying to get ad settings from db. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                    throw new Exception("Fehler beim laden der Active Directory Einstellungen aus der Datenbank.");
                }
            }
        }

        public async Task Set(ADSettings settings)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
                var _protectionProvider = scope.ServiceProvider.GetRequiredService<IDataProtectionProvider>();
                var _protector = _protectionProvider.CreateProtector("ADDomainPassword");

                try
                {
                    var set = _context.ADSettings.FirstOrDefault();
                    if (set != null)
                    {
                        if (!String.IsNullOrEmpty(settings.DomainNames))
                            set.DomainNames = settings.DomainNames;
                        else
                            set.DomainNames = null;

                        if (!String.IsNullOrEmpty(settings.DomainUserName))
                            set.DomainUserName = settings.DomainUserName;
                        else
                            set.DomainUserName = null;

                        if (!String.IsNullOrEmpty(settings.DomainUserPassword))
                            set.DomainUserPassword = _protector.Protect(settings.DomainUserPassword);
                        else
                            set.DomainUserPassword = null;

                        if (!String.IsNullOrEmpty(settings.ServerIP))
                            set.ServerIP = settings.ServerIP;
                        else
                            set.ServerIP = null;

                        set.UseAD = settings.UseAD;
                        set.GenerateAccountsForNewADUsers = settings.GenerateAccountsForNewADUsers;
                    }
                    else
                    {
                        _context.ADSettings.Add(settings);
                    }
                    await _context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while saving ad settings to db. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                    throw new Exception("Fehler beim speichern der Einstellungen in die Datenbank.");
                }
            }
        }
    }
}
