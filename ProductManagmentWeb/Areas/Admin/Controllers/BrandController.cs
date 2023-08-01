using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductManagment_DataAccess.Data;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;

using ProductManagment_Models.ViewModels;

using System.Data;

//using ProductManagment_Models.ModelsMetadata;


namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    // [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _db;
        public BrandController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _db = db;
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

            try
            {
                if (ModelState.IsValid)
                {
                    try
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
                    }
                    catch (Exception ex)
                    {
                        LogErrorToDatabase(ex);
                        TempData["error"] = ex.Message;
                        // return View(brand);
                        return View(brand);
                    }


                    if (brand.Id == 0)
                    {
                        try
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
                        catch (Exception ex)
                        {
                            // LogErrorToDatabase(ex);

                            TempData["error"] = ex.Message;
                            // return View(brand);
                            return View(brand);
                        }

                    }
                    else
                    {
                        try
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
                        catch (Exception ex)
                        {
                            TempData["error"] = ex.Message;
                            // return View(brand);
                            return View(brand);
                        }
                    }
                    return RedirectToAction("Index");
                }
                else
                {

                    return View(brand);
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


        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            try
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
            catch (Exception ex)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    ErrorDate = DateTime.Now
                };

                _db.ErrorLogs.Add(error);
                _db.SaveChanges();

                TempData["error"] = "error accured";
                // return View(brand);
                return RedirectToAction("Error", "Home");
            }
        }
    }
}

// catch (IOException ex)
//    {
//    // Handle IO-related exceptions (file operations)
//    TempData["error"] = "An error occurred while processing the file.";
//}
//    catch (DbUpdateException ex)
//    {
//    // Handle database update related exceptions
//    TempData["error"] = "An error occurred while saving data to the database.";
//}
//    catch (Exception ex)
//    {
//    // Catch any other unexpected exceptions
//    TempData["error"] = "An unexpected error occurred.";
//}

//// Log the error to the ILogger
//LogErrorToDatabase(ex);

//// Redirect to the error page
//return RedirectToAction("Error", "Home");
//}
