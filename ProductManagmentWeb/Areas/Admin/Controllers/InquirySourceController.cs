using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class InquirySourceController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public InquirySourceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Index
        public IActionResult Index()
        {
            List<InquirySource> objInquirySourceList = _unitOfWork.InquirySource.GetAll().ToList();

            return View(objInquirySourceList);
        }

        #endregion

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
            if (ModelState.IsValid)
            {

                if (inquirySource.Id == 0)
                {
                    _unitOfWork.InquirySource.Add(inquirySource);
                    _unitOfWork.Save();
                    TempData["success"] = "InquirySource created successfully";
                }
                else
                {
                    _unitOfWork.InquirySource.Update(inquirySource);
                    _unitOfWork.Save();
                    TempData["success"] = "InquirySource Updated successfully";
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(inquirySource);
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
            var InquirySourceToBeDeleted = _unitOfWork.InquirySource.Get(u => u.Id == id);
            if (InquirySourceToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.InquirySource.Remove(InquirySourceToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
