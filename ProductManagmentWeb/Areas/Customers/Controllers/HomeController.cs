using Microsoft.AspNetCore.Mvc;
using ProductManagment_DataAccess.Data;
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
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork,ApplicationDbContext db)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Brand,Category,Unit,Warehouse,Tax").ToList();
            return View(objProductList);
        }

        public IActionResult Error() 
        {
            return View();
        }

        //public IActionResult Get()
        //{
        //    _logger.LogInfo("Fetching all the Students from the storage");

        //    var students = DataManager.GetAllStudents(); //simulation for the data base access

        //    throw new Exception("Exception while fetching all the students from the storage.");

        //    _logger.LogInfo($"Returning {students.Count} students.");

        //    return Ok(students);
        //}
        // Other actions in the controller
    }
}
