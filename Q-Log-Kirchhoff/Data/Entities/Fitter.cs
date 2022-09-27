using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Data.Entities
{
    public class Fitter
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        public string ColorCode { get; set; }
    }
}


