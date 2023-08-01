using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Data;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using ProductManagment_Models.ViewModels;
using System.Data;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UnitController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public UnitController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        //#region Index
        //public IActionResult Index()
        //{
        //    List<Unit> objUnitList = _unitOfWork.Unit.GetAll().ToList();
        //    return View(objUnitList);
        //}

        //#endregion

        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

            //statePaginationVM.CountryList = _unitOfWork.Country.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.CountryName,
            //    Value = u.Id.ToString()
            //});


            UnitIndexVM unitIndexVM = new UnitIndexVM();
            unitIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "unitName_desc" : "";
            var units = (from data in _unitOfWork.Unit.GetAll().ToList()
                         where term == "" ||
                            data.UnitName.ToLower().
                            Contains(term)


                         select new Unit
                         {
                             Id = data.Id,
                             UnitName = data.UnitName,
                             UnitCode = data.UnitCode,
                             BaseUnit = data.BaseUnit
                         });
            switch (orderBy)
            {
                case "unitName_desc":
                    units = units.OrderByDescending(a => a.UnitName);
                    break;

                default:
                    units = units.OrderBy(a => a.UnitName);
                    break;
            }
            int totalRecords = units.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            units = units.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            unitIndexVM.Units = units;
            unitIndexVM.CurrentPage = currentPage;
            unitIndexVM.TotalPages = totalPages;
            unitIndexVM.Term = term;
            unitIndexVM.PageSize = pageSize;
            unitIndexVM.OrderBy = orderBy;
            return View(unitIndexVM);


        }

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
                    try
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
                return View(unit);
            }
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id)
        {
            try
            {


                Unit unitToBeDeleted = _unitOfWork.Unit.Get(u => u.Id == id);
                if (unitToBeDeleted == null)
                {
                    TempData["error"] = "Unit can't be Delete.";
                    return RedirectToAction("Index");
                }

                _unitOfWork.Unit.Remove(unitToBeDeleted);
                _unitOfWork.Save();
                TempData["success"] = "Unit Deleted successfully";
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
        //    List<Unit> objUnitList = _unitOfWork.Unit.GetAll().ToList();
        //    return Json(new { data = objUnitList });
        //}


        //[HttpDelete]
        //public IActionResult Delete(int? id)
        //{
        //    var UnitToBeDeleted = _unitOfWork.Unit.Get(u => u.Id == id);
        //    if (UnitToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }

        //    _unitOfWork.Unit.Remove(UnitToBeDeleted);
        //    _unitOfWork.Save();

        //    return Json(new { success = true, message = "Delete Successful" });
        //}

        //#endregion
    }
}
