using MVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface IClosedRegistrationsRepository
    {
        /// <summary>
        /// Adds the ClosedRegistration to the DB.
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        Task Add(ClosedRegistration registration);

        /// <summary>
        /// Gets all ClosedRegistraions from the DB and returns them as a list.
        /// </summary>
        /// <returns></returns>
        List<ClosedRegistration> GetAll();

        List<ClosedRegistration> GetAll(DateTime start, DateTime end, string vehiculeNr, string firstname, string lastname, string phonenumber, string forwarder, string customer);

        ClosedRegistration Get(int id);

        Task SetWasSendingSuccessful(ClosedRegistration regist, bool wasSendingSuccessful);
    }
}
