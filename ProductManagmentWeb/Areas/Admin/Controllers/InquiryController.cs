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
    [Authorize]
    public class InquiryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public InquiryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;


        }
        public IActionResult Index()
        {
            List<Inquiry> objInquiryList = _unitOfWork.Inquiry.GetAll(includeProperties: "Product,Country,State,City,InquirySource,InquiryStatus").ToList();

            return View(objInquiryList);
        }
        public IActionResult Upsert(int? id)
        {
            InquiryVM InquiryVM = new()
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
                ProductList = _unitOfWork.Product.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                //UserList = _unitOfWork.User.GetAll().Select(u => new SelectListItem
                //{
                //    Text = u.FirstName,
                //    Value = u.Id.ToString()
                //}), 
                SourceList = _unitOfWork.InquirySource.GetAll().Select(u => new SelectListItem
                {
                    Text = u.InquirySourceName,
                    Value = u.Id.ToString()
                }), 
                StatusList = _unitOfWork.InquiryStatus.GetAll().Select(u => new SelectListItem
                {
                    Text = u.InquiryStatusName,
                    Value = u.Id.ToString()
                }),

                Inquiry = new Inquiry()
            };

            if (id == null || id == 0)
            {
                //create
                return View(InquiryVM);
            }
            else
            {
                //update
                InquiryVM.Inquiry = _unitOfWork.Inquiry.Get(u => u.Id == id);
                return View(InquiryVM);
            }

        }
        [HttpPost]
        public IActionResult Upsert(InquiryVM inquiryVM)
        {
            if (ModelState.IsValid)
            {

                if (inquiryVM.Inquiry.Id == 0)
                {
                    _unitOfWork.Inquiry.Add(inquiryVM.Inquiry);
                    _unitOfWork.Save();
                    TempData["success"] = "Inquiry created successfully";
                }
                else
                {
                    _unitOfWork.Inquiry.Update(inquiryVM.Inquiry);
                    _unitOfWork.Save();
                    TempData["success"] = "Inquiry Updated successfully";
                }
                return RedirectToAction("Index");
            }
            else
            {
                inquiryVM.CityList = _unitOfWork.City.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CityName,
                    Value = u.Id.ToString()
                });
                inquiryVM.StateList = _unitOfWork.State.GetAll().Select(u => new SelectListItem
                {
                    Text = u.StateName,
                    Value = u.Id.ToString()
                });
                inquiryVM.CountryList = _unitOfWork.Country.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CountryName,
                    Value = u.Id.ToString()
                });
                inquiryVM.ProductList = _unitOfWork.Product.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });
                //inquiryVM.UserList = _unitOfWork.User.GetAll().Select(u => new SelectListItem
                //{
                //    Text = u.FirstName,
                //    Value = u.Id.ToString()
                //});
                inquiryVM.SourceList = _unitOfWork.InquirySource.GetAll().Select(u => new SelectListItem
                {
                    Text = u.InquirySourceName,
                    Value = u.Id.ToString()
                });
                inquiryVM.StatusList = _unitOfWork.InquiryStatus.GetAll().Select(u => new SelectListItem
                {
                    Text = u.InquiryStatusName,
                    Value = u.Id.ToString()
                });
                return View(inquiryVM);
            }
        }
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Inquiry> objInquiryList = _unitOfWork.Inquiry.GetAll(includeProperties: "Product,State,Country,City,InquirySource,InquiryStatus").ToList();
            return Json(new { data = objInquiryList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var InquiryToBeDeleted = _unitOfWork.Inquiry.Get(u => u.Id == id);
            if (InquiryToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Inquiry.Remove(InquiryToBeDeleted);
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
