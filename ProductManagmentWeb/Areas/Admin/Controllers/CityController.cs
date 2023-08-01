using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class CityController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;

        public CityController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;

        }

        //#region Index
        //public IActionResult Index()
        //{
        //    List<City> objCityList = _unitOfWork.City.GetAll(includeProperties: "Country,State").ToList();

        //    return View(objCityList);
        //}
        //#endregion

        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();


            CityIndexVM cityIndexVM = new CityIndexVM();
            cityIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "cityName_desc" : "";
            var cities = (from data in _unitOfWork.City.GetAll(includeProperties: "Country,State").ToList()
                          where term == "" ||
                             data.CityName.ToLower().
                             Contains(term) || data.Country.CountryName.ToLower().Contains(term) || data.State.StateName.ToLower().Contains(term)


                          select new City
                          {
                              Id = data.Id,
                              CityName = data.CityName,
                              State = data.State,
                              IsActive = data.IsActive,
                              Country = data.Country
                          });
            switch (orderBy)
            {
                case "cityName_desc":
                    cities = cities.OrderByDescending(a => a.CityName);
                    break;

                default:
                    cities = cities.OrderBy(a => a.CityName);
                    break;
            }
            int totalRecords = cities.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            cities = cities.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            cityIndexVM.Cities = cities;
            cityIndexVM.CurrentPage = currentPage;
            cityIndexVM.TotalPages = totalPages;
            cityIndexVM.Term = term;
            cityIndexVM.PageSize = pageSize;
            cityIndexVM.OrderBy = orderBy;
            return View(cityIndexVM);


        }


        #region Upsert
        public IActionResult Upsert(int? id)
        {
            CityVM cityVM = new()
            {
                CountryList = _unitOfWork.Country.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CountryName,
                    Value = u.Id.ToString()
                }),
                //StateList = _unitOfWork.State.GetAll().Select(u => new SelectListItem
                //{
                //    Text = u.StateName,
                //    Value = u.Id.ToString()
                //}),

                // this for add for the dropdown list
                StateList = Enumerable.Empty<SelectListItem>(),
                City = new City()
            };

            if (id == null || id == 0)
            {
                //create
                return View(cityVM);
            }
            else
            {
                //update
                cityVM.City = _unitOfWork.City.Get(u => u.Id == id);
                //  tis line add for the drodownn list.
                cityVM.StateList = _unitOfWork.State.GetAll().Select(u => new SelectListItem
                {
                    Text = u.StateName,
                    Value = u.Id.ToString()
                });
                return View(cityVM);

            }
        }

        [HttpPost]
        public IActionResult Upsert(CityVM cityVM)
        {


            if (ModelState.IsValid)
            {

                if (cityVM.City.Id == 0)
                {
                    try
                    {



                        City cityobj = _unitOfWork.City.Get(u => u.CityName == cityVM.City.CityName && u.StateId == cityVM.City.StateId);
                        if (cityobj != null)
                        {
                            TempData["error"] = "City Name Already Exist!";
                        }
                        else
                        {
                            _unitOfWork.City.Add(cityVM.City);
                            _unitOfWork.Save();
                            TempData["success"] = "City Created successfully";
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


                        City cityObj = _unitOfWork.City.Get(u => u.Id != cityVM.City.Id && u.CityName == cityVM.City.CityName && u.StateId == cityVM.City.StateId);
                        if (cityObj != null)
                        {
                            TempData["error"] = "Brand Name Already Exist!";
                        }
                        else
                        {
                            cityVM.CountryList = _unitOfWork.Country.GetAll().Select(u => new SelectListItem
                            {
                                Text = u.CountryName,
                                Value = u.Id.ToString()
                            });
                            cityVM.StateList = _unitOfWork.State.GetAll().Select(u => new SelectListItem
                            {
                                Text = u.StateName,
                                Value = u.Id.ToString()
                            });

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
                return View(cityVM);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            try
            {
                City cityToBeDeleted = _unitOfWork.City.Get(u => u.Id == id);
                if (cityToBeDeleted == null)
                {
                    TempData["error"] = "City can't be Delete.";
                    return RedirectToAction("Index");
                }

                _unitOfWork.City.Remove(cityToBeDeleted);
                _unitOfWork.Save();
                TempData["success"] = "City Deleted successfully";
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
        //    List<City> objCityList = _unitOfWork.City.GetAll(includeProperties: "Country,State").ToList();
        //    return Json(new { data = objCityList });
        //}


        //[HttpDelete]
        //public IActionResult Delete(int? id)
        //{
        //    var cityToBeDeleted = _unitOfWork.City.Get(u => u.Id == id);
        //    if (cityToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }

        //    _unitOfWork.City.Remove(cityToBeDeleted);
        //    _unitOfWork.Save();


        //    return Json(new { success = true, message = "Delete Successful" });
        //}

        //#endregion

        #region Csacadion Droup down State,country, City
        [HttpGet]

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

