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
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BrandController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        //#region Index
        //public IActionResult Index()
        //{
        //    List<Brand> objBrandList = _unitOfWork.Brand.GetAll().ToList();
        //    return View(objBrandList);
        //}
        //#endregion
        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

 


            BrandIndexVM brandIndexVM = new BrandIndexVM();
            brandIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "brandName_desc" : "";
            var brands = (from data in _unitOfWork.Brand.GetAll().ToList()
                          where term == "" ||
                             data.BrandName.ToLower().
                             Contains(term) 


                          select new Brand
                          {
                              Id = data.Id,
                              BrandName = data.BrandName,

                              BrandImage = data.BrandImage
                             
                          });
            switch (orderBy)
            {
                case "brandName_desc":
                    brands = brands.OrderByDescending(a => a.BrandName);
                    break;

                default:
                    brands = brands.OrderBy(a => a.BrandName);
                    break;
            }
            int totalRecords = brands.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            brands = brands.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            brandIndexVM.Brands = brands;
            brandIndexVM.CurrentPage = currentPage;
            brandIndexVM.TotalPages = totalPages;
            brandIndexVM.Term = term;
            brandIndexVM.PageSize = pageSize;
            brandIndexVM.OrderBy = orderBy;
            return View(brandIndexVM);


        }
        #region Upsert
        [HttpGet] // to grt the data on display.
        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new Brand());
            }
            else
            {
                //update
                Brand brandObj = _unitOfWork.Brand.Get(u => u.Id == id);
                return View(brandObj);
            }

        }


        [HttpPost]
        public IActionResult Upsert(Brand brand, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string brandPath = Path.Combine(wwwRootPath, @"images\brand");

                    if (!string.IsNullOrEmpty(brand.BrandImage))
                    {
                        //delete the old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath, brand.BrandImage.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(brandPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    brand.BrandImage = @"\images\brand\" + fileName;
                }



                if (brand.Id == 0)
                {
                    Brand brandObj = _unitOfWork.Brand.Get(u => u.BrandName == brand.BrandName);
                    if (brandObj != null)
                    {
                        TempData["error"] = "Brand Name Already Exist!";
                    }
                    else
                    {
                        _unitOfWork.Brand.Add(brand);
                        _unitOfWork.Save();
                        TempData["success"] = "Brand created successfully";
                    }
                }
                else
                {
                    Brand brandObj = _unitOfWork.Brand.Get(u => u.Id != brand.Id && u.BrandName == brand.BrandName);
                    if (brandObj != null)
                    {
                        TempData["error"] = "Brand Name Already Exist!";
                    }
                    else
                    {
                        _unitOfWork.Brand.Update(brand);
                        _unitOfWork.Save();
                        TempData["success"] = "Brand Updated successfully";
                    }

                }
;

                return RedirectToAction("Index");
            }
            else
            {
            
                return View(brand);
            }
        }
        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            Brand brandToBeDeleted = _unitOfWork.Brand.Get(u => u.Id == id);
            if (brandToBeDeleted == null)
            {
                TempData["error"] = "Brand can't be Delete.";
                return RedirectToAction("Index");
            }
            var oldImagePath =
                      Path.Combine(_webHostEnvironment.WebRootPath,
                       brandToBeDeleted.BrandImage.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Brand.Remove(brandToBeDeleted);
            _unitOfWork.Save();
            TempData["success"] = "Brand Deleted successfully";
            return RedirectToAction("Index");

        }


        //#region API CALLS

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    List<Brand> objBrandList = _unitOfWork.Brand.GetAll().ToList();
        //    return Json(new { data = objBrandList });
        //}


        //[HttpDelete]
        //public IActionResult Delete(int? id)
        //{
        //    var brandToBeDeleted = _unitOfWork.Brand.Get(u => u.Id == id);
        //    if (brandToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }

        //    var oldImagePath =
        //                   Path.Combine(_webHostEnvironment.WebRootPath,
        //                   brandToBeDeleted.BrandImage.TrimStart('\\'));

        //    if (System.IO.File.Exists(oldImagePath))
        //    {
        //        System.IO.File.Delete(oldImagePath);
        //    }

        //    _unitOfWork.Brand.Remove(brandToBeDeleted);
        //    _unitOfWork.Save();

        //    return Json(new { success = true, message = "Delete Successful" });
        //}


        //#endregion
    }
}
