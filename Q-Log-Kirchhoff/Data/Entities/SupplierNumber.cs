using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Data.Entities
{
    public class SupplierNumber
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [JsonProperty("kurzben")]
        public string SupplierName { get; set; }

        [JsonProperty("lief_nr")]
        public string Number { get; set; }
    }
}
