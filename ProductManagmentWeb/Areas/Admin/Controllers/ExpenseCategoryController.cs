using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Data;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using ProductManagment_Models.ViewModels;
using System.Data;
using System.Drawing.Drawing2D;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ExpenseCategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public ExpenseCategoryController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        #region Index
        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();



            ExpenseCategoryIndexVM expenseCategoryIndexVM = new ExpenseCategoryIndexVM();
            expenseCategoryIndexVM.ExpenseCategoryNameSortOrder = string.IsNullOrEmpty(orderBy) ? "expenseCategoryName_desc" : "";
            var expenseCategories = (from data in _unitOfWork.ExpenseCategory.GetAll().ToList()
                                     where term == "" ||
                                        data.ExpenseCategoryName.ToLower().
                                        Contains(term)


                                     select new ExpenseCategory
                                     {
                                         Id = data.Id,
                                         ExpenseCategoryName = data.ExpenseCategoryName,
                                         IsActive = data.IsActive,
                                     });

            switch (orderBy)
            {
                case "stateName_desc":
                    expenseCategories = expenseCategories.OrderByDescending(a => a.ExpenseCategoryName);
                    break;

                default:
                    expenseCategories = expenseCategories.OrderBy(a => a.ExpenseCategoryName);
                    break;
            }
            int totalRecords = expenseCategories.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            expenseCategories = expenseCategories.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            expenseCategoryIndexVM.ExpenseCategories = expenseCategories;
            expenseCategoryIndexVM.CurrentPage = currentPage;
            expenseCategoryIndexVM.TotalPages = totalPages;
            expenseCategoryIndexVM.Term = term;
            expenseCategoryIndexVM.PageSize = pageSize;
            expenseCategoryIndexVM.OrderBy = orderBy;
            return View(expenseCategoryIndexVM);
        }
        #endregion

        #region Upsert
        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new ExpenseCategory());
            }
            else
            {
                //update
                ExpenseCategory expenseCategory = _unitOfWork.ExpenseCategory.Get(u => u.Id == id);
                return View(expenseCategory);
            }

        }

        [HttpPost]
        public IActionResult Upsert(ExpenseCategory expenseCategory)
        {
            if (ModelState.IsValid)
            {




                if (expenseCategory.Id == 0)
                {
                    try
                    {


                        ExpenseCategory expenseCategoryobj = _unitOfWork.ExpenseCategory.Get(u => u.ExpenseCategoryName == expenseCategory.ExpenseCategoryName);
                        if (expenseCategoryobj != null)
                        {
                            TempData["error"] = "ExpenseCategory Name Already Exist!";
                        }
                        else
                        {
                            _unitOfWork.ExpenseCategory.Add(expenseCategory);
                            _unitOfWork.Save();
                            TempData["success"] = "ExpenseCategory created successfully";
                        }

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
                    try
                    {


                        ExpenseCategory expenseCategoryobj = _unitOfWork.ExpenseCategory.Get(u => u.Id != expenseCategory.Id && u.ExpenseCategoryName == expenseCategory.ExpenseCategoryName);
                        if (expenseCategoryobj != null)
                        {
                            TempData["error"] = "ExpenseCategory Already Exist!";
                        }
                        else
                        {
                            _unitOfWork.ExpenseCategory.Update(expenseCategory);
                            _unitOfWork.Save();
                            TempData["success"] = "ExpenseCategory Updated successfully";
                        }
                    }
                    catch (Exception ex)
                    {
                        LogErrorToDatabase(ex);

                        TempData["error"] = "error accured";
                        // return View(brand);
                        return RedirectToAction("Error", "Home");
                    }

                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(expenseCategory);
            }


        }

        #endregion
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
            List<ExpenseCategory> objExpenseCategoryList = _unitOfWork.ExpenseCategory.GetAll().ToList();
            return Json(new { data = objExpenseCategoryList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            try
            {


                var ExpenseCategoryToBeDeleted = _unitOfWork.ExpenseCategory.Get(u => u.Id == id);
                if (ExpenseCategoryToBeDeleted == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }

                _unitOfWork.ExpenseCategory.Remove(ExpenseCategoryToBeDeleted);
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
