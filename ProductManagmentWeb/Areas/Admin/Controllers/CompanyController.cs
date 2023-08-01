using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Data;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using ProductManagment_Models.ViewModels;
using System;
using System.Data;
using System.Net;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ApplicationDbContext _db;
        public CompanyController(IUnitOfWork unitOfWork, ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _db = db;
        }

        //#region Index
        //public IActionResult Index()
        //{
        //    List<Company> company = _unitOfWork.Company.GetAll().ToList();

        //    return View(company);
        //}
        //#endregion

        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();




            CompanyIndexVM companyIndexVM = new CompanyIndexVM();
            companyIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "title_desc" : "";
            var companies = (from data in _unitOfWork.Company.GetAll().ToList()
                             where term == "" ||
                                data.Title.ToLower().
                                Contains(term)


                             select new Company
                             {
                                 Id = data.Id,
                                 Title = data.Title,
                                 Currency = data.Currency,
                                 Address = data.Address,
                                 PhoneNumber = data.PhoneNumber,
                                 Email = data.Email,
                                 IsActive = data.IsActive,
                                 CompanyImage = data.CompanyImage
                             });
            switch (orderBy)
            {
                case "brandName_desc":
                    companies = companies.OrderByDescending(a => a.Title);
                    break;

                default:
                    companies = companies.OrderBy(a => a.Title);
                    break;
            }
            int totalRecords = companies.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            companies = companies.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            companyIndexVM.Companies = companies;
            companyIndexVM.CurrentPage = currentPage;
            companyIndexVM.TotalPages = totalPages;
            companyIndexVM.Term = term;
            companyIndexVM.PageSize = pageSize;
            companyIndexVM.OrderBy = orderBy;
            return View(companyIndexVM);


        }


        #region Upsert
        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new CompanyMetadata());
            }
            else
            {
                //update
                Company company = _unitOfWork.Company.Get(u => u.Id == id);
                return View(company);
            }

        }
        [HttpPost]
        public IActionResult Upsert(Company company, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (file != null)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string companyPath = Path.Combine(wwwRootPath, @"images\company");

                        if (!string.IsNullOrEmpty(company.CompanyImage))
                        {
                            //delete the old image
                            var oldImagePath =
                                Path.Combine(wwwRootPath, company.CompanyImage.TrimStart('\\'));

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        using (var fileStream = new FileStream(Path.Combine(companyPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        company.CompanyImage = @"\images\company\" + fileName;
                    }
                }
                catch (Exception ex)
                {
                    LogErrorToDatabase(ex);

                    TempData["error"] = "error accured";
                    // return View(brand);
                    return RedirectToAction("Error", "Home");
                }

                try
                {
                    if (company.Id == 0)
                    {
                        _unitOfWork.Company.Add(company);
                    }
                    else
                    {
                        _unitOfWork.Company.Update(company);
                    }

                    _unitOfWork.Save();
                    TempData["success"] = "Company created successfully";
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
            else
            {

                return View(company);
            }
        }
        #endregion
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


                Company companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
                if (companyToBeDeleted == null)
                {
                    TempData["error"] = "company can't be Delete.";
                    return RedirectToAction("Index");
                }
                var oldImagePath =
                                Path.Combine(_webHostEnvironment.WebRootPath,
                                companyToBeDeleted.CompanyImage.TrimStart('\\'));

                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                _unitOfWork.Company.Remove(companyToBeDeleted);
                _unitOfWork.Save();
                TempData["success"] = "Company Deleted successfully";
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
        //    List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
        //    return Json(new { data = objCompanyList });
        //}


        //[HttpDelete]
        //public IActionResult Delete(int? id)
        //{
        //    var companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
        //    if (companyToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }
        //    var oldImagePath =
        //                   Path.Combine(_webHostEnvironment.WebRootPath,
        //                   companyToBeDeleted.CompanyImage.TrimStart('\\'));

        //    if (System.IO.File.Exists(oldImagePath))
        //    {
        //        System.IO.File.Delete(oldImagePath);
        //    }

        //    _unitOfWork.Company.Remove(companyToBeDeleted);
        //    _unitOfWork.Save();

        //    return Json(new { success = true, message = "Delete Successful" });
        //}

        //#endregion
    }

}
