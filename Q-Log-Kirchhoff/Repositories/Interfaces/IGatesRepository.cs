using MVC.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface IGatesRepository
    {

        void Update(Gate gate);

        /// <summary>
        /// Returns a list of all gates from the DB.
        /// </summary>
        /// <returns></returns>
        List<Gate> GetAll();

        /// <summary>
        /// Adds the passed along Gate to the DB.
        /// </summary>
        /// <param name="gate"></param>
        /// <returns></returns>
        Task Add(Gate gate);

        /// <summary>
        /// Removes the Gate with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);

        /// <summary>
        /// Gets the Gate with the given id.
        /// Will return null if no such Gate could be found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Gate Get(int id);


        Gate Get(string name);

        /// <summary>
        /// Gets the corresponding DB entry for the param gate and
        /// sets the entry's values to match the gate's
        /// </summary>
        /// <param name="gate"></param>
        /// <returns></returns>
        Task Set(Gate gate);

        /// <summary>
        /// Checks if all gates in the list have unique names.
        /// Afterwards removes all gates from the DB and adds the list of gates to the db.
        /// </summary>
        /// <param name="gates"></param>
        /// <returns></returns>
        Task Import(List<Gate> gates);

        List<Gate> GetAllGatesFromLoadingStation(LoadingStation loadingStation);

        /// <summary>
        /// Gets the Loadingstation from the gate with the given name.
        /// </summary>
        /// <param name="gate"></param>
        /// <returns></returns>
        string GetLoadingStationFromGate(string gate);
    }
}
