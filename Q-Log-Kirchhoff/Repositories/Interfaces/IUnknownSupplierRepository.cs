using MVC.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface IUnknownSupplierRepository
    {
        /// <summary>
        /// Adds a new UnknownForwardingAgency with the given name, numberOFAppereances = 1 and
        /// the time of first Appereance to the current servertime.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task Add(string name);

        /// <summary>
        /// Retrieves the list of all UnknownForwardingAgencies from the DB.
        /// </summary>
        /// <returns></returns>
        List<UnknownSupplier> GetAll();

        Task Delete(int id);
    }
}
