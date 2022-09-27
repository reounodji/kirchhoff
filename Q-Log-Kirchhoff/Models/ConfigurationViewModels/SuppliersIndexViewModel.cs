using MVC.Data.Entities;
using System.Collections.Generic;

namespace MVC.Models.ConfigurationViewModels
{
    public class SuppliersIndexViewModel
    {
        public List<Supplier> Suppliers { get; set; }

        public List<SupplierNumber> SupplierNumbers { get; set; }
    }
}
