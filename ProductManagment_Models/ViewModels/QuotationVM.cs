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
    public class QuotationVM
    {

        public Quotation Quotation { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> SupplierList { get; set; }

        //[ValidateNever]
        //public IEnumerable<Product> ProducttList { get; set; }

    }
}
