using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MVC.Data.Enums;

namespace MVC.Data.Entities
{
    public abstract class Registration
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int ID { get; set; }   // id

        public string LicensePlate { get; set; } //vehicle_number

        public string FirstName { get; set; }

        public string Lastname { get; set; }

        public string Gate { get; set; }

        public string Phonenumber { get; set; }  //phone_number

        public string Customer { get; set; }  // KIRCHOFF  DAIMLER

        public string Forwarder { get; set; } // EX: DHL expedition

        public string Comment { get; set; } //notes

        public DateTime TimeOfRegistration { get; set; } //time_registration

        public DateTime TimeOfCall { get; set; } //time_call

        public DateTime? TimeOfDisplay { get; set; }  //time_display

        public DateTime? TimeOfClearance { get; set; } // time_clearance

        public DateTime? TimeOfInaction { get; set; } //time_inactive

        public int? StatusTarget { get; set; }

        public int? StatusYard { get; set; }

        public int? StatusCall { get; set; }

        public int? StatusInactive { get; set; }

        public bool GoodsReceiptdelivery { get; set; }

        public bool GoodsReceiptCustomerEmpties { get; set; }

        public bool LoadCustomerPickup { get; set; }

        public bool LoadEmptiesCollection { get; set; }


        public string CompressedLicensePlate { get; set; }

        public EApproachTyp ApproachTyp { get; set; }

        public bool IsSmallVehicle { get; set; }

        public string CompanyName { get; set; }

        public string SupplierNumber { get; set; }

        public string LoadReference { get; set; }

        public int NumberOfPeople { get; set; }

        public string LoadingStation { get; set; }

        public string ColorCode { get; set; }

        

        public bool WasSendingSuccessful { get; set; }


        public string Language { get; set; }

    

   

        public DateTime TimeOfRelease { get; set; }

    

        public DateTime TimeOfEntry { get; set; }

        public DateTime ProcessStart { get; set; }

        public DateTime ProcessEnd { get; set; }

        public DateTime TimeOfExit { get; set; }

    }
}
