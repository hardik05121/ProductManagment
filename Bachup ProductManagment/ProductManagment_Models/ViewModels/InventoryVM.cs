using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagment_Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagment_Models.ViewModels
{
    public class InventoryVM
    {
        public Inventory Inventory { get; set; }
       // public Product Product{ get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ProductList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> UnitList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> WarehouseList { get; set; }

    }
}
