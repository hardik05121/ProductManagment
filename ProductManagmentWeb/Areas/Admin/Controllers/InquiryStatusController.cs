using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;

using System.Data;

using System.Diagnostics.Metrics;


namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class InquiryStatusController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public InquiryStatusController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Index
        public IActionResult Index()
        {
            List<InquiryStatus> objInquiryStatusList = _unitOfWork.InquiryStatus.GetAll().ToList();

            return View(objInquiryStatusList);
        }

        #endregion

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
            if (ModelState.IsValid)
            {

                if (inquiryStatus.Id == 0)
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
                else
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
                return RedirectToAction("Index");
            }
            else
            {
                return View(inquiryStatus);
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
            var InquiryStatusToBeDeleted = _unitOfWork.InquiryStatus.Get(u => u.Id == id);
            if (InquiryStatusToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.InquiryStatus.Remove(InquiryStatusToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
