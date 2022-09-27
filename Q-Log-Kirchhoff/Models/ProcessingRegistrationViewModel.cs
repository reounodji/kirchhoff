using MVC.Data.Entities;
using System.Collections.Generic;

namespace MVC.Models
{
    public class ProcessingRegistrationViewModel
    {
        public OpenRegistration Registration { get; set; }

        public List<Gate> Gates { get; set; }

        public List<LoadingStation> LoadingStations { get; set; }
    }
}
