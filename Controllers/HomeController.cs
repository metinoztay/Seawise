using Microsoft.AspNetCore.Mvc;

namespace Seawise.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
