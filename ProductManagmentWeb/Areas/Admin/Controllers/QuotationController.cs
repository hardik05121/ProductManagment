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
        public IActionResult Upsert(int? id)
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;


            NewQuotationVM newQuotationVM = new()
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
            return View(newQuotationVM);
        }





        [HttpPost]
        public IActionResult Upsert(NewQuotationVM newQuotationVM)
        {
            //var claimsIdentity = (ClaimsIdentity)User.Identity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            //newQuotationVM.Quotation.UserId = userId;
            newQuotationVM.Quotation.OrderDate = System.DateTime.Now;

            if (ModelState.IsValid)
            {
                if (newQuotationVM.Quotation.Id == 0)
                {
                    Quotation quotation = _unitOfWork.Quotation.Get(u => u.QuotationNumber == newQuotationVM.Quotation.QuotationNumber);

                    if (quotation != null)
                    {
                        TempData["error"] = "quotation Name Already Exist!";
                    }
                    else
                    {

                        _unitOfWork.Quotation.Add(newQuotationVM.Quotation);
                        _unitOfWork.Save();
                        TempData["success"] = "quotation created successfully";
                    }
                }
                else
                {

                    TempData["error"] = "quotation created error";
                }
                if (newQuotationVM != null)
                {
                    foreach (var cart in newQuotationVM.QuotationXproducts)
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
                            QuotationId = newQuotationVM.Quotation.Id,

                        };
                        _unitOfWork.QuotationXproduct.Add(quotationXproduct);
                        _unitOfWork.Save();
                    }
                }


            }
            return RedirectToAction("Index");

        }
        #endregion
        #region Select Product
        [HttpGet]
        public IActionResult GetProduct(int Id)
        {
            var product = _unitOfWork.Product.Get(s => s.Id == Id, includeProperties: "Brand,Category,Unit,Warehouse,Tax");
            return Json(product);
        }

        // above method are work in java script

        [HttpPost]
        //[ParameterAttribute("productList", ",")]
       
        //public IActionResult AddProduct((List<QuotationXproduct> productList)
        //{
        //    try
        //    {
        //        // Store the list in the session
        //          HttpContext.Session.Set("QuotationXproducts", productList);

        //        // Return a JSON response indicating success
        //        return Json(new { success = true });
        //    }
        //    catch (Exception ex)
        //    {
        //        // Return a JSON response indicating failure and the error message
        //        return Json(new { success = false, errorMessage = ex.Message });
        //    }
        //}


        //below method are used in form method.

        //[HttpPost]
        //public ActionResult AddProduct(QuotationVM model, int productId, int warehouseId, int unitId, int taxId, double price, int quantity)
        //{
        //    // Create a new instance of QuotationXproduct
        //    QuotationXproduct quotationXproduct = new QuotationXproduct
        //    {
        //        ProductId = productId,
        //        WarehouseId = warehouseId,
        //        UnitId = unitId,
        //        TaxId = taxId,
        //        Price = price,
        //        Quantity = quantity,
        //        Subtotal = price * quantity
        //    };

        //    // Add the new product to the list
        //    model.QuotationXproducts = model.QuotationXproducts ?? new List<QuotationXproduct>();
        //    ((List<QuotationXproduct>)model.QuotationXproducts).Add(quotationXproduct);                                 

        //    // Get the updated product list as SelectListItem HTML
        //    var updatedProductList = GetProductListAsHtml(model.ProductList);

        //    // Generate the HTML for the new row
        //    var quotationXproductRowHtml = GetNewProductRowHtml(quotationXproduct);

        //    // Return the updated product list and new row HTML as JSON
        //    return Json(new { ProductList = updatedProductList, NewRowHtml = quotationXproductRowHtml });
        //}

        private string GetProductListAsHtml(IEnumerable<SelectListItem> productList)
        {
            // Convert the SelectListItem to HTML options
            var options = new StringBuilder();
            foreach (var product in productList)
            {
                options.AppendFormat("<option value='{0}'>{1}</option>", product.Value, product.Text);
            }

            // Return the HTML options
            return options.ToString();
        }

        private string GetNewProductRowHtml(QuotationXproduct product)
        {
            // Generate the HTML for the new row using the product data
            var rowHtml = "<tr>";

            // Add the cells with the corresponding data
            rowHtml += "<td>" + product.ProductId + "</td>";
            rowHtml += "<td>" + product.WarehouseId + "</td>";
            rowHtml += "<td>" + product.UnitId + "</td>";
            rowHtml += "<td>" + product.TaxId + "</td>";
            rowHtml += "<td>" + product.Price + "</td>";
            rowHtml += "<td>" + product.Quantity + "</td>";
            rowHtml += "<td>" + product.Subtotal + "</td>";

            rowHtml += "</tr>";

            // Return the new row HTML
            return rowHtml;
        }



        #endregion

    }

}
