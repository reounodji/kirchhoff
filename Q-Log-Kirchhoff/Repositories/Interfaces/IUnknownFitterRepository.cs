using MVC.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface IUnknownFitterRepository
    {
        /// <summary>
        /// Adds a new UnknownFitter with the given name, numberOFAppereances = 1 and
        /// the time of first Appereance to the current servertime.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task Add(string name);

        /// <summary>
        /// Retrieves the list of all UnknownFitters from the DB.
        /// </summary>
        /// <returns></returns>
        List<UnknownFitter> GetAll();


        /// <summary>
        /// Delets the Fitter from the DB with given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);
    }
}