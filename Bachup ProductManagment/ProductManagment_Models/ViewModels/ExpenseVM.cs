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
    public class ExpenseVM
    {

        public Expense Expense { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> ExpenseCategoryList { get; set; }

        //[ValidateNever]
        //public IEnumerable<SelectListItem> UserList { get; set; }
    }
}
