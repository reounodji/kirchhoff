using MVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface IOpenRegistrationsRepository
    {



        string GetLanguageCode(int id);

        OpenRegistration GetWithCompressedLicensePlate(string licensePlate);

        /// <summary>
        /// Uses toUpper() on the license plate and the forwarding agency.
        /// Adds the registration to the DB. 
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        Task<int> Add(Registration registration);

        /// <summary>
        /// Gets all OpenRegistrations from the DB and returns them as a list.
        /// </summary>
        /// <returns></returns>
        List<OpenRegistration> GetAll();

        /// <summary>
        /// Sets the time of Call for the OpenRegistration with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="curTime"></param>
        /// <returns></returns>
        Task Call(int id,string gate, DateTime curTime);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="curTime"></param>
        /// <returns></returns>
        Task UpdateCallStatus(int id, int status, DateTime curTime);

        /// <summary>
        /// Confirm the call
        /// </summary>
        /// <param name="id"></param>
        /// <param name="curTime"></param>
        /// <returns></returns>
        Task Confirm(int id, DateTime curTime);

        /// <summary>
        /// Sets the time of Release for the OpenRegistration with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="curTime"></param>
        /// <returns></returns>
        Task SetRelease(int id, DateTime curTime);

        /// <summary>
        /// Sets the time of Entry for the OpenRegistration with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="curTime"></param>
        /// <returns></returns>
        Task SetEntry(int id, DateTime curTime);

        Task SetProcessStart(int id, DateTime curTime);

        Task SetProcessEnd(int id, DateTime curTime);


        /// <summary>
        /// Removes the passed along registration from the DB.
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        Task Remove(OpenRegistration registration);

        /// <summary>
        /// Gets and returns the OpenRegistration with the given id from the DB.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        OpenRegistration Get(int id);

        /// <summary>
        /// Sets the new Value for the gate for an OpenRegistration with the give id.
        /// Afterwards returns the old gate.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetGate(int id, string value);

        /// <summary>
        /// Sets the Loadingstation for the given Registartion
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetLoadingStation(int id, string value);

        Task ResetGate(int id);

        Task IncCallStatus(int id);

        Task SetCallStatus(int id,int status);

        /// <summary>
        /// Sets the CompanyName in Registration with the given id
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="CompanyName"></param>
        /// <returns></returns>
        Task SetCompanyName(int ID, string CompanyName, string ColorCode);

        /// <summary>
        /// Sets the Comment in Registration with the given id
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        Task SetComment(int ID, string Comment);

        /// <summary>
        /// Sets the LoadReference in Registration with the given id
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="LoadReference"></param>
        /// <returns></returns>
        Task SetLoadReference(int ID, string LoadReference);

        /// <summary>
        /// Sets the LoadingStation of the registration to the correct one if All was selected
        /// </summary>
        /// <param name="regist"></param>
        void RemoveAllLoadingStation(int ID);

        /// <summary>
        /// Sets the Value if sending to ERP was Successful.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="wasSendingSuccessful"></param>
        /// <returns></returns>
        Task SetWasSendingSuccessful(int id, bool wasSendingSuccessful);
    }
}
