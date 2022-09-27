using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Data.Entities
{
    /// <summary>
    /// Contains settings, that dont really fit anywhere else.
    /// There will be exactly 1 object of this type in the DB.
    /// </summary>
    public class GeneralSettings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        //Processing
        public int RegistrationTimeThreshold { get; set; }

        public string ExceededWaitTimeColorCode { get; set; }

        public string RecentChangeColorCode { get; set; }

        public string HoverColorCode { get; set; }

        public string NewEntryColorCode { get; set; }

        public string ExitColorCode { get; set; }

        // History
        public int DefaultHistoryTimespan { get; set; }     // the default timespan is always between today and DefaultHistoryTimespan days before

        // Displays
        public int DisplayUpdateInterval { get; set; }
   }
}
