using Microsoft.AspNetCore.Mvc;

namespace Seawise.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Dashboard()
        {
            ViewBag.ActiveTabId = "Dashboard";
            return View();
        }

        public IActionResult Ships()
        {
            ViewBag.ActiveTabId = "Ships";
            return View();
        }

        public IActionResult AddShip()
        {
            ViewBag.ActiveTabId = "AddShip";
            return View();
        }
    }
}
