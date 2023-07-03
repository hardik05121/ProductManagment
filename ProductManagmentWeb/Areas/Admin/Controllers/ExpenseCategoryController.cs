using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using System.Drawing.Drawing2D;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ExpenseCategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public ExpenseCategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Index
        public IActionResult Index()
        {
            List<ExpenseCategory> objExpenseCategoryList = _unitOfWork.ExpenseCategory.GetAll().ToList();

            return View(objExpenseCategoryList);
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
                else
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

                return RedirectToAction("Index");
            }
            else
            {
                return View(expenseCategory); 
            }

         
        }

        #endregion

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
            var ExpenseCategoryToBeDeleted = _unitOfWork.ExpenseCategory.Get(u => u.Id == id);
            if (ExpenseCategoryToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.ExpenseCategory.Remove(ExpenseCategoryToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
