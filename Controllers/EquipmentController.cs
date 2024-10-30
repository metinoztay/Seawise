using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seawise.Models;

namespace Seawise.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly Db8536Context _context;

        public EquipmentController()
        {
            _context = new Db8536Context();
        }
        public IActionResult List(int shipId)
        {
            var ship = _context.Ships
                .Include(s => s.ShipEquipments)
                    .ThenInclude(se => se.ShipEquipmentCategory)
                .Include(s => s.InspectionRecords.OrderByDescending(ir => ir.Time))
                .Include(s => s.ShipEquipments)
                    .ThenInclude(se => se.MaintenanceRecords.OrderByDescending(mr => mr.Time))
                .FirstOrDefault(s => s.ShipId == shipId);

            ViewBag.ActiveTabId = "ShipDetails";
            return View(ship);
        }
    }
}
