using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagment_DataAccess.Data;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using ProductManagment_Models.ViewModels;
using System.Data;
using System.Diagnostics;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _db;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _db = db;
        }

        //#region Index
        //public IActionResult Index()
        //{
        //    List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Brand,Category,Unit,Warehouse,Tax").ToList();
        //    return View(objProductList);
        //}
        //#endregion
        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();



            ProductIndexVM productIndexVM = new ProductIndexVM();
            productIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "name_desc" : "";
            var products = (from data in _unitOfWork.Product.GetAll(includeProperties: "Brand,Category,Unit,Warehouse,Tax").ToList()
                            where term == "" ||
                               data.Name.ToLower().
                               Contains(term) || data.Brand.BrandName.ToLower().Contains(term) || data.Category.Name.ToLower().Contains(term) || data.Unit.UnitName.ToLower().Contains(term) || data.Warehouse.WarehouseName.ToLower().Contains(term) || data.Tax.Name.ToLower().Contains(term)


                            select new Product
                            {
                                Id = data.Id,
                                Name = data.Name,
                                Brand = data.Brand,
                                Category = data.Category,
                                SkuCode = data.SkuName,
                                SkuName = data.SkuName,
                                Mrp = data.Mrp,
                                SalesPrice = data.SalesPrice,
                                Code = data.Code,
                                Unit = data.Unit,
                                BarcodeNumber = data.BarcodeNumber,
                                Tax = data.Tax,
                                PurchasePrice = data.PurchasePrice,
                                Description = data.Description,
                                Warehouse = data.Warehouse,
                                IsActive = data.IsActive,
                                ProductImage = data.ProductImage
                            });

            switch (orderBy)
            {
                case "stateName_desc":
                    products = products.OrderByDescending(a => a.Name);
                    break;

                default:
                    products = products.OrderBy(a => a.Name);
                    break;
            }
            int totalRecords = products.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            products = products.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            productIndexVM.Products = products;
            productIndexVM.CurrentPage = currentPage;
            productIndexVM.TotalPages = totalPages;
            productIndexVM.Term = term;
            productIndexVM.PageSize = pageSize;
            productIndexVM.OrderBy = orderBy;
            return View(productIndexVM);


        }


        #region Upsert

        [HttpGet] // to grt the data on display.
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                BrandList = _unitOfWork.Brand.GetAll().Select(u => new SelectListItem
                {
                    Text = u.BrandName,
                    Value = u.Id.ToString()
                }),
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                UnitList = _unitOfWork.Unit.GetAll().Select(u => new SelectListItem
                {
                    Text = u.BaseUnit,
                    Value = u.Id.ToString()
                }),
                WarehouseList = _unitOfWork.Warehouse.GetAll().Select(u => new SelectListItem
                {
                    Text = u.WarehouseName,
                    Value = u.Id.ToString()
                }),
                TaxList = _unitOfWork.Tax.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };

            if (id == null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }

        }


        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = Path.Combine(wwwRootPath, @"images\product");

                        if (!string.IsNullOrEmpty(productVM.Product.ProductImage))
                        {
                            //delete the old image
                            var oldImagePath =
                                Path.Combine(wwwRootPath, productVM.Product.ProductImage.TrimStart('\\'));

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        productVM.Product.ProductImage = @"\images\product\" + fileName;
                    }

                    //if (productVM.Product.Id == 0)
                    //{
                    //    _unitOfWork.Product.Add(productVM.Product);
                    //    _unitOfWork.Save();
                    //    TempData["success"] = "Product created successfully";
                    //}
                    //else
                    //{
                    //    _unitOfWork.Product.Update(productVM.Product);
                    //    _unitOfWork.Save();
                    //    TempData["success"] = "Product Updated successfully";
                    //}


                    if (productVM.Product.Id == 0)
                    {
                        Product productObj = _unitOfWork.Product.Get(u => u.Name == productVM.Product.Name);
                        if (productObj != null)
                        {
                            TempData["error"] = "Product Name Already Exist!";
                        }
                        else
                        {
                            _unitOfWork.Product.Add(productVM.Product);
                            _unitOfWork.Save();
                            TempData["success"] = "Product created successfully";
                        }
                    }
                    else
                    {
                        Product productObj = _unitOfWork.Product.Get(u => u.Id != productVM.Product.Id && u.Name == productVM.Product.Name);
                        if (productObj != null)
                        {
                            TempData["error"] = "Product Name Already Exist!";
                        }
                        else
                        {
                            _unitOfWork.Product.Update(productVM.Product);
                            _unitOfWork.Save();
                            TempData["success"] = "Product Updated successfully";
                        }

                    }
                    return RedirectToAction("Index");
                }
                else
                {
                    productVM.BrandList = _unitOfWork.Brand.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.BrandName,
                        Value = u.Id.ToString()
                    });
                    productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });
                    productVM.UnitList = _unitOfWork.Unit.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.BaseUnit,
                        Value = u.Id.ToString()
                    });
                    productVM.WarehouseList = _unitOfWork.Warehouse.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.WarehouseName,
                        Value = u.Id.ToString()
                    });
                    productVM.TaxList = _unitOfWork.Tax.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    });
                    return View(productVM);
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
                Product productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
                if (productToBeDeleted == null)
                {
                    TempData["error"] = "Product can't be Delete.";
                    return RedirectToAction("Index");
                }
                var oldImagePath =
                          Path.Combine(_webHostEnvironment.WebRootPath,
                           productToBeDeleted.ProductImage.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                _unitOfWork.Product.Remove(productToBeDeleted);
                _unitOfWork.Save();
                TempData["success"] = "Product Deleted successfully";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                var error = new ErrorLog
                {
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                    ErrorDate = DateTime.Now
                };

                _db.ErrorLogs.Add(error);
                _db.SaveChanges();

                TempData["error"] = "error accured";
                // return View(brand);
                return RedirectToAction("Error", "Home");
            }
        }


        #endregion

        //#region API CALLS

        //[HttpGet]
        //public IActionResult GetAll()
        //{
        //    List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Brand,Category,Unit,Warehouse,Tax").ToList();
        //    return Json(new { data = objProductList });
        //}


        //[HttpDelete]
        //public IActionResult Delete(int? id)
        //{
        //    var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
        //    if (productToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }

        //    var oldImagePath =
        //                   Path.Combine(_webHostEnvironment.WebRootPath,
        //                   productToBeDeleted.ProductImage.TrimStart('\\'));

        //    if (System.IO.File.Exists(oldImagePath))
        //    {
        //        System.IO.File.Delete(oldImagePath);
        //    }

        //    _unitOfWork.Product.Remove(productToBeDeleted);
        //    _unitOfWork.Save();

        //    return Json(new { success = true, message = "Delete Successful" });
        //}


        // #endregion
    }
}