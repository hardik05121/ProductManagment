using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Data;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using ProductManagment_Models.ViewModels;
using System.Data;

namespace ProductManagment.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class WarehouseController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public WarehouseController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }


        //public IActionResult Index()
        //{
        //    List<Warehouse> objWarehousyList = _unitOfWork.Warehouse.GetAll().ToList();
        //    return View(objWarehousyList);
        //}

        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

            //statePaginationVM.CountryList = _unitOfWork.Country.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.CountryName,
            //    Value = u.Id.ToString()
            //});


            WarehouseIndexVM warehouseIndexVM = new WarehouseIndexVM();
            warehouseIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "warehouseName_desc" : "";
            var warehouses = (from data in _unitOfWork.Warehouse.GetAll().ToList()
                              where term == "" ||
                                 data.WarehouseName.ToLower().
                                 Contains(term)


                              select new Warehouse
                              {
                                  Id = data.Id,
                                  WarehouseName = data.WarehouseName,
                                  ContactPerson = data.ContactPerson,
                                  MobileNumber = data.MobileNumber,
                                  Email = data.Email,
                                  Address = data.Address,
                                  IsActive = data.IsActive

                              });
            switch (orderBy)
            {
                case "warehouseName_desc":
                    warehouses = warehouses.OrderByDescending(a => a.WarehouseName);
                    break;

                default:
                    warehouses = warehouses.OrderBy(a => a.WarehouseName);
                    break;
            }
            int totalRecords = warehouses.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            warehouses = warehouses.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            warehouseIndexVM.Warehouses = warehouses;
            warehouseIndexVM.CurrentPage = currentPage;
            warehouseIndexVM.TotalPages = totalPages;
            warehouseIndexVM.Term = term;
            warehouseIndexVM.PageSize = pageSize;
            warehouseIndexVM.OrderBy = orderBy;
            return View(warehouseIndexVM);


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
                    try
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
                return View(warehouse);
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
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            try
            {


                Warehouse warehouseToBeDeleted = _unitOfWork.Warehouse.Get(u => u.Id == id);
                if (warehouseToBeDeleted == null)
                {
                    TempData["error"] = "warehouse can't be Delete.";
                    return RedirectToAction("Index");
                }

                _unitOfWork.Warehouse.Remove(warehouseToBeDeleted);
                _unitOfWork.Save();
                TempData["success"] = "warehouse Deleted successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                LogErrorToDatabase(ex);

                TempData["error"] = "error accured";
                // return View(brand);
                return RedirectToAction("Error", "Home");
            }

        }



        //#region API CALLS

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    List<Warehouse> objWarehousyList = _unitOfWork.Warehouse.GetAll().ToList();
        //    return Json(new { data = objWarehousyList });
        //}


        //[HttpDelete]
        //public IActionResult Delete(int? id)
        //{
        //    var WarehousyToBeDeleted = _unitOfWork.Warehouse.Get(u => u.Id == id);
        //    if (WarehousyToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }

        //    _unitOfWork.Warehouse.Remove(WarehousyToBeDeleted);
        //    _unitOfWork.Save();

        //    return Json(new { success = true, message = "Delete Successful" });
        //}

        //#endregion
    }
}
