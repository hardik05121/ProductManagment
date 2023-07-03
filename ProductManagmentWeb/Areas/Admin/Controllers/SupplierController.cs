using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using ProductManagment_Models.ViewModels;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SupplierController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SupplierController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            List<Supplier> objSupplierList = _unitOfWork.Supplier.GetAll(includeProperties: "City,State,Country").ToList();

            return View(objSupplierList);
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

                if (supplierVM.Supplier.Id == 0)
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
                else
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
                return RedirectToAction("Index");
            }
            else
            {

                return View(supplierVM.Supplier);
            }
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Supplier> objSupplierList = _unitOfWork.Supplier.GetAll(includeProperties: "City,State,Country").ToList();
            return Json(new { data = objSupplierList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var SupplierToBeDeleted = _unitOfWork.Supplier.Get(u => u.Id == id);
            if (SupplierToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
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

            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion

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
