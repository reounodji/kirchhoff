using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Data.Entities
{
    public class SMSSettings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public bool UseSMSService { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public string AccountReference { get; set; }

    }
}
