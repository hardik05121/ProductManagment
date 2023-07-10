using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Repository.IRepository;
using ProductManagment_Models.Models;
using System.Diagnostics;

namespace ProductManagmentWeb.Areas.Customers.Controllers
{

    [Area("Customers")]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Brand,Category,Unit,Warehouse,Tax").ToList();
            return View(objProductList);
        }
    }
}