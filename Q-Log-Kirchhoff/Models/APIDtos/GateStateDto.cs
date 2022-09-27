using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models.APIDtos
{
    /// <summary>
    /// Used for the Gate APIs
    /// </summary>
    public class GateStateDto
    {
        public string Name { get; set; }

        public bool IsOccupied { get; set; }
    }
}
