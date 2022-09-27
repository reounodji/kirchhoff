using MVC.Data.Entities;
using MVC.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace MVC.BusinessLogic.Interfaces
{
    public interface IHistoryFacade
    {
        /// <summary>
        /// Gets all closed Registrations from the IClosedRegistrationsRepository and fills
        /// the new ViewModel with them.
        /// </summary>
        /// <returns></returns>
        HistoryViewModel GetHistoryViewModel(DateTime start, DateTime end, string vehiculeNr , string firstname, string lastname, string phonenumber, string forwarder, string customer);

        /// <summary>
        /// Gets the CSV string from the IExportFacade, writes it to a .csv-file and returns a filestream
        /// to said file.
        /// </summary>
        /// <returns></returns>
        FileStream ExportRegistrationHistoryCSV(List<ClosedRegistration> closesRegistrations);

        DateTime GetDefaultHistoryStart();
    }
}
