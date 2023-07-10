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
    public class CustomerVM
    {

        [ValidateNever]
        public Customer Customer { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CityList { get; set; }


        [ValidateNever]
        public IEnumerable<SelectListItem> CountryList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> StateList { get; set; }


    }
}
