using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TaxController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public TaxController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Index
        public IActionResult Index()
        {
            List<Tax> objTaxList = _unitOfWork.Tax.GetAll().ToList();

            return View(objTaxList);
        }

        #endregion

        #region Upsert
        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new Tax());
            }
            else
            {
                //update
                Tax tax = _unitOfWork.Tax.Get(u => u.Id == id);
                return View(tax);
            }

        }

        [HttpPost]
        public IActionResult Upsert(Tax tax)
        {
            if (ModelState.IsValid)
            {

                if (tax.Id == 0)
                {
                    _unitOfWork.Tax.Add(tax);
                    _unitOfWork.Save();
                    TempData["success"] = "Tax created successfully";
                }
                else
                {
                    _unitOfWork.Tax.Update(tax);
                    _unitOfWork.Save();
                    TempData["success"] = "Tax Updated successfully";
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(tax);
            }
        }
        #endregion

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Tax> objTaxList = _unitOfWork.Tax.GetAll().ToList();
            return Json(new { data = objTaxList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var TaxToBeDeleted = _unitOfWork.Tax.Get(u => u.Id == id);
            if (TaxToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Tax.Remove(TaxToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
