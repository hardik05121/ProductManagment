

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.ViewModels;
using ProductManagment_Models.Models;

using System.Data;
using Microsoft.AspNetCore.Authorization;

using System.Drawing.Drawing2D;
using ProductManagment_DataAccess.Data;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _db;

        public CustomerController(IUnitOfWork unitOfWork, ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _db = db;
        }
        //public IActionResult Index()
        //{
        //    List<Customer> listCustomer = _unitOfWork.Customer.GetAll(includeProperties: "City,Country,State").ToList();
        //    //List<Customer> listCustomer = _unitOfWork.Customer.GetAll(includeProperties: "City,Country,State").ToList();

        //    return View(listCustomer);
        //}

        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

            //statePaginationVM.CountryList = _unitOfWork.Country.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.CountryName,
            //    Value = u.Id.ToString()
            //});


            CustomerIndexVM customerIndexVM = new CustomerIndexVM();
            customerIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "customerName_desc" : "";
            var customers = (from data in _unitOfWork.Customer.GetAll(includeProperties: "City,Country,State").ToList()
                             where term == "" ||
                                data.CustomerName.ToLower().
                                Contains(term) || data.Country.CountryName.ToLower().Contains(term) || data.State.StateName.ToLower().Contains(term) || data.City.CityName.ToLower().Contains(term)


                             select new Customer
                             {
                                 Id = data.Id,
                                 CustomerName = data.CustomerName,
                                 ContactPerson = data.ContactPerson,
                                 Email = data.Email,
                                 MobileNumber = data.MobileNumber,
                                 WebSite = data.WebSite,
                                 CustomerImage = data.CustomerImage



                             });
            switch (orderBy)
            {
                case "customerName_desc":
                    customers = customers.OrderByDescending(a => a.CustomerName);
                    break;

                default:
                    customers = customers.OrderBy(a => a.CustomerName);
                    break;
            }
            int totalRecords = customers.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            customers = customers.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            customerIndexVM.Customers = customers;
            customerIndexVM.CurrentPage = currentPage;
            customerIndexVM.TotalPages = totalPages;
            customerIndexVM.Term = term;
            customerIndexVM.PageSize = pageSize;
            customerIndexVM.OrderBy = orderBy;
            return View(customerIndexVM);


        }


        public IActionResult Upsert(int? id)
        {
            CustomerVM customerVM = new()
            {
                CityList = _unitOfWork.City.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CityName,
                    Value = u.Id.ToString()
                }),

                CountryList = _unitOfWork.Country.GetAll().Select(u => new SelectListItem
                {
                    Text = u.CountryName,
                    Value = u.Id.ToString()
                }),
                StateList = _unitOfWork.State.GetAll().Select(u => new SelectListItem
                {
                    Text = u.StateName,
                    Value = u.Id.ToString()
                }),

                Customer = new Customer()
            };
            if (id == null || id == 0)
            {
                //create
                return View(customerVM);
            }
            else
            {
                //update
                customerVM.Customer = _unitOfWork.Customer.Get(u => u.Id == id);
                return View(customerVM);
            }

        }

        [HttpPost]



        public IActionResult Upsert(CustomerVM customerVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string customerPath = Path.Combine(wwwRootPath, @"images\customer");

                        if (!string.IsNullOrEmpty((string?)customerVM.Customer.CustomerImage))
                        {
                            //delete the old image
                            var oldImagePath =
                                        Path.Combine(wwwRootPath, (string)customerVM.Customer.CustomerImage.TrimStart('\\'));

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        using (var fileStream = new FileStream(Path.Combine(customerPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        customerVM.Customer.CustomerImage = @"\images\customer\" + fileName;
                    }
                }
                catch (Exception ex)
                {
                    LogErrorToDatabase(ex);

                    TempData["error"] = "error accured";
                    // return View(brand);
                    return RedirectToAction("Error", "Home");
                }


                if (customerVM.Customer.Id == 0)
                {
                    try
                    {


                        Customer customerObj = _unitOfWork.Customer.Get(u => u.CustomerName == customerVM.Customer.CustomerName);
                        if (customerObj != null)
                        {
                            TempData["error"] = "Customer Name Already Exist!";
                        }
                        else
                        {
                            _unitOfWork.Customer.Add(customerVM.Customer);
                            _unitOfWork.Save();
                            TempData["success"] = "Customer created successfully";
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


                        Customer customerObj = _unitOfWork.Customer.Get(u => u.Id != customerVM.Customer.Id && u.CustomerName == customerVM.Customer.CustomerName);
                        if (customerObj != null)
                        {
                            TempData["error"] = "Customer Name Already Exist!";
                        }
                        else
                        {
                            _unitOfWork.Customer.Update(customerVM.Customer);
                            _unitOfWork.Save();
                            TempData["success"] = "Customer created successfully";
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

            return View(customerVM);
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
                Customer customerToBeDeleted = _unitOfWork.Customer.Get(u => u.Id == id);
                if (customerToBeDeleted == null)
                {
                    TempData["error"] = "customer can't be Delete.";
                    return RedirectToAction("Index");
                }
                var oldImagePath =
                                   Path.Combine(_webHostEnvironment.WebRootPath,
                                    customerToBeDeleted.CustomerImage.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                _unitOfWork.Customer.Remove(customerToBeDeleted);
                _unitOfWork.Save();
                TempData["success"] = "customer Deleted successfully";
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
        //    List<Customer> objCustomerList = _unitOfWork.Customer.GetAll(includeProperties: "City,State,Country").ToList();
        //    return Json(new { data = objCustomerList });
        //}


        //[HttpDelete]
        //public IActionResult Delete(int? id)
        //{
        //    var CustomerToBeDeleted = _unitOfWork.Customer.Get(u => u.Id == id);
        //    if (CustomerToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }

        //    var oldImagePath =
        //                   Path.Combine(_webHostEnvironment.WebRootPath,
        //                    CustomerToBeDeleted.CustomerImage.TrimStart('\\'));

        //    if (System.IO.File.Exists(oldImagePath))
        //    {
        //        System.IO.File.Delete(oldImagePath);
        //    }

        //    _unitOfWork.Customer.Remove(CustomerToBeDeleted);
        //    _unitOfWork.Save();

        //    return Json(new { success = true, message = "Delete Successful" });
        //}


        //#endregion

        #region Csacadion Droup down State,country, City
        [HttpGet]
        public IActionResult GetStatesByCountry(int countryId)
        {
            var states = _unitOfWork.State.GetAll(s => s.CountryId == countryId);
            return Json(states);
        }

        [HttpGet]
        public IActionResult GetCitiesByState(int stateId)
        {
            var cities = _unitOfWork.City.GetAll(c => c.StateId == stateId);
            return Json(cities);
        }

        #endregion

    }
}
