using MVC.Data.Entities;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Interfaces
{
    public interface IERPSender
    {
        /// <summary>
        /// Send the registration with the given ID to a ERP as JSON
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task<bool> SendRegistrationToERP(Registration ID, string loadingStation = null);

        /// <summary>
        /// GetSupplierFromERPandUpdate
        /// </summary>
        /// <returns></returns>
        Task<bool> GetSuppliersFromERP();
    }
}
