using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Data.Entities
{
    public class BarrierControlSettings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public bool UseBarrierControl { get; set; }

        public string EntryBarrierAPIUrl { get; set; }

        public string ExitBarrierAPIUrl { get; set; }
    }
}
