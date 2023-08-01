using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagment_DataAccess.Data;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using ProductManagment_Models.ViewModels;
using System.Data;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize]
    public class ExpenseController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _db;

        public ExpenseController(IUnitOfWork unitOfWork, ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _db = db;
        }


        #region Index
        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();



            ExpenseIndexVM expenseIndexVM = new ExpenseIndexVM();
            expenseIndexVM.ExpenseNameSortOrder = string.IsNullOrEmpty(orderBy) ? "expenseName_desc" : "";
            var expenses = (from data in _unitOfWork.Expense.GetAll(includeProperties: "ExpenseCategory").ToList()
                            where term == "" || data.ExpenseCategory.ExpenseCategoryName.ToLower().Contains(term)

                            select new Expense
                            {
                                Id = data.Id,
                                CreatedDate = data.CreatedDate,
                                ExpenseDate = data.ExpenseDate,
                                Reference = data.Reference,
                                Amount = data.Amount,
                                ExpenseCategory = data.ExpenseCategory,
                                Note = data.Note,
                                ExpenseFile = data.ExpenseFile,
                            });

            switch (orderBy)
            {
                case "expenseName_desc":
                    expenses = expenses.OrderByDescending(a => a.ExpenseCategory.ExpenseCategoryName);
                    break;

                default:
                    expenses = expenses.OrderBy(a => a.ExpenseCategory.ExpenseCategoryName);
                    break;
            }
            int totalRecords = expenses.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            expenses = expenses.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            expenseIndexVM.Expenses = expenses;
            expenseIndexVM.CurrentPage = currentPage;
            expenseIndexVM.TotalPages = totalPages;
            expenseIndexVM.Term = term;
            expenseIndexVM.PageSize = pageSize;
            expenseIndexVM.OrderBy = orderBy;
            return View(expenseIndexVM);
        }
        #endregion

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            ExpenseVM ExpenseVM = new()
            {
                ExpenseCategoryList = _unitOfWork.ExpenseCategory.GetAll().Select(u => new SelectListItem
                {
                    Text = u.ExpenseCategoryName,
                    Value = u.Id.ToString()
                }),

                //UserList = _unitOfWork.User.GetAll().Select(u => new SelectListItem
                //{
                //    Text = u.FirstName,
                //    Value = u.Id.ToString()
                //}),
                Expense = new Expense()
            };
            if (id == null || id == 0)
            {
                //create
                return View(ExpenseVM);
            }
            else
            {
                //update
                ExpenseVM.Expense = _unitOfWork.Expense.Get(u => u.Id == id);
                return View(ExpenseVM);
            }

        }

        [HttpPost]



        public IActionResult Upsert(ExpenseVM ExpenseVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = Path.Combine(wwwRootPath, @"images\expense");

                        if (!string.IsNullOrEmpty((string?)ExpenseVM.Expense.ExpenseFile))
                        {
                            //delete the old image
                            var oldImagePath =
                                        Path.Combine(wwwRootPath, (string)ExpenseVM.Expense.ExpenseFile.TrimStart('\\'));

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        ExpenseVM.Expense.ExpenseFile = @"\images\expense\" + fileName;
                    }
                }
                catch (Exception ex)
                {
                    LogErrorToDatabase(ex);

                    TempData["error"] = "error accured";
                    // return View(brand);
                    return RedirectToAction("Error", "Home");
                }
                try
                {


                    if (ExpenseVM.Expense.Id == 0)
                    {
                        _unitOfWork.Expense.Add(ExpenseVM.Expense);
                    }
                    else
                    {
                        _unitOfWork.Expense.Update(ExpenseVM.Expense);
                    }

                    _unitOfWork.Save();
                    TempData["success"] = "Expense created successfully";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    LogErrorToDatabase(ex);

                    TempData["error"] = "error accured";
                    // return View(brand);
                    return RedirectToAction("Error", "Home");
                }
            }
            else
            {

                return View(ExpenseVM);
            }
        }
        private void LogErrorToDatabase(Exception ex)
        {
            var error = new ErrorLog
            {
                ErrorMessage = ex.Message,
                //  StackTrace = ex.StackTrace,
                ErrorDate = DateTime.Now
            };

            _db.ErrorLogs.Add(error);
            _db.SaveChanges();
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Expense> objExpenseList = _unitOfWork.Expense.GetAll(includeProperties: "ExpenseCategory").ToList();
            return Json(new { data = objExpenseList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            try
            {


                var productToBeDeleted = _unitOfWork.Expense.Get(u => u.Id == id);
                if (productToBeDeleted == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }

                var oldImagePath =
                               Path.Combine(_webHostEnvironment.WebRootPath,
                               productToBeDeleted.ExpenseFile.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                _unitOfWork.Expense.Remove(productToBeDeleted);
                _unitOfWork.Save();

                return Json(new { success = true, message = "Delete Successful" });
            }
            catch (Exception ex)
            {
                LogErrorToDatabase(ex);

                TempData["error"] = "error accured";
                // return View(brand);
                return RedirectToAction("Error", "Home");
            }
        }


        #endregion


    }
}
