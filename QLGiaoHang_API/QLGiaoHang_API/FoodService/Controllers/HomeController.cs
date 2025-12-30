using Microsoft.AspNetCore.Mvc;

namespace FoodService.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
