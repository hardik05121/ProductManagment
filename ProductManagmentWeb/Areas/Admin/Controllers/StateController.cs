

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class StateController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;

        public StateController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;

        }

        #region Index
        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();



            StateIndexVM stateIndexVM = new StateIndexVM();
            stateIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "stateName_desc" : "";
            var states = (from data in _unitOfWork.State.GetAll(includeProperties: "Country").ToList()
                          where term == "" ||
                             data.StateName.ToLower().
                             Contains(term) || data.Country.CountryName.ToLower().Contains(term)


                          select new State
                          {
                              Id = data.Id,
                              StateName = data.StateName,

                              IsActive = data.IsActive,
                              Country = data.Country
                          });

            switch (orderBy)
            {
                case "stateName_desc":
                    states = states.OrderByDescending(a => a.StateName);
                    break;

                default:
                    states = states.OrderBy(a => a.StateName);
                    break;
            }
            int totalRecords = states.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            states = states.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            stateIndexVM.States = states;
            stateIndexVM.CurrentPage = currentPage;
            stateIndexVM.TotalPages = totalPages;
            stateIndexVM.Term = term;
            stateIndexVM.PageSize = pageSize;
            stateIndexVM.OrderBy = orderBy;
            return View(stateIndexVM);
        }
        #endregion

        #region Upsert
        public IActionResult Upsert(int? id)
        {
            StateVM stateVM = new()
            {
                CountryList = _unitOfWork.Country.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CountryName,
                    Value = u.Id.ToString()
                }),
                State = new State()
            };
            if (id == null || id == 0)
            {
                //create
                return View(stateVM);
            }
            else
            {
                //update
                stateVM.State = _unitOfWork.State.Get(u => u.Id == id);
                return View(stateVM);

            }

            //else
            //{
            //    //update
            //    stateVM.State = _unitOfWork.State.Get(u => u.Id == id, includeProperties: "ProductImages");
            //    return View(stateVM);
            //}

        }
        [HttpPost]
        public IActionResult Upsert(StateVM stateVM)
        {
            if (ModelState.IsValid)
            {


                if (stateVM.State.Id == 0)
                {
                    try
                    {


                        State stateObj = _unitOfWork.State.Get(u => u.StateName == stateVM.State.StateName && u.CountryId == stateVM.State.CountryId);
                        if (stateObj != null)
                        {
                            TempData["error"] = "State Name Already Exist!";
                        }
                        else
                        {
                            _unitOfWork.State.Add(stateVM.State);
                            _unitOfWork.Save();
                            TempData["success"] = "State Created successfully";
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


                        State stateObj = _unitOfWork.State.Get(u => u.Id != stateVM.State.Id && u.StateName == stateVM.State.StateName && u.CountryId == stateVM.State.CountryId);
                        if (stateObj != null)
                        {
                            TempData["error"] = "State Name Already Exist!";
                        }
                        else
                        {
                            _unitOfWork.State.Update(stateVM.State);
                            _unitOfWork.Save();
                            TempData["success"] = "State Updated successfully";
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
                stateVM.CountryList = _unitOfWork.Country.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CountryName,
                    Value = u.Id.ToString()
                });
                return View(stateVM);
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


                State stateToBeDeleted = _unitOfWork.State.Get(u => u.Id == id);
                if (stateToBeDeleted == null)
                {
                    TempData["error"] = "State can't be Delete.";
                    return RedirectToAction("Index");
                }

                _unitOfWork.State.Remove(stateToBeDeleted);
                _unitOfWork.Save();
                TempData["success"] = "State Deleted successfully";
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
        //    List<State> objStateList = _unitOfWork.State.GetAll(includeProperties: "Country").ToList();
        //    return Json(new { data = objStateList });
        //}


        //[HttpDelete]
        //public IActionResult Delete(int? id)
        //{
        //    var stateToBeDeleted = _unitOfWork.State.Get(u => u.Id == id);
        //    if (stateToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }

        //    _unitOfWork.State.Remove(stateToBeDeleted);
        //    _unitOfWork.Save();

        //    return Json(new { success = true, message = "Delete Successful" });
        //}

        //#endregion
    }
}
