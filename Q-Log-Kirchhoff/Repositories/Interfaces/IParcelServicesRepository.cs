using MVC.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace MVC.Repositories.Interfaces
{
    public interface IParcelServicesRepository
    {
        /// <summary>
        /// Returns the ParcelService with the given name.
        /// Will return null if no such ParcelService is found.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        ParcelService Get(string name);

        /// <summary>
        /// Returns the ParcelService with the given id.
        /// Will return null if no such ParcelService is found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ParcelService Get(int id);


        /// <summary>
        /// Returns a list of all ParcelService stored in the DB.
        /// </summary>
        /// <returns></returns>
        List<ParcelService> GetAll();

        /// <summary>
        /// Adds the ParcelService to the DB.
        /// </summary>
        /// <param name="parcelService"></param>
        /// <returns></returns>
        Task Add(ParcelService parcelService);

        /// <summary>
        /// Applies the values of the passed along ParcelService to
        /// the corresponding DB entry.
        /// </summary>
        /// <param name="parcelService"></param>
        /// <returns></returns>
        Task Edit(ParcelService parcelService);

        /// <summary>
        /// Removes the ParcelService with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);

        /// <summary>
        /// Checks if all ParcelServices in the list have a unique name.
        /// Afterwards removes all current ParcelService from 
        /// the DB and adds those from the list.
        /// </summary>
        /// <param name="agencies"></param>
        /// <returns></returns>
        Task Import(List<ParcelService> parcelServices);


        /// <summary>
        /// returns the ColorCode of the parcel service with the given Name
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        string GetColorCode(string Name);
    }
}