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
    public class ProductVM
    {
  
        public Product Product { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> BrandList { get; set; } 
        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }  
        [ValidateNever]
        public IEnumerable<SelectListItem> UnitList { get; set; } 
        [ValidateNever]
        public IEnumerable<SelectListItem> WarehouseList { get; set; }  
        [ValidateNever]
        public IEnumerable<SelectListItem> TaxList { get; set; } 
    }
}
