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
    public class NewQuotationVM
    {
        public Quotation Quotation { get; set; }
        [ValidateNever]
        public List<QuotationXproduct> QuotationXproducts { get; set; } = new List<QuotationXproduct>();   
        public QuotationXproduct QuotationXproduct { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> SupplierList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ProductList { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> UserList { get; set; } 
        [ValidateNever]
        public IEnumerable<SelectListItem> WarehouseList { get; set; } 
        [ValidateNever]
        public IEnumerable<SelectListItem> UnitList { get; set; } 
        [ValidateNever]
        public IEnumerable<SelectListItem> TaxList { get; set; } 

    }
}
