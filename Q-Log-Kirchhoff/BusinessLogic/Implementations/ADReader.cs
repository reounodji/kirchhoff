using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;

namespace MVC.BusinessLogic.Implementations
{
    /// <summary>
    /// Handles the interaction with ActiveDirectory
    /// </summary>
    public class ADReader : IADReader
    {
        private readonly ILogger<ADReader> _logger;
        private readonly IServiceProvider _serviceProvider;

        private string _serverAddress;
        private string _domainNames;
        private string _domainUserName;
        private string _domainUserPass;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public ADReader(ILogger<ADReader> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Gets the names of all groups that the user is part of in ActiveDirectory.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<string> GetADGroups(string userName)
        {
            List<string> groups = new List<string>();
            try
            {
                using (var ctx = CreatePrincipalContext())
                {
                    if (ctx == null)
                    {
                        _logger.LogWarning("Could not create PrincipalContext! Check if the server info is correct and make sure that it is an ActiveDirectory server!");
                        return null;
                    }
                    // find a user
                    UserPrincipal user = UserPrincipal.FindByIdentity(ctx, userName);

                    if (user != null)
                    {
                        // get the authorization groups - those are the "roles" 
                        var g = user.GetAuthorizationGroups();

                        foreach (Principal principal in g)
                        {
                            groups.Add(principal.Name);
                        }
                    }
                }
                return groups;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not get list of groups" + " error: " + e.Message + " inner: " + e.InnerException?.Message);
                return new List<string>();
            }
        }

        /// <summary>
        /// Validates the userName and password against ActiveDirectory
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPass"></param>
        /// <returns></returns>
        public bool Validate(string userName, string userPass)
        {
            _logger.LogInformation("Validating credentials. ");

            try
            {
                using (var principalContext = CreatePrincipalContext())
                {
                    if (principalContext == null)
                    {
                        _logger.LogWarning("Could not create PrincipalContext! Check if the server info is correct and make sure that it is an ActiveDirectory server!");
                        return false;
                    }

                    bool valid = principalContext.ValidateCredentials(userName, userPass);
                    return valid;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Could not ValidateCredentials for user: " + userName + ". Message: " + e.Message + " inner: " + e.InnerException?.Message);
                return false;
            }
        }


        /// <summary>
        /// Loads the current ad settings and creates a PrincipalContext with the
        /// current serveraddress, domain names, domain userName and password
        /// </summary>
        /// <returns></returns>
        private PrincipalContext CreatePrincipalContext()
        {
            try
            {
                LoadSettings();
                var principalContext = new PrincipalContext(ContextType.Domain, _serverAddress, _domainNames, _domainUserName, _domainUserPass);
                return principalContext;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Could not create PrincipalContext! Check if the server info is correct and make sure that it is an ActiveDirectory server!" + e.Message + " inner: " + e.InnerException?.Message);
                throw e;
            }

        }


        /// <summary>
        /// Loads the AD settings from the repository.
        /// </summary>
        private void LoadSettings()
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var _adSettingsRepository = scope.ServiceProvider.GetRequiredService<IADSettingsRepository>();
                    var settings = _adSettingsRepository.Get();
                    _serverAddress = settings.ServerIP;
                    _domainNames = settings.DomainNames;
                    _domainUserName = settings.DomainUserName;
                    _domainUserPass = settings.DomainUserPassword;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("Error while trying to load ad Settings. Message: " + e.Message + " inner: " + e.InnerException?.Message);
                throw e;
            }
        }
    }
}
