using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Repositories.Interfaces
{
    public interface ILocalizationRepository
    {
        Dictionary<string, string> Get(string name);

        string GetSMSMessage(string languageCode);
    }
}
