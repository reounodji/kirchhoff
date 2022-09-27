using System.ComponentModel.DataAnnotations;

namespace MVC.Data.Entities
{
    /// <summary>
    /// Pendant to the OpenRegistration.
    /// This is used to have a clear seperation between Registrations
    /// that are still in progress and those that have been completed.
    /// </summary>
    public class ClosedRegistration : Registration
    {
        public int OpenRegistID { get; set; }

        public ClosedRegistration() : base()
        {
        }

        /// <summary>
        /// Constructor that takes the informations from the openRegistration wich was just completed and sets the values of this object.
        /// </summary>
        /// <param name="openRegist"></param>
        public ClosedRegistration(OpenRegistration openRegist)
        {
            this.Gate = openRegist.Gate;
            this.OpenRegistID = openRegist.ID;
            this.IsSmallVehicle = openRegist.IsSmallVehicle;
            this.Language = openRegist.Language;
            this.LicensePlate = openRegist.LicensePlate;
            this.CompressedLicensePlate = openRegist.CompressedLicensePlate;
            this.NumberOfPeople = openRegist.NumberOfPeople;
            this.ProcessEnd = openRegist.ProcessEnd;
            this.ProcessStart = openRegist.ProcessStart;
            this.TimeOfCall = openRegist.TimeOfCall;
            this.TimeOfEntry = openRegist.TimeOfEntry;
            this.TimeOfExit = openRegist.TimeOfExit;
            this.TimeOfRegistration = openRegist.TimeOfRegistration;
            this.ColorCode = openRegist.ColorCode;
            this.ApproachTyp = openRegist.ApproachTyp;
            this.LoadingStation = openRegist.LoadingStation;
            this.CompanyName = openRegist.CompanyName;
            this.LoadReference = openRegist.LoadReference;
            this.TimeOfRelease = openRegist.TimeOfRelease;
            this.Comment = openRegist.Comment;
            this.SupplierNumber = openRegist.SupplierNumber;
            this.WasSendingSuccessful = openRegist.WasSendingSuccessful;

            this.FirstName = openRegist.FirstName;
            this.Lastname = openRegist.Lastname;
            this.Customer = openRegist.Customer;
            this.Phonenumber = openRegist.Phonenumber;
            this.Forwarder = openRegist.Forwarder;
            this.GoodsReceiptCustomerEmpties = openRegist.GoodsReceiptCustomerEmpties;
            this.GoodsReceiptdelivery = openRegist.GoodsReceiptdelivery;
            this.LoadEmptiesCollection = openRegist.LoadEmptiesCollection;
            this.LoadCustomerPickup = openRegist.LoadCustomerPickup;
            this.StatusCall = openRegist.StatusCall;
         

        }
    }
}
