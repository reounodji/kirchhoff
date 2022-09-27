using MVC.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MVC.Models.ConfigurationViewModels
{
    public class SupplierViewModel
    {
        public Supplier Supplier { get; set; }

        public List<SupplierNumber> SupplierNumbers { get; set; }

        public string Numbers { get; set; }

        public string OldName { get; set; }
    }
}
