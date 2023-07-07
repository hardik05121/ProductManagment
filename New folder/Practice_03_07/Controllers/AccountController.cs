using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Practice_03_07.Data;
using Practice_03_07.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Practice_03_07.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDb _db;
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(AppDb db, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Register(string returnurl=null)
        {
            if(!await roleManager.RoleExistsAsync(WC.AdminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(WC.AdminRole));
                await roleManager.CreateAsync(new IdentityRole(WC.UserRole));
            }

            ViewData["ReturnUrl"] = returnurl;
            Register register = new Register();
            return View(register);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Register register, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = register.Email, Email = register.Email, Name = register.Name };
                var result = await userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {

                    int count = userManager.Users.Count();

                    if(count==1)
                    {
                        await userManager.AddToRoleAsync(user, WC.AdminRole);
                    }

                    else
                    {
                        await userManager.AddToRoleAsync(user, WC.UserRole);
                    }

                   


                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(HomeController.Index),"Home");
                }
            }
            return View(register);     
        }


        public async Task<IActionResult> Logoff()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Login(Login login,string returnurl=null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(login.Email, login.Password, login.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(HomeController.Index), "Home");

                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invlaid Login attempt");
                    return View(login);
                }
            }
            return View(login);
        }


        public IActionResult AccessDenied()
        {
            return View("Denied");
        }

    }
}
