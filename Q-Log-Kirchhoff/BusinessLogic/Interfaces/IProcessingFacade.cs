using MVC.Models;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Interfaces
{
    public interface IProcessingFacade
    {
        /// <summary>
        /// Gets all OpenRegistrations from the IOpenRegistrationRepository
        /// and all Gates from the IGatesRepository. Creates the ViewModel and
        /// puts those in there.
        /// </summary>
        /// <returns></returns>
        ProcessingViewModel GetProcessingViewModel();

    }
}
