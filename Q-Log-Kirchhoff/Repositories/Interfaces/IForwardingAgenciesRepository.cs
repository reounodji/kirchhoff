using MVC.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface IForwardingAgenciesRepository
    {
        /// <summary>
        /// Returns the forwardingAgency with the given name.
        /// Will return null if no such Agency is found.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ForwardingAgency Get(string name);

        /// <summary>
        /// Returns the ForwardingAgency with the given id.
        /// Will return null if no such Agency is found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ForwardingAgency Get(int id);


        /// <summary>
        /// Returns a list of all ForwardingAgencies stored in the DB.
        /// </summary>
        /// <returns></returns>
        List<ForwardingAgency> GetAll();

        /// <summary>
        /// Adds the ForwardingAgency to the DB.
        /// </summary>
        /// <param name="forwardingAgency"></param>
        /// <returns></returns>
        Task Add(ForwardingAgency forwardingAgency);

        /// <summary>
        /// Applies the values of the passed along ForwardingAgency to
        /// the corresponding DB entry.
        /// </summary>
        /// <param name="forwardingAgency"></param>
        /// <returns></returns>
        Task Edit(ForwardingAgency forwardingAgency);

        /// <summary>
        /// Removes the ForwardingAgency with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);

        /// <summary>
        /// Checks if all agencies in the list have a unique name.
        /// Afterwards removes all current ForwardingAgencies from 
        /// the DB and adds those from the list.
        /// </summary>
        /// <param name="agencies"></param>
        /// <returns></returns>
        Task Import(List<ForwardingAgency> agencies);

        /// <summary>
        /// returns the ColorCode of the ForwardingAgancy with the given Name
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        string GetColorCode(string Name);

    }
}
