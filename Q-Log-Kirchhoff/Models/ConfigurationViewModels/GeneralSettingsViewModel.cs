using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models.ConfigurationViewModels
{
    public class GeneralSettingsViewModel
    {
        public int RegistrationTimeThreshold { get; set; }

        public int RecencyThresholdDays { get; set; }

        public int RecencyThresholdHours { get; set; }

        public int DefaultHistoryTimespan { get; set; }

        public int UpdateDisplayInterval { get; set; }

        public char CompressChar { get; set; }

        public string ExceededWaitTimeColorCode { get; set; }

        public string RecentChangeColorCode { get; set; }

        public string HoverColorCode { get; set; }

        public string NewEntryColorCode { get; set; }

        public string ExitColorCode { get; set; }
    }
}
