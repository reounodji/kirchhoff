using MVC.Data.Entities;
using System;
using System.Collections.Generic;

namespace MVC.Models
{
    public class HistoryViewModel
    {
        public List<ClosedRegistration> Registrations { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string HoverColorCode { get; set; }

    }
}
