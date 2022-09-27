using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Data.Entities
{
    public class Gate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string LoadingStation { get; set; }

        public bool IsOccupied { get; set; }
    }
}
