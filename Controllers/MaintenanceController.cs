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
            var completed = _context.MaintenanceRecords
                .Include(m => m.ShipEquipment)
                    .ThenInclude(e => e.ShipEquipmentCategory)
                .Include(m => m.ShipEquipment)
                    .ThenInclude(e => e.Ship)
                    .ThenInclude(e => e.InspectionRecords).OrderByDescending(ir => ir.Time)                   
                .Where(m => m.ShipEquipment.ShipId == shipId && m.Status == true)
                .OrderByDescending(m => m.Time)          
                .ToList();

            var planned = _context.MaintenanceRecords
                .Include(m => m.ShipEquipment)
                    .ThenInclude(e => e.ShipEquipmentCategory)
                .Include(m => m.ShipEquipment)
                    .ThenInclude(e => e.Ship)
                    .ThenInclude(e => e.InspectionRecords).OrderByDescending(ir => ir.Time)
                .Where(m => m.ShipEquipment.ShipId == shipId && m.Status == false)  
                .OrderBy(m => m.Time)
                .ToList();

            foreach (var c in completed)
            {
                planned.Add(c);
            }

            ViewBag.ActiveTabId = "ShipDetails";
            return View(planned);
        }
    }
}
