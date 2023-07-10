using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagment_Models.Models;

namespace ProductManagment_Models.ViewModels
{
    public class StateVM
    {

        public  State State { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> CountryList { get; set; }
    }
}
