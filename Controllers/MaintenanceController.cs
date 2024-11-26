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
            var ship = _context.Ships
               .Include(s => s.ShipEquipments)
               .ThenInclude(e => e.MaintenanceRecords)
               .Include(s => s.InspectionRecords)
               .FirstOrDefault(s => s.ShipId == shipId);

            if (ship == null)
            {
                return NotFound();
            }


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


            ViewBag.Ship = ship;
            ViewBag.ActiveTabId = "ShipDetails";
            return View(planned);
        }

        public async Task<IActionResult> Details(int maintenanceRecordId)
        {
            var maintenanceRecord = await _context.MaintenanceRecords
                .Include(m => m.ShipEquipment)
                    .ThenInclude(e => e.ShipEquipmentCategory)
                .Include(m => m.MaintenanceParticipants)
                    .ThenInclude(p => p.Employee).ThenInclude(e => e.EmployeePosition).ThenInclude(p => p.EmployeeDepartment)
                .FirstOrDefaultAsync(m => m.MaintenanceRecordId == maintenanceRecordId);

            if (maintenanceRecord == null)
            {
                return NotFound();
            }

            int shipId = maintenanceRecord.ShipEquipment.ShipId;
            var ship = _context.Ships
               .Include(s => s.ShipEquipments)
               .ThenInclude(e => e.MaintenanceRecords)
               .Include(s => s.InspectionRecords)
               .FirstOrDefault(s => s.ShipId == shipId);

            ViewBag.Ship = ship;
            ViewBag.ActiveTabId = "ShipDetails";
            return View(maintenanceRecord);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateMaintenance([FromBody] MaintenanceRecord maintenance)
        {
            if (maintenance.MaintenanceRecordId != 0)
            {
                var tempRecord = await _context.MaintenanceRecords.FindAsync(maintenance.MaintenanceRecordId);
                if (tempRecord != null)
                {
                    tempRecord.Description = maintenance.Description;
                    tempRecord.Status = maintenance.Status;

                    _context.Entry(tempRecord).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "Data saved successfully!" });
                }
            }
            return Json(new { success = false, message = "Error saving data!" });
        }



        [HttpPost]
        public async Task<IActionResult> AddMaintenance([FromBody] MaintenanceRecord newRecord)
        {
            if (newRecord.ShipEquipmentId == null || newRecord.Time == null || newRecord == null)
            {
                return null;
            }

            newRecord.Time = newRecord.Time.AddHours(-9);
            newRecord.Status = false;
            await _context.MaintenanceRecords.AddAsync(newRecord);
            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteMaintenance([FromBody] MaintenanceRecord deleteRecord)
        {
            var record = await _context.MaintenanceRecords.FindAsync(deleteRecord.MaintenanceRecordId);
            int eq = record.ShipEquipmentId;
            int tempshipId = _context.ShipEquipments.FirstOrDefault(e => e.ShipEquipmentId == eq).ShipId;

            if (record != null)
            {
                _context.MaintenanceRecords.Remove(record);
                await _context.SaveChangesAsync();

                return Json(new { redirectUrl = Url.Action("List", "Maintenance", new { shipId = tempshipId }) });
            }
            return BadRequest(new { message = "Error deleting record!" });
        }
        
        [HttpPost]
        public async Task<IActionResult> AddParticipant([FromBody] MaintenanceParticipant newParticipant)
        {
            bool isAny = _context.MaintenanceParticipants.Any(m => m.EmployeeId == newParticipant.EmployeeId && m.MaintenanceRecordId == newParticipant.MaintenanceRecordId);
            if (isAny)
            {
               return null;
            }

            newParticipant.StartedAt = DateTime.Now;
            _context.MaintenanceParticipants.AddAsync(newParticipant);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveParticipant([FromBody] MaintenanceParticipant participant)
        {
           var mp = _context.MaintenanceParticipants.FirstOrDefault(m => m.EmployeeId == participant.EmployeeId && m.MaintenanceRecordId == participant.MaintenanceRecordId);
            if (mp != null)
            {
                _context.MaintenanceParticipants.Remove(mp);
                await _context.SaveChangesAsync();
                return Ok();
            }           
            
                return null;
            
        }

    }
}
