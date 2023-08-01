using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagment_DataAccess.Data;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using ProductManagment_Models.ViewModels;
using System.Data;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class InventoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;


        public InventoryController(IUnitOfWork unitOfWork,ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        #region Index
        public IActionResult Index()
        {
            List<Inventory> objInventoryList = _unitOfWork.Inventory.GetAll(includeProperties: "Product,Unit,Warehouse").ToList();

            return View(objInventoryList);
        }
        #endregion

        #region Upsert
        public IActionResult Upsert(int? id)
        {
            InventoryVM inventoryVM = new()
            {
                ProductList = _unitOfWork.Product.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                UnitList = _unitOfWork.Unit.GetAll().Select(u => new SelectListItem
                {
                    Text = u.BaseUnit,
                    Value = u.Id.ToString()
                }),
                WarehouseList = _unitOfWork.Warehouse.GetAll().Select(u => new SelectListItem
                {
                    Text = u.WarehouseName,
                    Value = u.Id.ToString()
                }),

                //  Product = _unitOfWork.Product.Get(u => u.Id == id),
                // this for add for the dropdown list
                //StateList = Enumerable.Empty<SelectListItem>(),
                Inventory = new Inventory()
            };

            if (id == null || id == 0)
            {
                //create
                return View(inventoryVM);
            }
            else
            {
                //update
                inventoryVM.Inventory = _unitOfWork.Inventory.Get(u => u.Id == id, includeProperties: "Product,Unit,Warehouse");
                //  tis line add for the drodownn list.
                //inventoryVM.StateList = _unitOfWork.State.GetAll().Select(u => new SelectListItem
                //{
                //    Text = u.StateName,
                //    Value = u.Id.ToString()
                //});
                return View(inventoryVM);

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
        public IActionResult Upsert(InventoryVM inventoryVM)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (inventoryVM.Inventory.Id == 0)
                    {
                        try
                        {
                            _unitOfWork.Inventory.Add(inventoryVM.Inventory);
                            _unitOfWork.Save();
                            TempData["success"] = "Inventory Created successfully";
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
                            _unitOfWork.Inventory.Update(inventoryVM.Inventory);
                            _unitOfWork.Save();
                            TempData["success"] = "Inventory Updated successfully";
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
                    inventoryVM.ProductList = _unitOfWork.Product.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });
                    inventoryVM.UnitList = _unitOfWork.Unit.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.BaseUnit,
                        Value = u.Id.ToString()
                    });
                    inventoryVM.WarehouseList = _unitOfWork.Warehouse.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.WarehouseName,
                        Value = u.Id.ToString()
                    });
                    return View(inventoryVM);
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
            List<Inventory> objInventoryList = _unitOfWork.Inventory.GetAll(includeProperties: "Product,Unit,Warehouse").ToList();
            return Json(new { data = objInventoryList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            try
            {
                var inventoryToBeDeleted = _unitOfWork.Inventory.Get(u => u.Id == id);
                if (inventoryToBeDeleted == null)
                {
                    return Json(new { success = false, message = "Error while deleting" });
                }

                _unitOfWork.Inventory.Remove(inventoryToBeDeleted);
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


        //#region Csacadion Droup down State,country, City
        //[HttpGet]
        //public IActionResult GetStatesByCountry(int countryId)
        //{
        //    var states = _unitOfWork.State.GetAll(s => s.CountryId == countryId);
        //    return Json(states);
        //}

        //[HttpGet]
        //public IActionResult GetCitiesByState(int stateId)
        //{
        //    var cities = _unitOfWork.City.GetAll(c => c.StateId == stateId);
        //    return Json(cities);
        //}

        //#endregion
    }
}
