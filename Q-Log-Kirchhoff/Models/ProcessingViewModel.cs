using MVC.Data.Entities;
using System.Collections.Generic;

namespace MVC.Models
{
    public class ProcessingViewModel
    {
        public List<OpenRegistration> Registrations { get; set; }

        public List<Gate> Gates { get; set; }

        public List<LoadingStation> LoadingStations { get; set; }

        public int maxWaitingTime { get; set; }

        public string ExceededWaitTimeColorCode { get; set; }

        public string RecentChangeColorCode { get; set; }

        public string HoverColorCode { get; set; }

        public string NewEntryColorCode { get; set; }

        public string ExitColorCode { get; set; }
    }
}
