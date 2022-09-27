using MVC.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface IUnknownParcelServiceRepository
    {
        /// <summary>
        /// Adds a new UnknownParcelService with the given name, numberOFAppereances = 1 and
        /// the time of first Appereance to the current servertime.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task Add(string name);

        /// <summary>
        /// Retrieves the list of all UnknownParcelServices from the DB.
        /// </summary>
        /// <returns></returns>
        List<UnknownParcelService> GetAll();

        /// <summary>
        /// Delets the UnknownParcelService with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);

    }
}
