using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public ExpenseController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            List<Expense> objExpenseList = _unitOfWork.Expense.GetAll(includeProperties: "ExpenseCategory").ToList();
            return View(objExpenseList);
        }

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

        // jayre image levani hoy tyre IFormFile? file Parameter lai levnu

        public IActionResult Upsert(ExpenseVM ExpenseVM, IFormFile? file)
        {
            if (ModelState.IsValid)
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

                if (ExpenseVM.Expense.Id == 0)
                {
                    _unitOfWork.Expense.Add(ExpenseVM.Expense);
                }
                else
                {
                    _unitOfWork.Expense.Add(ExpenseVM.Expense);
                }

                _unitOfWork.Save();
                TempData["success"] = "Expense created successfully";
                return RedirectToAction("Index");
            }
            else
            {

                return View(ExpenseVM);
            }
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


        #endregion


    }
}
