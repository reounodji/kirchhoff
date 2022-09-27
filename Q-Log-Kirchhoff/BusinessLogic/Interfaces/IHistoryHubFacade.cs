using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Interfaces
{
    interface IHistoryHubFacade
    {
        /// <summary>
        /// resends a closed Registration to ERP
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task ResendToERP(int id);

    }
}
