using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public CityController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        #region Index
        public IActionResult Index()
        {
            List<City> objCityList = _unitOfWork.City.GetAll(includeProperties: "Country,State").ToList();

            return View(objCityList);
        }
        #endregion

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
                else
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
                return RedirectToAction("Index");
            }
            else
            { 
                return View(); 
            }
        }
            #endregion


            #region API CALLS

            [HttpGet]
            public IActionResult GetAll()
            {
                List<City> objCityList = _unitOfWork.City.GetAll(includeProperties: "Country,State").ToList();
                return Json(new { data = objCityList });
            }


            [HttpDelete]
            public IActionResult Delete(int? id)
            {
                var cityToBeDeleted = _unitOfWork.City.Get(u => u.Id == id);
                if (cityToBeDeleted == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }

                _unitOfWork.City.Remove(cityToBeDeleted);
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

