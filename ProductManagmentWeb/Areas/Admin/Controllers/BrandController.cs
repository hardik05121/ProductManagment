using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public BrandController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        #region Index
        public IActionResult Index()
        {
            List<Brand> objBrandList = _unitOfWork.Brand.GetAll().ToList();
            return View(objBrandList);
        }
        #endregion

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
                    _unitOfWork.Brand.Add(brand);
                    _unitOfWork.Save();
                    TempData["success"] = "Brand created successfully";
                }
                else
                {
                    _unitOfWork.Brand.Update(brand);
                    _unitOfWork.Save();
                    TempData["success"] = "Brand Updated successfully";
                }


                return RedirectToAction("Index");
            }
            else
            {
                //productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                //{
                //    Text = u.Name,
                //    Value = u.Id.ToString()
                //});
                return View(brand);
            }
        }
        #endregion

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Brand> objBrandList = _unitOfWork.Brand.GetAll().ToList();
            return Json(new { data = objBrandList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var brandToBeDeleted = _unitOfWork.Brand.Get(u => u.Id == id);
            if (brandToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
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

            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion
    }
}
