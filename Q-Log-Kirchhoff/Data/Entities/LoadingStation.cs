using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Data.Entities
{
    public class LoadingStation
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool ShowAll { get; set; }
    }
}
