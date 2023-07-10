using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagment_Models.Models;

namespace ProductManagment_Models.ViewModels
{
    public class UnitIndexVM
    {

        // public IQueryable<State> states { get; set; }

        public IEnumerable<Unit> Units { get; set; }
        public string NameSortOrder { get; set; }
        // public string EmailSortOrder { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Term { get; set; }
        public string OrderBy { get; set; }

    }
}
