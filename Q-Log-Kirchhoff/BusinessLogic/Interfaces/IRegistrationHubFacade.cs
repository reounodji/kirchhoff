using MVC.Data.Entities;
using System.Collections.Generic;

namespace MVC.BusinessLogic.Interfaces
{
    public interface IRegistrationHubFacade
    {
        /// <summary>
        /// Gets all ForwardingAgencies from the IForwardingAgenciesRepository
        /// and returns the names of those that contain the input string.
        /// The check is dont with both sides toUpper().
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //List<string> GetForwardingAgenciesWithName(string input);

        List<ForwardingAgency> GetForwardingAgencies(string input);

        List<string> GetSuppliers(string input);

        List<ParcelService> GetParcelServices(string input);

        List<Fitter> GetFitters(string input);
    }
}
