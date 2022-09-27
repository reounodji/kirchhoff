using MVC.Data.Entities;
using MVC.Models.ConfigurationViewModels;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface IGeneralSettingsRepository
    {
        /// <summary>
        /// Loads the GeneralSettings from the db.
        /// Will always load the first entry and there should always be exactly 1 entry.
        /// </summary>
        /// <returns></returns>
        GeneralSettings GetGeneralSettings();

        Task SetGeneralSettings(GeneralSettingsViewModel model);

    }
}
