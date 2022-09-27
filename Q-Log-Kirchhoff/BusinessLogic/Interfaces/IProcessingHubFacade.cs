using MVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Interfaces
{
    public interface IProcessingHubFacade
    {

        //Task ResetAction(int id, EActionType actionType);



        Task SetProcessStart(int id, DateTime curTime);

        Task SetProcessEnd(int id, DateTime curTime);

        /// <summary>
        /// exectues 'SetEntry(id, curTime)' on the IOpenRegistrationRepository to set the time.
        /// afterwards tells the DisplayFacade to Update the display. 
        /// If the Update was not successfull, SetEntry is executed again on the 
        /// IOpenRegisttaionRepository with new DateTime() as time.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="curTime"></param>
        /// <returns></returns>
        Task SetEntry(int id, DateTime curTime);

        /// <summary>
        /// tells the OpenRegistrationsRepository to remove the registration with 
        /// the given id.
        /// Creates a new ClosedRegistration from the registration data and calls
        /// all clients to add the closed registration.
        /// Finally updates the displays
        /// </summary>
        /// <param name="id"></param>
        /// <param name="curTime"></param>
        /// <returns></returns>
        Task SetExit(int id, DateTime curTime);

        /// <summary>
        /// Tells the openRegistrationRepository to set the new gate.
        /// Updates the displays afterwards.
        /// If the Update was unsuccessfull, call setGate on the OpenRegistrationRepository again with
        /// the pre change value, to restore the original state.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetGate(int id, string value);

        /// <summary>
        /// Tells the openRegistrationRepository to set the new LoadingStation
        /// if the selectedGate is not from this Loadingstation it will get set to no Gate
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task SetLoadingStation(int id, string value);

        Task ResetGate(int id);

        List<Gate> GetAllGates();

        /// <summary>
        /// Returns all Gatenames from a Loadingstation seperated by comma
        /// </summary>
        /// <param name="selectedLoadingStation"></param>
        /// <returns></returns>
        string GetGatesAsString(string selectedLoadingStation);

        /// <summary>
        /// Returns all Loadingstationsnames in a String seperated by comma
        /// </summary>
        /// <returns></returns>
        string GetLoadingstationsAsString();

        /// <summary>
        /// Updates the Companyname of the registration with the given id
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="CompanyName"></param>
        /// <returns></returns>
        Task<string> UpdateCompanyName(int ID, string CompanyName);

        /// <summary>
        /// Updates the Comment of the registration with the given id
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="Comment"></param>
        /// <returns></returns>
        Task UpdateComment(int ID, string Comment);

        /// <summary>
        /// Updates the LoadReference of the registration with the given id
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="LoadReference"></param>
        /// <returns></returns>
        Task UpdateLoadReference(int ID, string LoadReference);

        /// <summary>
        /// Sets the registration, with the given id, on released an saves the given timestamp.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="curTime"></param>
        /// <returns></returns>
        Task SetRelease(int id, DateTime curTime);
        
        /// <summary>
        /// Sets the call of the registration with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="curTime"></param>
        /// <returns></returns>
        Task SetCall(int id,string gate, DateTime curTime);


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="curTime"></param>
        /// <returns></returns>
        Task SetUpdateCallStatus(int id, int status, DateTime curTime);

        /// <summary>
        /// confirm the process call
        /// </summary>
        /// <param name="id"></param>
        /// <param name="curTime"></param>
        /// <returns></returns>
        Task SetConfirm(int id,DateTime curTime);

        /// <summary>
        /// Send the Entry with the given id to ERP
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task SendOpenRegistrationToERP(int id);
    }
}
