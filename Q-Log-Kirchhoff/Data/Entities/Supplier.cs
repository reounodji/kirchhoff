using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Data.Entities
{
    public class Supplier
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string ColorCode { get; set; }

        public string Name { get; set; }
    }
}
