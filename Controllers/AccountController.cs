using Microsoft.AspNetCore.Mvc;

namespace Seawise.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
