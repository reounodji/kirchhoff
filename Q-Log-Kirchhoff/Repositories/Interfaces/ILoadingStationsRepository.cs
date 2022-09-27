using MVC.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface ILoadingStationsRepository
    {
        /// <summary>
        /// Returns a list of all LoadingStations from the DB.
        /// </summary>
        /// <returns></returns>
        List<LoadingStation> GetAll();

        /// <summary>
        /// Adds the passed along LoadingStation to the DB.
        /// </summary>
        /// <param name="LoadingStation"></param>
        /// <returns></returns>
        Task Add(LoadingStation LoadingStation);

        /// <summary>
        /// Removes the LoadingStation with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);

        /// <summary>
        /// Gets the LoadingStation with the given id.
        /// Will return null if no such LoadingStation could be found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        LoadingStation Get(int id);


        LoadingStation Get(string name);

        /// <summary>
        /// Gets the corresponding DB entry for the param LoadingStation and
        /// sets the entry's values to match the LoadingStation's
        /// </summary>
        /// <param name="LoadingStation"></param>
        /// <returns></returns>
        Task Set(LoadingStation LoadingStation);

        /// <summary>
        /// Checks if all LoadingStations in the list have unique names.
        /// Afterwards removes all LoadingStations from the DB and adds the list of LoadingStations to the db.
        /// </summary>
        /// <param name="LoadingStations"></param>
        /// <returns></returns>
        Task Import(List<LoadingStation> LoadingStations);
    }
}
