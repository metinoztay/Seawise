using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seawise.Models;

namespace Seawise.Controllers
{
    public class InspectionController : Controller
    {
        private readonly Db8536Context _context;

        public InspectionController()
        {
            _context = new Db8536Context();
        }
        public IActionResult List(int shipId)
        {
            var inspections = _context.InspectionRecords
                .Include(i => i.InspectionType)
                .Include(i => i.InspectionFindings)
                .Include(i => i.InspectionParticipants).ThenInclude(e => e.Employee)
                .Include(m => m.Ship).ThenInclude(s => s.ShipEquipments).ThenInclude(s => s.MaintenanceRecords)
                .Where(i => i.ShipId == shipId)
                .OrderByDescending(i => i.Time)
                .ToList(); ;

            ViewBag.ActiveTabId = "ShipDetails";
            return View(inspections);
        }
    }
}
