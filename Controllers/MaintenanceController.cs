using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seawise.Models;

namespace Seawise.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly Db8536Context _context;

        public MaintenanceController()
        {
            _context = new Db8536Context();
        }
        public IActionResult List(int shipId)
        {

            // Completed MaintenanceRecords
            var completed = _context.MaintenanceRecords
                .Include(m => m.ShipEquipment)
                    .ThenInclude(e => e.ShipEquipmentCategory)
                .Include(m => m.ShipEquipment)
                    .ThenInclude(e => e.Ship)
                    .ThenInclude(s => s.InspectionRecords)
                .Where(m => m.ShipEquipment.ShipId == shipId && m.Status == true)
                .OrderByDescending(m => m.Time)
                .ToList();

            // Planned MaintenanceRecords
            var planned = _context.MaintenanceRecords
                .Include(m => m.ShipEquipment)
                    .ThenInclude(e => e.ShipEquipmentCategory)
                .Include(m => m.ShipEquipment)
                    .ThenInclude(e => e.Ship)
                    .ThenInclude(s => s.InspectionRecords)
                .Where(m => m.ShipEquipment.ShipId == shipId && m.Status == false)
                .OrderBy(m => m.Time)
                .ToList();



            var temp = _context.Ships
                                  .Include(s => s.ShipEquipments)
                                  .FirstOrDefault(s => s.ShipId == shipId);

            foreach (var c in completed)
            {
                planned.Add(c);
            }

            ViewBag.ActiveTabId = "ShipDetails";
            return View(planned);
        }
    }
}
