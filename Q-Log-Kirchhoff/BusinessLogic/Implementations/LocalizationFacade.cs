using MVC.BusinessLogic.Interfaces;
using MVC.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.BusinessLogic.Implementations
{
    public class LocalizationFacade : ILocalizationFacade
    {
        private readonly ILocalizationRepository _localizationRepository;

        public LocalizationFacade(ILocalizationRepository localizationRepository)
        {
            _localizationRepository = localizationRepository;
        }

        public Dictionary<string, string> Get(CultureInfo cultureInfo)
        {
            return _localizationRepository.Get(cultureInfo.TwoLetterISOLanguageName);
        }


    }
}
