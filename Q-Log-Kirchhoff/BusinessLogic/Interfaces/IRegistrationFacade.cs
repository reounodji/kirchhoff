using MVC.Models;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Interfaces
{
    public interface IRegistrationFacade
    {
        /// <summary>
        /// Creates an OpenRegistration from the ViewModel and tells the IOpenRegistrationsRepository to add it.
        /// Calls addRegistration on all clients of the processinghub to update their lists.
        /// Finally update the displays.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task AddRegistrationFromViewModel(RegistrationViewModel model);

        void CheckForUnknownInput(RegistrationViewModel model);

        RegistrationViewModel GetRegistrationViewModel();
    }
}
