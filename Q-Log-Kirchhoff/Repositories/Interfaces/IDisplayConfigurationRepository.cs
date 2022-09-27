using MVC.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface IDisplayConfigurationRepository
    {
        /// <summary>
        /// Gets all DisplayConfigurations from the DB and returns them as a list.
        /// </summary>
        /// <returns></returns>
        List<DisplayConfiguration> GetAll();

        /// <summary>
        /// Adds the given DisplayConfiguration to the DB.
        /// </summary>
        /// <param name="displayConfiguration"></param>
        /// <returns></returns>
        Task Add(DisplayConfiguration displayConfiguration);

        /// <summary>
        /// Removes the DisplayConfiguration with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(int id);

        /// <summary>
        /// Applies the values of the passed along Configuration 
        /// to the corresponding DB entry.
        /// </summary>
        /// <param name="displayConfiguration"></param>
        /// <returns></returns>
        Task EditAsync(DisplayConfiguration displayConfiguration);

        void Edit(DisplayConfiguration displayConfiguration);

        /// <summary>
        /// Returns the DisplayConfiguration with the given id.
        /// Will return null if no such entry can be found.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        DisplayConfiguration Get(int id);
    }
}
