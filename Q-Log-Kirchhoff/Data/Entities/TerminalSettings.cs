using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Data.Entities
{
    public class TerminalSettings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        /// <summary>
        /// how many seconds is each language shown at the language selection page
        /// </summary>
        public int TimePerLanguage { get; set; }

        /// <summary>
        /// after how many seconds of inactivity will the screen jump back
        /// </summary>
        public int TimeTillReset { get; set; }


    }
}
