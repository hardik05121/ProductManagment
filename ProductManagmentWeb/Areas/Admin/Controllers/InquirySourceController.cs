using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
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
    public class InquirySourceController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public InquirySourceController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        #region Index
        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

            InquirySourceIndexVM inquirySourceIndexVM = new InquirySourceIndexVM();

            inquirySourceIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "inquirySourceName_desc" : "";
            var inquirySources = (from data in _unitOfWork.InquirySource.GetAll().ToList()
                                  where term == "" ||
                                     data.InquirySourceName.ToLower().
                                     Contains(term)

                                  select new InquirySource
                                  {
                                      Id = data.Id,
                                      InquirySourceName = data.InquirySourceName,
                                      IsActive = data.IsActive,

                                  });
            switch (orderBy)
            {
                case "inquirySourceName_desc":
                    inquirySources = inquirySources.OrderByDescending(a => a.InquirySourceName);
                    break;

                default:
                    inquirySources = inquirySources.OrderBy(a => a.InquirySourceName);
                    break;
            }
            int totalRecords = inquirySources.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            inquirySources = inquirySources.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            inquirySourceIndexVM.InquirySources = inquirySources;
            inquirySourceIndexVM.CurrentPage = currentPage;
            inquirySourceIndexVM.TotalPages = totalPages;
            inquirySourceIndexVM.Term = term;
            inquirySourceIndexVM.PageSize = pageSize;
            inquirySourceIndexVM.OrderBy = orderBy;

            return View(inquirySourceIndexVM);
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

        #region Upsert
        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new InquirySource());
            }
            else
            {
                //update
                InquirySource inquirySource = _unitOfWork.InquirySource.Get(u => u.Id == id);
                return View(inquirySource);
            }

        }

        [HttpPost]
        public IActionResult Upsert(InquirySource inquirySource)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (inquirySource.Id == 0)
                    {
                        try
                        {
                            InquirySource inquirySourceobj = _unitOfWork.InquirySource.Get(u => u.InquirySourceName == inquirySource.InquirySourceName);
                            if (inquirySourceobj != null)
                            {
                                TempData["error"] = "InquirySource Already Exist!";
                            }
                            else
                            {
                                _unitOfWork.InquirySource.Add(inquirySource);
                                _unitOfWork.Save();
                                TempData["success"] = "InquirySource created successfully";
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
                            InquirySource inquirySourceobj = _unitOfWork.InquirySource.Get(u => u.Id != inquirySource.Id && u.InquirySourceName == inquirySource.InquirySourceName);
                            if (inquirySourceobj != null)
                            {
                                TempData["error"] = "Brand Name Already Exist!";
                            }
                            else
                            {
                                _unitOfWork.InquirySource.Update(inquirySource);
                                _unitOfWork.Save();
                                TempData["success"] = "InquirySource Updated successfully";
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
                    return View(inquirySource);
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
        #endregion

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<InquirySource> objInquirySourceList = _unitOfWork.InquirySource.GetAll().ToList();
            return Json(new { data = objInquirySourceList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            try
            {
                var InquirySourceToBeDeleted = _unitOfWork.InquirySource.Get(u => u.Id == id);
                if (InquirySourceToBeDeleted == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }

                _unitOfWork.InquirySource.Remove(InquirySourceToBeDeleted);
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
