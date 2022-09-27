using MVC.Data.Entities;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface IADSettingsRepository
    {
        /// <summary>
        /// Gets the first settingsobject from the db.
        /// the first one is seeded when creating the db. 
        /// there should always only be 1 ADSettings object in the db.
        /// </summary>
        /// <returns></returns>
        ADSettings Get();

        /// <summary>
        /// Applies the settings to the DB
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        Task Set(ADSettings settings);
    }
}
