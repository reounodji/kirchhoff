namespace MVC.BusinessLogic.Interfaces
{
    public interface IAccountFacade
    {
        /// <summary>
        /// Gets the current setting from the settingsRepository.
        /// If UseAD == false, no user will be logged in using Active Directory
        /// </summary>
        bool UseAD { get; }

        /// <summary>
        /// Gets the current setting from the settingsRepository.
        /// If true, and UseAD == true, a new account will be created upon 
        /// requesting a site if the user is logged in via AD
        /// </summary>
        bool GenerateAccountsForNewADUsers { get; }
    }
}
