using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Data.Entities
{
    /// <summary>
    /// Represents those Registrations, that are currently in motion.
    /// </summary>
    public class OpenRegistration : Registration, IEquatable<OpenRegistration>
    {

        public OpenRegistration() : base()
        {
        }

        public bool Equals(OpenRegistration other)
        {
            return this.LicensePlate == other.LicensePlate && this.TimeOfRegistration == other.TimeOfRegistration && this.ApproachTyp == other.ApproachTyp && this.CompanyName == other.CompanyName && this.LoadReference == other.LoadReference;
        }
    }
}
