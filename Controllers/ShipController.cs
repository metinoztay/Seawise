using Microsoft.AspNetCore.Mvc;
using Seawise.Models;

namespace Seawise.Controllers
{
    public class ShipController : Controller
    {
        private readonly Db8536Context _context;

        public ShipController()
        {
            _context = new Db8536Context();
        }

        [HttpPost]
        public ActionResult Search([FromBody] string searchTerm)
        {
            var results = _context.Ships.Where(s => s.ShipName == (searchTerm)).ToList();

            // Sonuçları bir partial view ile dönün
            return PartialView("_ShipSearchResults", results);
        }
    }
}
