using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Data.Entities
{
    public class UnknownSupplier
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime FirstAppereance { get; set; }

        public int NumberOfAppereances { get; set; }
    }
}
