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
    [Authorize(Roles = "Admin")]
    public class SupplierController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _db;

        public SupplierController(IUnitOfWork unitOfWork, ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _db = db;

        }
        //public IActionResult Index()
        //{
        //    List<Supplier> objSupplierList = _unitOfWork.Supplier.GetAll(includeProperties: "City,State,Country").ToList();

        //    return View(objSupplierList);
        //}

        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();



            SupplierIndexVM supplierIndexVM = new SupplierIndexVM();
            supplierIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "supplierName_desc" : "";
            var suppliers = (from data in _unitOfWork.Supplier.GetAll(includeProperties: "City,State,Country").ToList()
                             where term == "" ||
                                data.SupplierName.ToLower().
                                Contains(term) || data.Country.CountryName.ToLower().Contains(term) || data.State.StateName.ToLower().Contains(term) || data.City.CityName.ToLower().Contains(term)


                             select new Supplier
                             {
                                 Id = data.Id,
                                 SupplierName = data.SupplierName,
                                 Email = data.Email,
                                 MobileNumber = data.MobileNumber,
                                 Country = data.Country,
                                 State = data.State,
                                 City = data.City,
                                 WebSite = data.WebSite

                             });

            switch (orderBy)
            {
                case "supplierName_desc":
                    suppliers = suppliers.OrderByDescending(a => a.SupplierName);
                    break;

                default:
                    suppliers = suppliers.OrderBy(a => a.SupplierName);
                    break;
            }
            int totalRecords = suppliers.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            suppliers = suppliers.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            supplierIndexVM.Suppliers = suppliers;
            supplierIndexVM.CurrentPage = currentPage;
            supplierIndexVM.TotalPages = totalPages;
            supplierIndexVM.Term = term;
            supplierIndexVM.PageSize = pageSize;
            supplierIndexVM.OrderBy = orderBy;
            return View(supplierIndexVM);


        }

        public IActionResult Upsert(int? id)
        {
            SupplierVM supplierVM = new()
            {
                CityList = _unitOfWork.City.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CityName,
                    Value = u.Id.ToString()
                }),
                StateList = _unitOfWork.State.GetAll().Select(u => new SelectListItem
                {
                    Text = u.StateName,
                    Value = u.Id.ToString()
                }),
                CountryList = _unitOfWork.Country.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CountryName,
                    Value = u.Id.ToString()
                }),


                Supplier = new Supplier()
            };

            if (id == null || id == 0)
            {
                //create
                return View(supplierVM);
            }
            else
            {
                //update
                supplierVM.Supplier = _unitOfWork.Supplier.Get(u => u.Id == id);
                return View(supplierVM);
            }

        }
        [HttpPost]
        public IActionResult Upsert(SupplierVM supplierVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                try
                {


                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string supplierPath = Path.Combine(wwwRootPath, @"images\supplier");

                        if (!string.IsNullOrEmpty(supplierVM.Supplier.SupplierImage))
                        {
                            //delete the old image
                            var oldImagePath =
                                Path.Combine(wwwRootPath, supplierVM.Supplier.SupplierImage.TrimStart('\\'));

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        using (var fileStream = new FileStream(Path.Combine(supplierPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        supplierVM.Supplier.SupplierImage = @"\images\supplier\" + fileName;
                    }
                }
                catch (Exception ex)
                {
                    LogErrorToDatabase(ex);

                    TempData["error"] = "error accured";
                    // return View(brand);
                    return RedirectToAction("Error", "Home");
                }

                if (supplierVM.Supplier.Id == 0)
                {
                    try
                    {


                        Supplier supplierObj = _unitOfWork.Supplier.Get(u => u.SupplierName == supplierVM.Supplier.SupplierName);
                        if (supplierObj != null)
                        {
                            TempData["error"] = "Supplier Name Already Exist!";
                        }
                        else
                        {
                            _unitOfWork.Supplier.Add(supplierVM.Supplier);
                            _unitOfWork.Save();
                            TempData["success"] = "Supplier created successfully";
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


                        Supplier supplierObj = _unitOfWork.Supplier.Get(u => u.Id != supplierVM.Supplier.Id && u.SupplierName == supplierVM.Supplier.SupplierName);
                        if (supplierObj != null)
                        {
                            TempData["error"] = "Supplier Name Already Exist!";
                        }
                        else
                        {
                            _unitOfWork.Supplier.Update(supplierVM.Supplier);
                            _unitOfWork.Save();
                            TempData["success"] = "Supplier Updated successfully";
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

                return View(supplierVM.Supplier);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            try
            {


                Supplier SupplierToBeDeleted = _unitOfWork.Supplier.Get(u => u.Id == id);
                if (SupplierToBeDeleted == null)
                {
                    TempData["error"] = "Supplier can't be Delete.";
                    return RedirectToAction("Index");
                }
                var oldImagePath =
                          Path.Combine(_webHostEnvironment.WebRootPath,
                           SupplierToBeDeleted.SupplierImage.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                _unitOfWork.Supplier.Remove(SupplierToBeDeleted);
                _unitOfWork.Save();
                TempData["success"] = "Supplier Deleted successfully";
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

        //#region API CALLS
        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    List<Supplier> objSupplierList = _unitOfWork.Supplier.GetAll(includeProperties: "City,State,Country").ToList();
        //    return Json(new { data = objSupplierList });
        //}


        //[HttpDelete]
        //public IActionResult Delete(int? id)
        //{
        //    var SupplierToBeDeleted = _unitOfWork.Supplier.Get(u => u.Id == id);
        //    if (SupplierToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }

        //    var oldImagePath =
        //                   Path.Combine(_webHostEnvironment.WebRootPath,
        //                   SupplierToBeDeleted.SupplierImage.TrimStart('\\'));

        //    if (System.IO.File.Exists(oldImagePath))
        //    {
        //        System.IO.File.Delete(oldImagePath);
        //    }

        //    _unitOfWork.Supplier.Remove(SupplierToBeDeleted);
        //    _unitOfWork.Save();

        //    return Json(new { success = true, message = "Delete Successful" });
        //}


        //#endregion

        #region Csacadion Droup down State,country, City
        [HttpGet]
        public IActionResult GetStatesByCountry(int countryId)
        {
            var states = _unitOfWork.State.GetAll(s => s.CountryId == countryId);
            return Json(states);
        }

        [HttpGet]
        public IActionResult GetCitiesByState(int stateId)
        {
            var cities = _unitOfWork.City.GetAll(c => c.StateId == stateId);
            return Json(cities);
        }

        #endregion
    }
}
