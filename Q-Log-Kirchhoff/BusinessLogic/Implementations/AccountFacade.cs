using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Repositories.Interfaces;
using System;

namespace MVC.BusinessLogic.Implementations
{
    /// <summary>
    /// Provides Information concerning the UserAccount
    /// </summary>
    public class AccountFacade : IAccountFacade
    {
        private readonly ILogger<AccountFacade> _logger;
        private readonly IServiceProvider _serviceProvider;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public AccountFacade(ILogger<AccountFacade> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Property that checks the ADSettings weather the use of AD is enabled or not.
        /// </summary>
        public bool UseAD
        {
            get
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _adSettingsRepository = scope.ServiceProvider.GetRequiredService<IADSettingsRepository>();
                    var settings = _adSettingsRepository.Get();
                    if (settings != null)
                        return settings.UseAD;
                    _logger.LogWarning("Error loading settings while trying to check for UseAD");
                    return false;
                }
            }
        }

        /// <summary>
        /// Property that checks the ADSettings to check if accounts should be generated or not.
        /// </summary>
        public bool GenerateAccountsForNewADUsers
        {
            get
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _adSettingsRepository = scope.ServiceProvider.GetRequiredService<IADSettingsRepository>();
                    var settings = _adSettingsRepository.Get();
                    if (settings != null)
                        return settings.GenerateAccountsForNewADUsers;
                    _logger.LogWarning("Error loading settings while trying to check for GenerateAccountsForNewADUsers");
                    return false;
                }
            }
        }

    }
}
