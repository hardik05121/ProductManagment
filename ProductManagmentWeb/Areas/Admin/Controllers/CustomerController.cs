

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.ViewModels;
using ProductManagment_Models.Models;

using System.Data;
using Microsoft.AspNetCore.Authorization;

using System.Drawing.Drawing2D;


namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CustomerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public CustomerController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<CustomerMetadata> listCustomer = _unitOfWork.Customer.GetAll(includeProperties: "City,Country,State").ToList();
            //List<Customer> listCustomer = _unitOfWork.Customer.GetAll(includeProperties: "City,Country,State").ToList();

            return View(listCustomer);
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

                Customer = new CustomerMetadata()
            };
            if (id == null || id == 0)
            {
                //create
                return View(customerVM);
            }
            else
            {
                //update
                customerVM.Customer = _unitOfWork.Customer.Get((System.Linq.Expressions.Expression<Func<CustomerMetadata, bool>>)(u => u.Id == id));
                return View(customerVM);
            }

        }

        [HttpPost]

        // jayre image levani hoy tyre IFormFile? file Parameter lai levnu

        public IActionResult Upsert(CustomerVM customerVM, IFormFile? file)
        {
            if (ModelState.IsValid)
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

                if (customerVM.Customer.Id == 0)
                {
                    CustomerMetadata customerObj = _unitOfWork.Customer.Get(u => u.CustomerName == customerVM.Customer.CustomerName);
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
                else
                {
                    CustomerMetadata customerObj = _unitOfWork.Customer.Get(u => u.Id != customerVM.Customer.Id && u.CustomerName == customerVM.Customer.CustomerName);
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

            
                return RedirectToAction("Index");
            }

            return View(customerVM);
        }



        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CustomerMetadata> objCustomerList = _unitOfWork.Customer.GetAll(includeProperties: "City,State,Country").ToList();
            return Json(new { data = objCustomerList });
        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CustomerToBeDeleted = _unitOfWork.Customer.Get((System.Linq.Expressions.Expression<Func<CustomerMetadata, bool>>)(u => u.Id == id));
            if (CustomerToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath =
                           Path.Combine(_webHostEnvironment.WebRootPath,
                            CustomerToBeDeleted.CustomerImage.TrimStart('\\'));

            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Customer.Remove(CustomerToBeDeleted);
            _unitOfWork.Save();

            return Json(new { success = true, message = "Delete Successful" });
        }


        #endregion

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
