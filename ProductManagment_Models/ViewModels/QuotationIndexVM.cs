using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagment_Models.Models;

namespace ProductManagment_Models.ViewModels
{
    public class QuotationIndexVM
    {

        // public IQueryable<State> states { get; set; }

        public IEnumerable<Quotation> Quotations { get; set; }
        public string SupplierNameSortOrder { get; set; }
        // public string EmailSortOrder { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Term { get; set; }
        public string OrderBy { get; set; }

    }
}
