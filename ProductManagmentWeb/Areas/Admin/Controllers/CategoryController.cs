using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using System.Data;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            List<CategoryMetadata> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return View(objCategoryList);
        }




        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new CategoryMetadata());
            }
            else
            {
                //update
                CategoryMetadata category = _unitOfWork.Category.Get(u => u.Id == id);
                return View(category);
            }

        }

        [HttpPost]
        public IActionResult Upsert(CategoryMetadata category)
        {
            if (ModelState.IsValid)
            {

                if (category.Id == 0)
                {
                    _unitOfWork.Category.Add(category);
                    _unitOfWork.Save();
                    TempData["success"] = "Category created successfully";
                }
                else
                {
                    _unitOfWork.Category.Update(category);
                    _unitOfWork.Save();
                    TempData["success"] = "Category Updated successfully";
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(category);
            }
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CategoryMetadata> objCategoryList = _unitOfWork.Category.GetAll().ToList();
            return Json(new { data = objCategoryList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CategoryToBeDeleted = _unitOfWork.Category.Get(u => u.Id == id);
            if (CategoryToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Category.Remove(CategoryToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
