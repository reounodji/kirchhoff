using MVC.Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Interfaces
{
    public interface ICompanyFacade
    {
        Task<string> ChangeCompany(int ID, string Name);
    }
}
