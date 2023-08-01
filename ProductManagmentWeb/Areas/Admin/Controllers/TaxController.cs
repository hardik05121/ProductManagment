using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Data;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using ProductManagment_Models.ViewModels;
using System.Data;
using System.Drawing.Drawing2D;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class TaxController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ApplicationDbContext _db;
        public TaxController(IUnitOfWork unitOfWork, ApplicationDbContext db)
        {
            _unitOfWork = unitOfWork;
            _db = db;
        }

        //#region Index
        //public IActionResult Index()
        //{
        //    List<Tax> objTaxList = _unitOfWork.Tax.GetAll().ToList();

        //    return View(objTaxList);
        //}

        //#endregion


        public IActionResult Index(string term = "", string orderBy = "", int currentPage = 1)
        {
            ViewData["CurrentFilter"] = term;
            term = string.IsNullOrEmpty(term) ? "" : term.ToLower();

            //statePaginationVM.CountryList = _unitOfWork.Country.GetAll().Select(u => new SelectListItem
            //{
            //    Text = u.CountryName,
            //    Value = u.Id.ToString()
            //});


            TaxIndexVM taxIndexVM = new TaxIndexVM();
            taxIndexVM.NameSortOrder = string.IsNullOrEmpty(orderBy) ? "name_desc" : "";
            var taxes = (from data in _unitOfWork.Tax.GetAll().ToList()
                         where term == "" ||
                            data.Name.ToLower().
                            Contains(term)


                         select new Tax
                         {
                             Id = data.Id,
                             Name = data.Name,
                             Percentage = data.Percentage
                         });
            switch (orderBy)
            {
                case "name_desc":
                    taxes = taxes.OrderByDescending(a => a.Name);
                    break;

                default:
                    taxes = taxes.OrderBy(a => a.Name);
                    break;
            }
            int totalRecords = taxes.Count();
            int pageSize = 5;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
            taxes = taxes.Skip((currentPage - 1) * pageSize).Take(pageSize);
            // current=1, skip= (1-1=0), take=5 
            // currentPage=2, skip (2-1)*5 = 5, take=5 ,
            taxIndexVM.Taxes = taxes;
            taxIndexVM.CurrentPage = currentPage;
            taxIndexVM.TotalPages = totalPages;
            taxIndexVM.Term = term;
            taxIndexVM.PageSize = pageSize;
            taxIndexVM.OrderBy = orderBy;
            return View(taxIndexVM);


        }

        #region Upsert
        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new Tax());
            }
            else
            {
                //update
                Tax tax = _unitOfWork.Tax.Get(u => u.Id == id);
                return View(tax);
            }

        }

        [HttpPost]
        public IActionResult Upsert(Tax tax)
        {

            if (ModelState.IsValid)
            {



                if (tax.Id == 0)
                {
                    try
                    {


                        Tax taxObj = _unitOfWork.Tax.Get(u => u.Name == tax.Name);
                        if (taxObj != null)
                        {
                            TempData["error"] = "Tax Name Already Exist!";
                        }
                        else
                        {

                            _unitOfWork.Tax.Add(tax);
                            _unitOfWork.Save();
                            TempData["success"] = "Tax created successfully";
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


                        Tax taxObj = _unitOfWork.Tax.Get(u => u.Id != tax.Id && u.Name == tax.Name);
                        if (taxObj != null)
                        {
                            TempData["error"] = "Tax Name Already Exist!";
                        }
                        else
                        {
                            _unitOfWork.Tax.Update(tax);
                            _unitOfWork.Save();
                            TempData["success"] = "Tax Updated successfully";
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
            else
            {
                return View(tax);
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


                Tax TaxToBeDeleted = _unitOfWork.Tax.Get(u => u.Id == id);
                if (TaxToBeDeleted == null)
                {
                    TempData["error"] = "Tax can't be Delete.";
                    return RedirectToAction("Index");
                }

                _unitOfWork.Tax.Remove(TaxToBeDeleted);
                _unitOfWork.Save();
                TempData["success"] = "Tax Deleted successfully";
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
        //    List<Tax> objTaxList = _unitOfWork.Tax.GetAll().ToList();
        //    return Json(new { data = objTaxList });
        //}


        //[HttpDelete]
        //public IActionResult Delete(int? id)
        //{
        //    var TaxToBeDeleted = _unitOfWork.Tax.Get(u => u.Id == id);
        //    if (TaxToBeDeleted == null)
        //    {
        //        return Json(new { success = false, message = "Error while deleting" });
        //    }

        //    _unitOfWork.Tax.Remove(TaxToBeDeleted);
        //    _unitOfWork.Save();

        //    return Json(new { success = true, message = "Delete Successful" });
        //}

        //#endregion
    }
}
