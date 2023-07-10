using Microsoft.AspNetCore.Authorization;
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
        public QuotationController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {

        
         
            
                List<Quotation> objQuotationList = _unitOfWork.Quotation.GetAll(includeProperties: "Supplier").ToList();
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
                Quotation = new Quotation()
            };
            return View(quotationVM);
            //if (id == null || id == 0)
            //{
            //    //create
            //    return View(quotationVM);
            //}
            //foreach (var cart in quotationVM.QuotationXproduct)
            //{
            //    cart.Price = GetPriceBasedOnQuantity(cart);
            //    cart.Product.ProductImages = productImages.Where(u => u.ProductId == cart.Product.Id).ToList();
            //    ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            //}

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
