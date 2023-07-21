using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using ProductManagment_Models.ViewModels;
using ProductManagment_Utility;
using System.Data;
using System.Security.Claims;
using System.Text;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class QuotationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuotationController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        #region Index
        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();



            QuotationIndexVM quotationIndexVM = new QuotationIndexVM();
            quotationIndexVM.SupplierNameSortOrder = string.IsNullOrEmpty(orderBy) ? "supplierName_desc" : "";
            var quotations = (from data in _unitOfWork.Quotation.GetAll(includeProperties: "Supplier").ToList()
                              where term == "" ||
                                 data.Supplier.SupplierName.ToLower().
                                 Contains(term)


                              select new Quotation
                              {
                                  Id = data.Id,
                                  Supplier = data.Supplier,
                                  QuotationNumber = data.QuotationNumber,
                                  OrderDate = data.OrderDate,
                                  DeliveryDate = data.DeliveryDate,
                                  TermCondition = data.TermCondition,
                                  Notes = data.Notes,
                                  ScanBarCode = data.ScanBarCode,
                                  GrandTotal = data.GrandTotal
                              });

            switch (orderBy)
            {
                case "supplierName_desc":
                    quotations = quotations.OrderByDescending(a => a.Supplier.SupplierName);
                    break;

                default:
                    quotations = quotations.OrderBy(a => a.Supplier.SupplierName);
                    break;
            }
            int totalRecords = quotations.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            quotations = quotations.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            quotationIndexVM.Quotations = quotations;
            quotationIndexVM.CurrentPage = currentPage;
            quotationIndexVM.TotalPages = totalPages;
            quotationIndexVM.Term = term;
            quotationIndexVM.PageSize = pageSize;
            quotationIndexVM.OrderBy = orderBy;
            return View(quotationIndexVM);
        }
        #endregion
        #region Upsert
        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            QuotationVM quotationVM = new()
            {
                SupplierList = _unitOfWork.Supplier.GetAll().Select(u => new SelectListItem
                {
                    Text = u.SupplierName,
                    Value = u.Id.ToString()
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
                TaxList = _unitOfWork.Tax.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Percentage.ToString(),
                    Value = u.Id.ToString()
                }),
                QuotationXproduct = new QuotationXproduct(),
            };
            quotationVM.QuotationXproducts = HttpContext.Session.GetComplexData<List<QuotationXproduct>>("loggerUser");

            return View(quotationVM);
        }


        [HttpPost]
        public IActionResult Upsert(QuotationVM quotationVM)
        {

            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            List<QuotationXproduct> quotationXproducts = HttpContext.Session.GetComplexData<List<QuotationXproduct>>("loggerUser");

            //newQuotationVM.Quotation.UserId = userId;
            quotationVM.Quotation.OrderDate = System.DateTime.Now;

            if (ModelState.IsValid)
            {
                if (quotationVM.Quotation.Id == 0)
                {
                    Quotation quotation = _unitOfWork.Quotation.Get(u => u.QuotationNumber == quotationVM.Quotation.QuotationNumber);

                    if (quotation != null)
                    {
                        TempData["error"] = "quotation Name Already Exist!";
                    }
                    else
                    {

                        _unitOfWork.Quotation.Add(quotationVM.Quotation);
                        _unitOfWork.Save();
                        TempData["success"] = "quotation created successfully";
                    }
                }
                else
                {
                    TempData["error"] = "quotation created error";
                }
                if (quotationVM != null)
                {
                    foreach (var cart in quotationXproducts)
                    {
                        QuotationXproduct quotationXproduct = new()
                        {
                            ProductId = cart.ProductId,
                            WarehouseId = cart.WarehouseId,
                            UnitId = cart.UnitId,
                            TaxId = cart.TaxId,
                            Price = cart.Price,
                            Quantity = cart.Quantity,
                            Subtotal = cart.Subtotal,
                            Discount = cart.Discount,
                            QuotationId = quotationVM.Quotation.Id,

                        };
                        _unitOfWork.QuotationXproduct.Add(quotationXproduct);
                        _unitOfWork.Save();
                    }
                }
                return RedirectToAction("Index");
            }
            else

            {
                return View(quotationVM);
            }

        }
        #endregion

        public IActionResult GetProduct(int Id)
        {
            var product = _unitOfWork.Product.Get(s => s.Id == Id, includeProperties: "Brand,Category,Unit,Warehouse,Tax");
            return Json(product);
        }

        // above method are work in java script

        [HttpPost]

        public IActionResult AddProduct(QuotationXproduct product)
        {
            //List<QuotationXproduct> data = new List<QuotationXproduct>();
            //data = productList;
            List<QuotationXproduct> data = new List<QuotationXproduct>();
            data.Add(product);
            try
            {
                List<QuotationXproduct> dataFromSession = new List<QuotationXproduct>();

                dataFromSession = HttpContext.Session.GetComplexData<List<QuotationXproduct>>("loggerUser");

                if (dataFromSession != null)
                {
                    QuotationXproduct quotationXproduct = new QuotationXproduct();
                    quotationXproduct = data.FirstOrDefault();
                    dataFromSession.Add(quotationXproduct);
                    //   HttpContext.Session.SetComplexData("loggerUser", null);
                    HttpContext.Session.SetComplexData("loggerUser", dataFromSession);

                }
                else
                {
                    // HttpContext.Session.SetComplexData("loggerUser", null);
                    HttpContext.Session.SetComplexData("loggerUser", data);
                }
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, errorMessage = ex.Message });
            }
        }

    }

}
