using System.ComponentModel.DataAnnotations.Schema;

namespace MVC.Data.Entities
{
    /// <summary>
    /// The Authorization group describes which permissions the users of this group have.
    /// </summary>
    public class AuthorizationGroup
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        public string Name { get; set; }
        public string ADGroupName { get; set; }

        // Processing
        public bool CanUseProcessingList { get; set; }
        public bool CanModifyProcessingList { get; set; }
        public bool CanSetLoadingStation { get; set; }
        public bool CanSetGate { get; set; }
        public bool CanSetRelease { get; set; }
        public bool CanSetCall { get; set; }
        public bool CanSetEntrance { get; set; }
        public bool CanSetProcessStart { get; set; }
        public bool CanSetProcessEnd { get; set; }
        public bool CanSetExit { get; set; }
        public bool CanProcessLoadingList { get; set; }
        public bool CanProcessDeliveryList { get; set; }

        // History
        public bool CanUseHistory { get; set; }
        public bool CanExportHistory { get; set; }


        // Config
        public bool CanUseConfig { get; set; }
        public bool CanModifyAllSettings { get; set; }
        public bool CanInspectApproachTyps { get; set; }
        public bool CanModifyApproachTyps { get; set; }
        public bool CanInspectUnknownApproachTyps { get; set; }
        public bool CanModifyUnknownApproachTyps { get; set; }

    }
}
