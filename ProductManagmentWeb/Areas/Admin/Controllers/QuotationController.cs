﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using ProductManagment_Models.ViewModels;
using System.Data;
using System.Security.Claims;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class QuotationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;
        public QuotationController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            List<Quotation> objQuotationList = _unitOfWork.Quotation.GetAll(includeProperties: "Supplier,User").ToList();
            return View(objQuotationList);
        }
        #region Upsert
        public IActionResult Upsert(int? id)
        {
            QuotationVM quotationVM = new()
            {
                SupplierList = _unitOfWork.Supplier.GetAll().Select(u => new SelectListItem
                {
                    Text = u.SupplierName,
                    Value = u.Id.ToString()
                }),
                UserList = _userManager.Users.Select(x => x.UserName).Select(i => new SelectListItem
                {
                    Text = i,
                    Value = i
                }),
                Quotation = new Quotation(),

                ProductList = _unitOfWork.Product.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                WarehouseList = _unitOfWork.Warehouse.GetAll().Select(u => new SelectListItem
                {
                    Text = u.WarehouseName,
                    Value = u.Id.ToString()
                }),
                UnitList = _unitOfWork.Unit.GetAll().Select(u => new SelectListItem
                {
                    Text = u.BaseUnit,
                    Value = u.Id.ToString()
                }),
                QuotationXproduct = new QuotationXproduct(),
                QuotationXproducts = _unitOfWork.QuotationXproduct.GetAll().ToList(),
                Products = _unitOfWork.Product.GetAll().ToList(),
            };
                return View(quotationVM);
          
          
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
                    State stateObj = _unitOfWork.State.Get(u => u.Id != stateVM.State.Id && u.StateName == stateVM.State.StateName);
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


    }

}
