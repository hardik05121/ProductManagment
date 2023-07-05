

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public StateController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }

        #region Index
        public IActionResult Index()
        {
            List<State> objStateList = _unitOfWork.State.GetAll(includeProperties: "Country").ToList();

            return View(objStateList);
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
                else
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

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<State> objStateList = _unitOfWork.State.GetAll(includeProperties: "Country").ToList();
            return Json(new { data = objStateList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var stateToBeDeleted = _unitOfWork.State.Get(u => u.Id == id);
            if (stateToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.State.Remove(stateToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
