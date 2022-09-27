using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models.APIDtos
{
    public class ERPRegistrationDTO
    {

        // nur 'lieferanten', 'spedition', 'zusteller', 'monteure' erlaubt
        public string lade_typ { get; set; }

        public string kennzeichen { get; set; }

        //'< 7,5t', '> 7,5t'
        public string gewichtsklasse { get; set; }

        public string name_firma { get; set; }

        public string name_ansprechpartner { get; set; }

        public int anz_personen { get; set; }

        //format: 2019-12-13T13:30:28Z 
        public string zpt_anmeldung { get; set; }

        //format: 2019-12-13T13:30:28Z 
        public string zpt_freigabe { get; set; }

        public string lieferanten { get; set; }

        public string lade_referenz { get; set; }

        public string ladestelle { get; set; }

        public string anliefer_tor { get; set; }

    }
}
