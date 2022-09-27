using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVC.BusinessLogic.Interfaces;
using MVC.Data.Entities;
using MVC.Models;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace MVC.Controllers
{
    /// <summary>
    /// Handles the login and logout features.
    /// Also handles access denied and the creation of ad accounts
    /// </summary>
    /// <remarks>The controller uses the Authorize attribute to be able to create accounts in certain situations when windows authorization 
    /// is active on the iis and anonymous authentification isnt.</remarks>
    [Authorize]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public AccountController(IServiceProvider serviceProvider, ILogger<AccountController> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /// <summary>
        /// If anyone navigates to the controllers index url directly, the client will just be redirected to the login.
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {

            return RedirectToAction("Login");
        }


        /// <summary>
        /// Shows the Login view
        /// </summary>
        /// <remarks>Uses the AllowAnonymous attribute to enable everyone to login.</remarks>
        /// <param name="targetArea"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult Login(string targetArea, string msg = "")
        {
            // set the targetarea so that the correct menu item will be highlighted and the right page will be loaded after the login.
            if (!string.IsNullOrEmpty(targetArea))
                ViewBag.targetArea = targetArea;

            // if a message got passed along, use the ViewBag to pass it to the view and display it there.
            ViewBag.ErrorMessage = msg;
            using (var scope = _serviceProvider.CreateScope())
            {
                var _localizationFacade = scope.ServiceProvider.GetRequiredService<ILocalizationFacade>();

                ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
            }

            LoginViewModel loginViewModel = new LoginViewModel()
            {
                Password = string.Empty,
                UserName = string.Empty
            };

            return View(loginViewModel);
        }


        /// <summary>
        /// Handels the login procedure. 
        /// Depending on the settings, the user will be logged in using AD or local users. 
        /// Can also call CreateUserFromAD if the user could be validated by ad but doesnt have a local user account.
        /// </summary>
        /// <remarks>Uses the AllowAnonymous Attribute to allow anonymous clients to login.</remarks>
        /// <param name="details">The login information entered by the user (username and password)</param>
        /// <param name="targetArea">The area of the application that should be loaded after a successfull login</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel details, string targetArea)
        {
            if (targetArea == null)
                targetArea = "";

            using (var scope = _serviceProvider.CreateScope())
            {
                var _adReader = scope.ServiceProvider.GetRequiredService<IADReader>();
                var _accountFacade = scope.ServiceProvider.GetRequiredService<IAccountFacade>();
               
                var _localizationFacade = scope.ServiceProvider.GetRequiredService<ILocalizationFacade>();

                ViewBag.Localizations = _localizationFacade.Get(Thread.CurrentThread.CurrentCulture);
                // set the targetarea to highlight the correct menu item
                if (!string.IsNullOrEmpty(targetArea))
                    ViewBag.targetArea = targetArea;

                // Make sure that both a username and a password got entered
                if (ModelState.IsValid)
                {

                    AppUser user = await _userManager.FindByNameAsync(details.UserName);
                    if (user != null)
                    {
                        // sign out the current user. this is used so that it is possible to login to a different account from the same machine without any problems. Example 2 workers 1 pc and a new shift starts.
                        await _signInManager.SignOutAsync();

                        // if the use of AD is enabled, try to validate the userdata through ad
                        if (_accountFacade.UseAD)
                        {
                            var valid = _adReader.Validate(details.UserName, details.Password);
                            if (valid)
                            {
                                // if the validation was successful, sign in the user and redirect him to the desired controller / location
                                await _signInManager.SignInAsync(user, details.RememberMe);

                                List<string> rolenames = (List<string>)await _userManager.GetRolesAsync(user);
                                targetArea = getAllowedTargetarea(targetArea, rolenames);

                                return RedirectToAction("Index", targetArea ?? "/");
                            }
                        }

                        // if this point is reached, eigther the app doesnt use AD or the entered username and pw combination is not valid for active directory and is a local account instead.
                        var result = await _signInManager.PasswordSignInAsync(details.UserName, details.Password, details.RememberMe, lockoutOnFailure: false);
                        if (result.Succeeded)
                        {

                            List<string> rolenames = (List<string>)await _userManager.GetRolesAsync(user);
                            targetArea = getAllowedTargetarea(targetArea, rolenames);




                            return RedirectToAction("Index", targetArea ?? "/");
                        }
                    }
                    else // user == null
                    {
                        if (_accountFacade.UseAD && _accountFacade.GenerateAccountsForNewADUsers)
                        {
                            // if the user is null because no such local user exists, and the app uses AD and should generate accounts for new users, 
                            //then validate the userdata through AD and create the account with the given username
                            var valid = _adReader.Validate(details.UserName, details.Password);
                            if (valid)
                            {
                                try
                                {
                                    user = await CreateUserFromAD(details.UserName);
                                }
                                catch (Exception e)
                                {
                                    _logger.LogError("Error while trying to create user from AD Information. Message: " + e.Message + " inner exception message: " + e.InnerException?.Message);
                                    return RedirectToAction("Login", new { targetArea, msg = e.Message });
                                }

                                // if the creation of the account was successful and the user isnt signed in yet, sign him in and go to the disired location.
                                if (user != null && !_signInManager.IsSignedIn(User))
                                {
                                    await _signInManager.SignOutAsync();
                                    await _signInManager.SignInAsync(user, false);

                                    List<string> rolenames = (List<string>)await _userManager.GetRolesAsync(user);
                                    targetArea = getAllowedTargetarea(targetArea, rolenames);

                                    return RedirectToAction("Index", targetArea ?? "/");
                                }
                            }
                        }
                    }
                    // This point will only be reached if the user password combination couldnt be validated. Neighter through AD nor local account.
                    ModelState.AddModelError(nameof(LoginViewModel.UserName), "Benutzername oder Passwort inkorrekt.");
                    ModelState.AddModelError(nameof(LoginViewModel.Password), "");
                }

                // return the login View again. This will only happen if the login wasnt successful
                return View(details);
            }
        }



        /// <summary>
        /// Hanldes the Logout of the user and redirects him to the login.
        /// The targetarea is used to allow the user to get to the site he logged out from when he logs back in.
        /// </summary>
        /// <remarks>Uses Authorize attribute since only people that are logged in can log out.</remarks>
        /// <param name="targetArea"></param>
        /// <returns></returns>
        [Authorize]
        public async Task<IActionResult> Logout(string targetArea = "")
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<AppUser>>();
                await _signInManager.SignOutAsync();

                return RedirectToAction("Login", new { targetArea });
            }
        }


        /// <summary>
        /// If the user isnt authenticated, he will be redirected to the login.
        /// Otherwise 
        /// </summary>
        /// <remarks>Uses AllowAnonymous attribute because clients that failed to authorize will land here.</remarks>
        /// <param name="targetArea"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public async Task<IActionResult> AccessDenied(string targetArea)
        {
            if (targetArea == null)
                targetArea = "";

            using (var scope = _serviceProvider.CreateScope())
            {
                var _accountFacade = scope.ServiceProvider.GetRequiredService<IAccountFacade>();
               
                if (!string.IsNullOrEmpty(targetArea))
                    ViewBag.targetArea = targetArea;
                try
                {
                    if (User?.Identity?.IsAuthenticated == true)
                    {
                        // if the username isnt empty, try to find the local account and 
                        var userName = User?.Identity?.Name;
                        if (!String.IsNullOrEmpty(userName))
                        {
                            var user = await _userManager.FindByNameAsync(userName);

                            if (user != null)
                            {
                                // User is authenticated and was found in db and still landed in access denied so he is not supposed to have access
                                return RedirectToAction("Login", new { targetArea, msg = "Sie haben keine Berechtigung um auf eine Seite zuzugreifen. Melden Sie sich bitte bei Ihrem Systemadministrator.." });
                            }
                            else // user == null
                            {
                                // if the user wasnt found and the app uses AD and should generate accounts for new users, then try to do so from the users identity name
                                if (_accountFacade.UseAD && _accountFacade.GenerateAccountsForNewADUsers)
                                {

                                    try
                                    {
                                        user = await CreateUserFromAD(User?.Identity?.Name);
                                    }
                                    catch (Exception e)
                                    {
                                        _logger.LogError("Error while trying to create user from AD Information. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                                        return RedirectToAction("Login", new { targetArea, msg = e.Message });
                                    }

                                    // if the account creation was successful, sign in the user and redirect him to the desired location
                                    if (user != null && !_signInManager.IsSignedIn(User))
                                    {
                                        await _signInManager.SignOutAsync();
                                        await _signInManager.SignInAsync(user, false);

                                        List<string> rolenames = (List<string>)await _userManager.GetRolesAsync(user);
                                        targetArea = getAllowedTargetarea(targetArea, rolenames);

                                        return RedirectToAction("Index", targetArea ?? "/");
                                    }
                                }

                                // if the app doesnt use AD or shouldnt create local accounts, then just redirect to the login
                                return RedirectToAction("Login", new { targetArea });
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while handeling 'Access Denied'. Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                }

                // if the user isnt authenticated, just redirect to login
                return RedirectToAction("Login");
            }
        }


        /// <summary>
        /// Creates a local user account for the given username. 
        /// Uses the AD groups that the user is part of, to set the authorization group of the local account
        /// and there by add him to the group specific roles.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>AppUser</returns>
        private async Task<AppUser> CreateUserFromAD(string userName)
        {
            if (String.IsNullOrEmpty(userName))
                return null;
            _logger.LogInformation("Creating User with name:" + userName + " from AD Information.");
            using (var scope = _serviceProvider.CreateScope())
            {
                var _groupsRepository = scope.ServiceProvider.GetRequiredService<IAuthorizationGroupsRepository>();
                var _adReader = scope.ServiceProvider.GetRequiredService<IADReader>();
                //var _userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

                var adGroups = new List<String>();
                try
                {
                    // accquire the names of the AD groups that this user is part of
                    adGroups = _adReader.GetADGroups(userName);
                }
                catch (Exception e)
                {
                    _logger.LogError("Error while trying to get ad groups for user: " + userName + ". Message: " + e.Message + " Inner exception message: " + e.InnerException?.Message);
                    throw new Exception("Fehler beim erstellen des Nutzers aus den AD Daten. ");
                }


                // create the new user
                var newUser = new AppUser
                {
                    UserName = userName
                };

                // get all authorization groups and check if any of them match the ad groups that the user is part of
                var authGroups = _groupsRepository.GetAll();
                var matchingGroups = new List<string>();
                foreach (var adGrp in adGroups)
                {
                    foreach (var authGrp in authGroups)
                    {
                        if (adGrp == authGrp.ADGroupName)
                        {
                            matchingGroups.Add(authGrp.Name);
                        }
                    }
                }
                // if there are multiple authorization groups that this user could be part of, set his authorization group to the first one found.
                // this case should be avoided if possible because the resulting rights of the user are borderline random
                if (matchingGroups.Count > 0)
                {
                    newUser.AuthorizationGroup = matchingGroups[0];
                }

                // now that all the user info is gathered, create the identity user
                var result = await _userManager.CreateAsync(newUser);

                // if the user is part of an authorization group, set his roles
                if (!string.IsNullOrEmpty(newUser.AuthorizationGroup))
                    await SetUserRoles(newUser);

                // finally return the new User
                return newUser;
            }
        }

        /// <summary>
        /// Sets the roles of the passed along user to match the roles
        /// appropriate for his authorization group.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task SetUserRoles(AppUser user)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _groupsRepository = scope.ServiceProvider.GetRequiredService<IAuthorizationGroupsRepository>();
             
                var group = _groupsRepository.Get(user.AuthorizationGroup);
                if (group == null)
                {
                    return;
                }
                List<string> addRoles = new List<string>();

                var roles = await _userManager.GetRolesAsync(user);

                // Processing
                if (group.CanUseProcessingList)
                    addRoles.Add("CanUseProcessingList");

                if (group.CanModifyProcessingList)
                    addRoles.Add("CanModifyProcessingList");

                if (group.CanSetLoadingStation)
                    addRoles.Add("CanSetLoadingStation");

                if (group.CanSetGate)
                    addRoles.Add("CanSetGate");

                if (group.CanSetRelease)
                    addRoles.Add("CanSetRelease");

                if (group.CanSetCall)
                    addRoles.Add("CanSetCall");

                if (group.CanSetEntrance)
                    addRoles.Add("CanSetEntrance");

                if (group.CanSetProcessStart)
                    addRoles.Add("CanSetProcessStart");

                if (group.CanSetProcessEnd)
                    addRoles.Add("CanSetProcessEnd");

                if (group.CanSetExit)
                    addRoles.Add("CanSetExit");

                // History
                if (group.CanUseHistory)
                    addRoles.Add("CanUseHistory");

                if (group.CanExportHistory)
                    addRoles.Add("CanExportHistory");

                // Config
                if (group.CanUseConfig)
                    addRoles.Add("CanUseConfig");

                if (group.CanModifyAllSettings)
                    addRoles.Add("CanModifyAllSettings");

                if (group.CanInspectApproachTyps)
                    addRoles.Add("CanInspectApproachTyps");

                if (group.CanModifyApproachTyps)
                    addRoles.Add("CanModifyApproachTyps");

                if (group.CanInspectUnknownApproachTyps)
                    addRoles.Add("CanInspectUnknownApproachTyps");

                if (group.CanModifyUnknownApproachTyps)
                    addRoles.Add("CanModifyUnknownApproachTyps");

                var resultRemove = await _userManager.RemoveFromRolesAsync(user, roles);
                var resultAdd = await _userManager.AddToRolesAsync(user, addRoles);
            }
        }

        private string getAllowedTargetarea(string targetArea, List<string> roles)
        {
            bool targetAreaSet = true;

            if (!string.IsNullOrWhiteSpace(targetArea))
            {
                if (targetArea == "Processing")
                {
                    if (!roles.Contains("CanUseProcessingList"))
                        targetAreaSet = false;
                }
                else if (targetArea == "History")
                {
                    if (!roles.Contains("CanUseHistory"))
                        targetAreaSet = false;
                }
                else if (targetArea == "Configuration")
                {
                    if (!roles.Contains("CanUseConfig"))
                        targetAreaSet = false;
                }
            }

            if (!targetAreaSet || string.IsNullOrWhiteSpace(targetArea))
            {
                if (roles.Contains("CanUseProcessingList"))
                {
                    targetArea = "Processing";
                }
                else if (roles.Contains("CanUseHistory"))
                {
                    targetArea = "History";
                }
                else if (roles.Contains("CanUseConfig"))
                {
                    targetArea = "Configuration";
                }else
                {
                    targetArea = "Processing";
                }
            }

            return targetArea;
        }
    }
}