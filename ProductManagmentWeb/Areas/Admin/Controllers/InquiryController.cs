using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using ProductManagment_DataAccess.Data;
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
        private readonly ApplicationDbContext _db;

        public InquiryController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;

        }
        #region Index
        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

            InquiryIndexVM inquiryIndexVM = new InquiryIndexVM(); // page and serch mate
            inquiryIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "email_desc" : "";
            var inquiries = (from data in _unitOfWork.Inquiry.GetAll(includeProperties: "InquirySource,InquiryStatus,Product,Country,State,City").ToList()
                             where term == "" || data.Email.ToLower().
                              Contains(term) || data.InquirySource.InquirySourceName.ToLower().Contains(term)

                             select new Inquiry
                             {
                                 Id = data.Id,
                                 Organization = data.Organization,
                                 ContactPerson = data.ContactPerson,
                                 Email = data.Email,
                                 MobileNumber = data.MobileNumber,
                                 PhoneNumber = data.PhoneNumber,
                                 Website = data.Website,
                                 Address = data.Address,
                                 Country = data.Country,
                                 Product = data.Product,
                                 State = data.State,
                                 City = data.City,
                                 InquirySource = data.InquirySource,
                                 InquiryStatus = data.InquiryStatus,
                             });

            switch (orderBy)
            {
                case "email_desc":
                    inquiries = inquiries.OrderByDescending(a => a.Email);
                    break;

                default:
                    inquiries = inquiries.OrderBy(a => a.Email);
                    break;
            }
            int totalRecords = inquiries.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            inquiries = inquiries.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            inquiryIndexVM.Inquiries = inquiries;
            inquiryIndexVM.CurrentPage = currentPage;
            inquiryIndexVM.TotalPages = totalPages;
            inquiryIndexVM.Term = term;
            inquiryIndexVM.PageSize = pageSize;
            inquiryIndexVM.OrderBy = orderBy;
            return View(inquiryIndexVM);
        }
        #endregion

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
        public IActionResult Upsert(InquiryVM inquiryVM)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (inquiryVM.Inquiry.Id == 0)
                    {
                        try
                        {
                            _unitOfWork.Inquiry.Add(inquiryVM.Inquiry);
                            _unitOfWork.Save();
                            TempData["success"] = "Inquiry created successfully";
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
                            _unitOfWork.Inquiry.Update(inquiryVM.Inquiry);
                            _unitOfWork.Save();
                            TempData["success"] = "Inquiry Updated successfully";
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
            catch (Exception ex)
            {
                LogErrorToDatabase(ex);

                TempData["error"] = "error accured";
                // return View(brand);
                return RedirectToAction("Error", "Home");
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
            try
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
            catch (Exception ex)
            {
                LogErrorToDatabase(ex);

                TempData["error"] = "error accured";
                // return View(brand);
                return RedirectToAction("Error", "Home");
            }
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
