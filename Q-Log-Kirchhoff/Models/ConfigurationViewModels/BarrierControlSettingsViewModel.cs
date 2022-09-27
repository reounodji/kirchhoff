using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models.ConfigurationViewModels
{
    public class BarrierControlSettingsViewModel
    {
        public bool UseBarrierControl { get; set; }

        public string EntryBarrierAPIUrl { get; set; }

        public string ExitBarrierAPIUrl { get; set; }
    }
}
