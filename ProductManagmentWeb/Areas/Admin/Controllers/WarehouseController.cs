using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;

namespace ProductManagment.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class WarehouseController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public WarehouseController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IActionResult Index()
        {
            List<Warehouse> objWarehousyList = _unitOfWork.Warehouse.GetAll().ToList();
            return View(objWarehousyList);
        }




        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new Warehouse());
            }
            else
            {
                //update
                Warehouse warehouse = _unitOfWork.Warehouse.Get(u => u.Id == id);
                return View(warehouse);
            }

        }

        [HttpPost]
        public IActionResult Upsert(Warehouse warehouse)
        {
           if (ModelState.IsValid)
            {

                if (warehouse.Id == 0)
                {
                    Warehouse warehouseObj = _unitOfWork.Warehouse.Get(u => u.WarehouseName == warehouse.WarehouseName);
                    if (warehouseObj != null)
                    {
                        TempData["error"] = "Warehouse Name Already Exist!";
                    }
                    else
                    {

                        _unitOfWork.Warehouse.Add(warehouse);
                        _unitOfWork.Save();
                        TempData["success"] = "Warehouse created successfully";
                    }
                }
                else
                {
                    Warehouse warehouseObj = _unitOfWork.Warehouse.Get(u => u.Id != warehouse.Id && u.WarehouseName == warehouse.WarehouseName);
                    if (warehouseObj != null)
                    {
                        TempData["error"] = "Warehouse Name Already Exist!";
                    }
                    else
                    {
                        _unitOfWork.Warehouse.Update(warehouse);
                        _unitOfWork.Save();
                        TempData["success"] = "Warehouse Updated successfully";
                    }
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(warehouse);
            }
        }


        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Warehouse> objWarehousyList = _unitOfWork.Warehouse.GetAll().ToList();
            return Json(new { data = objWarehousyList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var WarehousyToBeDeleted = _unitOfWork.Warehouse.Get(u => u.Id == id);
            if (WarehousyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Warehouse.Remove(WarehousyToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
