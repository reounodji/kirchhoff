using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Data.Entities
{
    public class UnknownFitter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        public DateTime FirstAppereance { get; set; }

        public int NumberOfAppereances { get; set; }
    }
}
