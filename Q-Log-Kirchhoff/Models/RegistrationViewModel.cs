using Microsoft.AspNetCore.Mvc.Rendering;
using MVC.Data.Entities;
using MVC.Data.Enums;
using System.Collections.Generic;

namespace MVC.Models
{
    public class RegistrationViewModel
    {
        public int TimePerLanguage { get; set; }

        public int TimeTillReset { get; set; }

        public bool CompanyUnknown { get; set; }

        public string SelectedLanguage { get; set; }


        public bool Edit { get; set; }

        public string LicensePlate { get; set; }

        public string Firstname { get; set; }

        public string Lastname { get; set; }

        public string Forwarder { get; set; }

        public string Customer { get; set; }


        public string Phonenumber { get; set; }

        public int Targets { get; set; }

        public bool GoodsReceiptdelivery { get; set; }

        public bool GoodsReceiptCustomerEmpties { get; set; }

        public bool LoadCustomerPickup { get; set; }

        public bool LoadEmptiesCollection { get; set; }


        public string PostalCode { get; set; }

        public int NumberOfPeople { get; set; }

        public bool IsSmallVehicle { get; set; }

        public string SupplierNumber { get; set; }
        
        public EApproachTyp ApproachTyp { get; set; }

        public string CompanyName { get; set; }
        
        public string LoadReference { get; set; }
        
        public string LoadingStation { get; set; }

        public List<LoadingStation> LoadingStations { get; set; }

    }
}
