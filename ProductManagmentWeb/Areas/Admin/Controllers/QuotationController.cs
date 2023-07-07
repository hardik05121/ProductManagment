using Microsoft.AspNetCore.Mvc;

namespace ProductManagmentWeb.Areas.Admin.Controllers
{
    public class QuotationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
