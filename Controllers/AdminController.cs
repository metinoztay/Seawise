using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seawise.Data;
using Seawise.Models;

namespace Seawise.Controllers
{
    
    public class AdminController : Controller
    {
        private readonly Db8536Context _context;

        public AdminController()
        {
            _context = new Db8536Context();
        }
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

        public IActionResult ShipOwners()
        {
            ViewBag.ActiveTabId = "ShipOwners";
            return View();
        }

        
        public IActionResult OwnerProfile(int ownerId)
        {
            ViewBag.ActiveTabId = "OwnerProfile";

            Countries.countries.Clear();
            Countries.countries = _context.Countries.ToList();

            var owner = _context.ShipOwners.Include(o => o.Ships).ThenInclude(o => o.CountryCodeNavigation).FirstOrDefault(o => o.ShipOwnerId == ownerId);
            
            return View(owner);
        }
    }
}
