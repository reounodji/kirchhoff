using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Data.Entities
{
    /// <summary>
    /// Each physical display is described by one DisplayConfiguration.
    /// It provides all the Information necessary to communicate with the Display 
    /// and show the correct amount of registrations.
    /// </summary>
    public class DisplayConfiguration
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        #region tcp communication
        public string IPAddress { get; set; }
        public int Port { get; set; }
        public int TcpTimeoutInMs { get; set; }
        // wait this time after sending data, to receive answer or possibly swap protocol
        public int ModeBreakInMs { get; set; }

        // ´what index does the first shown entry currently have. This is used for paging
        public int curDisplayedStartingIndex { get; set; }
        #endregion

        #region general settings
        public string Name { get; set; }
        public EDisplayType Type { get; set; }
        public int Rows { get; set; }
        public int CharsPerLine { get; set; }
        #endregion

        public EEntryRemovalType EntryRemovalType { get; set; }

    }
}
