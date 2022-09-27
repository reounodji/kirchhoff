using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Data.Entities
{
    /// <summary>
    /// All settings related to Active Directory.
    /// There will be exactly 1 object of this type in the DB
    /// </summary>
    public class ADSettings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public bool UseAD { get; set; }

        // if set to true any new user that visits the website and sends windows authentication, will 
        // lead to the creation of a new account in the user db. The Group will be determined by the 
        // ad groups of that user. this account can then be used to login without ad once the password is set.
        // also work when the user enters his AD information.
        public bool GenerateAccountsForNewADUsers { get; set; }

        public string ServerIP { get; set; }

        public string DomainNames { get; set; }

        public string DomainUserName { get; set; }

        public string DomainUserPassword { get; set; }
    }
}
