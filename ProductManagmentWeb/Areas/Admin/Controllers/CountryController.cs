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
    [Authorize(Roles = "Admin")]
    public class CountryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public CountryController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        #region Index
        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

            CountryIndexVM countryIndexVM = new CountryIndexVM();
            countryIndexVM.CountryNameSortOrder = string.IsNullOrEmpty(orderBy) ? "countryName_desc" : "";
            var countries = (from data in _unitOfWork.Country.GetAll().ToList()
                             where term == "" ||
                                data.CountryName.ToLower().
                                Contains(term)


                             select new Country
                             {
                                 Id = data.Id,
                                 CountryName = data.CountryName,
                                 IsActive = data.IsActive,
                             });

            switch (orderBy)
            {
                case "stateName_desc":
                    countries = countries.OrderByDescending(a => a.CountryName);
                    break;

                default:
                    countries = countries.OrderBy(a => a.CountryName);
                    break;
            }
            int totalRecords = countries.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            countries = countries.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            countryIndexVM.Countries = countries;
            countryIndexVM.CurrentPage = currentPage;
            countryIndexVM.Term = term;
            countryIndexVM.PageSize = pageSize;
            countryIndexVM.OrderBy = orderBy;
            return View(countryIndexVM);
        }
        #endregion

        #region Upsert
        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new Country());
            }
            else
            {
                //update
                Country country = _unitOfWork.Country.Get(u => u.Id == id);
                return View(country);
            }

        }

        [HttpPost]
        public IActionResult Upsert(Country country)
        {
            if (ModelState.IsValid)
            {

                if (country.Id == 0)
                {
                    try
                    {
                        Country countryObj = _unitOfWork.Country.Get(u => u.CountryName == country.CountryName);
                        if (countryObj != null)
                        {
                            TempData["error"] = "Country Name Already Exist!";
                        }
                        else
                        {

                            _unitOfWork.Country.Add(country);
                            _unitOfWork.Save();
                            TempData["success"] = "Country created successfully";
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
                        Country countryObj = _unitOfWork.Country.Get(u => u.Id != country.Id && u.CountryName == country.CountryName);
                        if (countryObj != null)
                        {
                            TempData["error"] = "Country Name Already Exist!";
                        }
                        else
                        {
                            _unitOfWork.Country.Update(country);
                            _unitOfWork.Save();
                            TempData["success"] = "Country Updated successfully";
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
                return View(country);
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
            List<Country> objCountryList = _unitOfWork.Country.GetAll().ToList();
            return Json(new { data = objCountryList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            try
            {
                var CountryToBeDeleted = _unitOfWork.Country.Get(u => u.Id == id);
                if (CountryToBeDeleted == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }

                _unitOfWork.Country.Remove(CountryToBeDeleted);
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
