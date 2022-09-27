using MVC.Data.Entities;
using System.Collections.Generic;

namespace MVC.BusinessLogic.Interfaces
{
    public interface IExportFacade
    {
        /// <summary>
        /// Gets all unknownForwardingAgencies and creates a CSV-formatted string 
        /// from the data.
        /// </summary>
        /// <returns></returns>
        string GenerateUnknownForwardingAgenciesCSVData();

        /// <summary>
        /// Gets all unknownForwardingAgencies and creates a XML-formatted string 
        /// from the data.
        /// </summary>
        /// <returns></returns>
        string GenerateUnknownForwardingAgenciesXMLData();

        /// <summary>
        /// Gets all ClosedRegistrations from the DB and creates a CSV-formatted
        /// string from the data.
        /// </summary>
        /// <returns></returns>
        string GenerateRegistrationHistoryCSV(List<ClosedRegistration> closesRegistrations);

        string GenerateUnknownSuppliersCSVData();
        string GenerateUnknownFittersCSVData();
        string GenerateUnknownParcelServicesCSVData();
    }
}
