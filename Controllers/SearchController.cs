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

        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult EmployeeSearch(int departmentId, int positionId, string searchText)
        {
            var employeesQuery = _context.Employees.Include(e => e.EmployeePosition).ThenInclude(e => e.EmployeeDepartment).OrderBy(e => e.HireDate).AsQueryable();

            if (departmentId != 0)
            {
                employeesQuery = employeesQuery.Where(e => e.EmployeePosition.EmployeeDepartmentId == departmentId);
            }

            if (positionId != 0)
            {
                employeesQuery = employeesQuery.Where(e => e.EmployeePositionId == positionId);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                employeesQuery = employeesQuery.Where(e => EF.Functions.Like(e.Name, $"%{searchText}%") || EF.Functions.Like(e.Surname, $"%{searchText}%"));
            }

            var employees = employeesQuery.ToList();

            return PartialView("EmployeeSearchResult", employees);
        }
        

    }
}
