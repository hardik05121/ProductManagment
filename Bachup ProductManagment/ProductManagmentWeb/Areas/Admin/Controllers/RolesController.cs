using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductManagment_DataAccess.Data;
using System.Collections.Generic;
using System.Data;


namespace ProductManagment.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles ="Admin")]
    public class RolesController : Controller
    {

        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var roles = _db.Roles.ToList();
            return View(roles);
        }

        [HttpGet]
        public IActionResult Upsert(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return View();
            }
            else
            {
                var objFromDb = _db.Roles.FirstOrDefault(u => u.Id == id);
                return View(objFromDb);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(IdentityRole roleObj)
        {
            if (await _roleManager.RoleExistsAsync(roleObj.Name))
            {
                //errr
                TempData["error"] = "Role Already Exist!";
                return RedirectToAction(nameof(Index));

            }
            if (string.IsNullOrEmpty(roleObj.Id))
            {
                //create

                await _roleManager.CreateAsync(new IdentityRole { Name = roleObj.Name });
                TempData["success"] = "Role created successfully";

            }
            else
            {
                var objFromDb = _db.Roles.FirstOrDefault(u => u.Id == roleObj.Id);
                if (objFromDb == null)
                {
                    TempData["error"] = "Role Not Found";


                }
                objFromDb.Name = roleObj.Name;
                objFromDb.NormalizedName = roleObj.Name.ToUpper();

                var result = await _roleManager.UpdateAsync(objFromDb);
                TempData["success"] = "Role Updated successfully";


            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Delete(string id)
        {
            var objFromDB = _db.Roles.FirstOrDefault(u => u.Id == id);
            if (objFromDB == null)
            {
                TempData["error"] = "Role Not Found";
                return RedirectToAction(nameof(Index));


            }

            var userRoleAssign = _db.UserRoles.Where(w => w.RoleId == id).Count();
            if (userRoleAssign > 0)
            {
                TempData["error"] = "Can not delete Role, this user is assign a role";
                return RedirectToAction(nameof(Index));

            }
            await _roleManager.DeleteAsync(objFromDB);
            TempData["success"] = "Role Deleted Successfully!";

            return RedirectToAction(nameof(Index));


        }
    }
}
