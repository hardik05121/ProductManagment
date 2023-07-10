using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using ProductManagment_Models.ViewModels;
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


        //public IActionResult Index()
        //{
        //    List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
        //    return View(objCategoryList);
        //}
        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

        

            CategoryIndexVM categoryIndexVM = new CategoryIndexVM();
            categoryIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "Name_desc" : "";
            var categories = (from data in _unitOfWork.Category.GetAll().ToList()
                          where term == "" ||
                             data.Name.ToLower().
                             Contains(term) 


                          select new Category
                          {
                              Id = data.Id,
                              Name = data.Name,
                              Description = data.Description,
                              IsActive = data.IsActive
                            
                          });
            switch (orderBy)
            {
                case "Name_desc":
                    categories = categories.OrderByDescending(a => a.Name);
                    break;

                default:
                    categories = categories.OrderBy(a => a.Name);
                    break;
            }
            int totalRecords = categories.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            categories = categories.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            categoryIndexVM.Categories = categories;
            categoryIndexVM.CurrentPage = currentPage;
            categoryIndexVM.TotalPages = totalPages;
            categoryIndexVM.Term = term;
            categoryIndexVM.PageSize = pageSize;
            categoryIndexVM.OrderBy = orderBy;
            return View(categoryIndexVM);


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
                Category category = _unitOfWork.Category.Get(u => u.Id == id);
                return View(category);
            }

        }

        [HttpPost]
        public IActionResult Upsert(Category category)
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            Category categoryToBeDeleted = _unitOfWork.Category.Get(u => u.Id == id);
            if (categoryToBeDeleted == null)
            {
                TempData["error"] = "Category can't be Delete.";
                return RedirectToAction("Index");
            }

            _unitOfWork.Category.Remove(categoryToBeDeleted);
            _unitOfWork.Save();
            TempData["success"] = "Category Deleted successfully";
            return RedirectToAction("Index");

        }
        //#region API CALLS

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
        //    return Json(new { data = objCategoryList });
        //}


        //[HttpDelete]
        //public IActionResult Delete(int? id)
        //{
        //    var CategoryToBeDeleted = _unitOfWork.Category.Get(u => u.Id == id);
        //    if (CategoryToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }

        //    _unitOfWork.Category.Remove(CategoryToBeDeleted);
        //    _unitOfWork.Save();

        //    return Json(new { success = true, message = "Delete Successful" });
        //}

        //#endregion
    }
}
