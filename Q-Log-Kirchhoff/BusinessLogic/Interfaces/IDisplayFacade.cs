using MVC.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Interfaces
{
    public interface IDisplayFacade
    {
        /// <summary>
        /// Updates all displays. Returns true if successful, false otherwise
        /// </summary>
        /// <returns></returns>
        List<List<OpenRegistration>> Update(List<List<OpenRegistration>> previousRegistrationsToShow);    
    }
}
