using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public RoleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #region Index
        public IActionResult Index()
        {
            List<Role> objRoleList = _unitOfWork.Role.GetAll().ToList();
            return View(objRoleList);
        }

        #endregion

        #region Upsert
        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new Role());
            }
            else
            {
                //update
                Role role = _unitOfWork.Role.Get(u => u.Id == id);
                return View(role);
            }

        }

        [HttpPost]
        public IActionResult Upsert(Role role)
        {
            if (ModelState.IsValid)
            {

                if (role.Id == 0)
                {
                    _unitOfWork.Role.Add(role);
                    _unitOfWork.Save();
                    TempData["success"] = "role created successfully";
                }
                else
                {
                    _unitOfWork.Role.Update(role);
                    _unitOfWork.Save();
                    TempData["success"] = "Role Updated successfully";
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(role);
            }
        }
        #endregion

        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<Role> objRoleList = _unitOfWork.Role.GetAll().ToList();
            return Json(new { data = objRoleList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var RoleToBeDeleted = _unitOfWork.Role.Get(u => u.Id == id);
            if (RoleToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Role.Remove(RoleToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }

        #endregion
    }
}
