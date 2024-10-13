using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [ValidateAntiForgeryToken] 
        public ActionResult Search(string searchTerm)
        { // gelen verinin ımonumarası na ve owner ismine göre de aranması sağlanacak
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                var allShips = _context.Ships
                                    .Include(s => s.ShipOwner) // Owner tablosunu dahil ediyoruz
                                    .ToList();
                return PartialView("_ShipSearchResults", allShips);
            }

            var results = _context.Ships.Where(s => s.ShipName.Contains(searchTerm)).Include(s => s.ShipOwner).ToList();

            return PartialView("_ShipSearchResults", results);
        }

    }
}
