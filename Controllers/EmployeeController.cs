using Microsoft.AspNetCore.Mvc;

namespace Seawise.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult List()
        {
            ViewBag.ActiveTabId = "EmployeeList";
            return View();
        }

        public IActionResult AddEmployee()
        {
            ViewBag.ActiveTabId = "AddEmployee";
            return View();
        }
    }
}
