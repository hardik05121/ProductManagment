using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using System.Data;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UnitController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public UnitController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Index
        public IActionResult Index()
        {
            List<Unit> objUnitList = _unitOfWork.Unit.GetAll().ToList();
            return View(objUnitList);
        }

        #endregion

        #region Upsert

        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new Unit());
            }
            else
            {
                //update
                Unit unit = _unitOfWork.Unit.Get(u => u.Id == id);
                return View(unit);
            }

        }

        [HttpPost]
        public IActionResult Upsert(Unit unit)
        {
            if (ModelState.IsValid)
            {

                if (unit.Id == 0)
                {
                    Unit unitObj = _unitOfWork.Unit.Get(u => u.UnitName == unit.UnitName);
                    if (unitObj != null)
                    {
                        TempData["error"] = "Unit Name Already Exist!";
                    }
                    else
                    {

                        _unitOfWork.Unit.Add(unit);
                        _unitOfWork.Save();
                        TempData["success"] = "Unit created successfully";
                    }
                }
                else
                {
                    Unit unitObj = _unitOfWork.Unit.Get(u => u.Id != unit.Id && u.UnitName == unit.UnitName);
                    if (unitObj != null)
                    {
                        TempData["error"] = "Tax Name Already Exist!";
                    }
                    else
                    {
                        _unitOfWork.Unit.Update(unit);
                        _unitOfWork.Save();
                        TempData["success"] = "Tax Updated successfully";
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(unit);
            }
        }
        #endregion

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Unit> objUnitList = _unitOfWork.Unit.GetAll().ToList();
            return Json(new { data = objUnitList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var UnitToBeDeleted = _unitOfWork.Unit.Get(u => u.Id == id);
            if (UnitToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Unit.Remove(UnitToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
