using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CountryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public CountryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Index
        public IActionResult Index()
        {
            List<Country> objCountryList = _unitOfWork.Country.GetAll().ToList();

            return View(objCountryList);
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
                    _unitOfWork.Country.Add(country);
                    _unitOfWork.Save();
                    TempData["success"] = "Country created successfully";
                }
                else
                {
                    _unitOfWork.Country.Update(country);
                    _unitOfWork.Save();
                    TempData["success"] = "Country Updated successfully";
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(country);
            }
        }
        #endregion

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
            var CountryToBeDeleted = _unitOfWork.Country.Get(u => u.Id == id);
            if (CountryToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Country.Remove(CountryToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
