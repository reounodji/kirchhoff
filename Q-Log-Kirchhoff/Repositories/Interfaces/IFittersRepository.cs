using MVC.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface IFittersRepository
    {
        /// <summary>
        /// Returns the fitter with the given name.
        /// Will return null if no such Fitter is found.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Fitter Get(string name);

        /// <summary>
        /// Returns the Fitter with the given id.
        /// Will return null if no such Fitter is found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Fitter Get(int id);


        /// <summary>
        /// Returns a list of all Fitters stored in the DB.
        /// </summary>
        /// <returns></returns>
        List<Fitter> GetAll();

        /// <summary>
        /// Adds the Fitter to the DB.
        /// </summary>
        /// <param name="fitter"></param>
        /// <returns></returns>
        Task Add(Fitter fitter);

        /// <summary>
        /// Applies the values of the passed along Fitter to
        /// the corresponding DB entry.
        /// </summary>
        /// <param name="fitter"></param>
        /// <returns></returns>
        Task Edit(Fitter fitter);

        /// <summary>
        /// Removes the Fitter with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);

        /// <summary>
        /// Checks if all Fitter in the list have a unique name.
        /// Afterwards removes all current Fitters from 
        /// the DB and adds those from the list.
        /// </summary>
        /// <param name="fitters"></param>
        /// <returns></returns>
        Task Import(List<Fitter> fitters);

        /// <summary>
        /// returns the ColorCode of the Fitter with the given Name
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        string GetColorCode(string Name);
    }
}