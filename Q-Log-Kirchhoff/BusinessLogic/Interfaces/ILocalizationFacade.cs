using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Interfaces
{
    public interface ILocalizationFacade
    {
        Dictionary<string, string> Get(CultureInfo cultureInfo);
    }
}
