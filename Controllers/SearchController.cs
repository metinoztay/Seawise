using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seawise.Models;

namespace Seawise.Controllers
{
    public class SearchController : Controller
    {
        private readonly Db8536Context _context;

        public SearchController()
        {
            _context = new Db8536Context();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] 
        public ActionResult ShipSearch(string searchTerm)
        {   

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                var allShips = _context.Ships
                                    .Include(s => s.ShipOwner) 
                                    .ToList();
                return PartialView("ShipSearchResult", allShips);            }

            var results = _context.Ships
                            .Include(s => s.ShipOwner) 
                            .Where(s => s.ShipName.Contains(searchTerm) || 
                                        s.ShipOwner.Name.Contains(searchTerm) || 
                                        s.Imonumber.Contains(searchTerm)) 
                            .ToList();

            return PartialView("ShipSearchResult", results);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OwnerSearch (string searchTerm)
        {

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                var allOwners = _context.ShipOwners.Include(o => o.CountryCodeNavigation).ToList();
                return PartialView("OwnerSearchResult", allOwners);
            }

            var owners = _context.ShipOwners
                            .Include(o => o.CountryCodeNavigation)
                            .Where(s =>  s.Name.Contains(searchTerm) ||
                                         s.Phone.Contains(searchTerm))
                            .ToList();

            return PartialView("OwnerSearchResult", owners);
        }

    }
}
