using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProductManagment_DataAccess.Data;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using ProductManagment_Models.ViewModels;
using System.Data;

using System.Diagnostics.Metrics;


namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class InquiryStatusController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public InquiryStatusController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        #region Index
        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

            InquiryStatusIndexVM inquiryStatusIndexVM = new InquiryStatusIndexVM();

            inquiryStatusIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "inquiryStatusName_desc" : "";
            var inquiryStatuses = (from data in _unitOfWork.InquiryStatus.GetAll().ToList()
                                   where term == "" ||
                                      data.InquiryStatusName.ToLower().
                                      Contains(term)


                                   select new InquiryStatus
                                   {
                                       Id = data.Id,
                                       InquiryStatusName = data.InquiryStatusName,
                                       IsActive = data.IsActive,

                                   });
            switch (orderBy)
            {
                case "inquiryStatusName_desc":
                    inquiryStatuses = inquiryStatuses.OrderByDescending(a => a.InquiryStatusName);
                    break;

                default:
                    inquiryStatuses = inquiryStatuses.OrderBy(a => a.InquiryStatusName);
                    break;
            }
            int totalRecords = inquiryStatuses.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            inquiryStatuses = inquiryStatuses.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            inquiryStatusIndexVM.InquiryStatuses = inquiryStatuses;
            inquiryStatusIndexVM.CurrentPage = currentPage;
            inquiryStatusIndexVM.TotalPages = totalPages;
            inquiryStatusIndexVM.Term = term;
            inquiryStatusIndexVM.PageSize = pageSize;
            inquiryStatusIndexVM.OrderBy = orderBy;

            return View(inquiryStatusIndexVM);
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
                return View(new InquiryStatus());
            }
            else
            {
                //update
                InquiryStatus inquiryStatus = _unitOfWork.InquiryStatus.Get(u => u.Id == id);
                return View(inquiryStatus);
            }

        }

        [HttpPost]
        public IActionResult Upsert(InquiryStatus inquiryStatus)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    if (inquiryStatus.Id == 0)
                    {
                        try
                        {
                            InquiryStatus inquiryStatusObj = _unitOfWork.InquiryStatus.Get(u => u.InquiryStatusName == inquiryStatus.InquiryStatusName);
                            if (inquiryStatusObj != null)
                            {
                                TempData["error"] = "InquiryStatus Name Already Exist!";
                            }
                            else
                            {

                                _unitOfWork.InquiryStatus.Add(inquiryStatus);
                                _unitOfWork.Save();
                                TempData["success"] = "InquiryStatus created successfully";
                            }

                            //_unitOfWork.InquiryStatus.Add(inquiryStatus);
                            //_unitOfWork.Save();
                            //TempData["success"] = "InquiryStatus created successfully";
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
                            InquiryStatus inquiryStatusyObj = _unitOfWork.InquiryStatus.Get(u => u.Id != inquiryStatus.Id && u.InquiryStatusName == inquiryStatus.InquiryStatusName);
                            if (inquiryStatusyObj != null)
                            {
                                TempData["error"] = "InquiryStatus Name Already Exist!";
                            }
                            else
                            {
                                _unitOfWork.InquiryStatus.Update(inquiryStatus);
                                _unitOfWork.Save();
                                TempData["success"] = "InquiryStatus Updated successfully";
                            }
                            //_unitOfWork.InquiryStatus.Update(inquiryStatus);
                            //_unitOfWork.Save();
                            //TempData["success"] = "InquiryStatus Updated successfully";
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
                    return View(inquiryStatus);
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
            List<InquiryStatus> objInquiryStatusList = _unitOfWork.InquiryStatus.GetAll().ToList();
            return Json(new { data = objInquiryStatusList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            try
            {
                var InquiryStatusToBeDeleted = _unitOfWork.InquiryStatus.Get(u => u.Id == id);
                if (InquiryStatusToBeDeleted == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }

                _unitOfWork.InquiryStatus.Remove(InquiryStatusToBeDeleted);
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
