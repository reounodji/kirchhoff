using MVC.Data.Entities;
using System.Collections.Generic;

namespace MVC.Models.ConfigurationViewModels
{
    public class GateViewModel
    {
        public Gate Gate { get; set; }

        public List<LoadingStation> LoadingStations { get; set; }
    }
}
