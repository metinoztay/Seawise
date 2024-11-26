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
            var ship = _context.Ships
                .Include(s => s.ShipEquipments)
                .ThenInclude(e => e.MaintenanceRecords)
                .Include(s => s.InspectionRecords)
                .FirstOrDefault(s => s.ShipId == shipId);

            if (ship == null)
            {
                return NotFound();
            }

            var inspections = _context.InspectionRecords
                .Include(i => i.InspectionType)
                .Include(i => i.InspectionFindings)
                .Include(i => i.InspectionParticipants).ThenInclude(e => e.Employee)
                .Include(m => m.Ship).ThenInclude(s => s.ShipEquipments).ThenInclude(s => s.MaintenanceRecords)
                .Where(i => i.ShipId == shipId)
                .OrderByDescending(i => i.Time)
                .ToList(); 


            ViewBag.Ship = ship;
            ViewBag.ActiveTabId = "ShipDetails";
            return View(inspections);
        }

        public async Task<IActionResult> Details(int? inspectionId)
        {
            if (inspectionId == null)
                return NotFound();

            var inspectionRecord = await _context.InspectionRecords
                .Include(i => i.Ship)
                .Include(i => i.InspectionType)
                .Include(i => i.InspectionFindings)
                    .ThenInclude(f => f.InspectionCriteria)
                .Include(i => i.InspectionParticipants)
                    .ThenInclude(p => p.Employee)
                    .ThenInclude(e => e.EmployeePosition)
                    .ThenInclude(ep => ep.EmployeeDepartment)
                .FirstOrDefaultAsync(m => m.InspectionRecordId == inspectionId);

            if (inspectionRecord == null)
                return NotFound();
            ViewBag.InspectionCriteriaList = await _context.InspectionCriterias.ToListAsync();

            var ship = _context.Ships
                .Include(s => s.ShipEquipments)
                .ThenInclude(e => e.MaintenanceRecords)
                .Include(s => s.InspectionRecords)
                .FirstOrDefault(s => s.ShipId == inspectionRecord.ShipId);

            ViewBag.Ship = ship;
            ViewBag.ActiveTabId = "ShipDetails";
            return View(inspectionRecord);
        }


        [HttpPost]
        public async Task<IActionResult> AddFinding([FromBody] InspectionFinding newFinding)
        {
            if (newFinding == null || newFinding.InspectionRecordId <= 0 || newFinding.InspectionCriteriaId <= 0)
                return BadRequest("Invalid data");

            try
            {
                // Yeni bulgu oluştur ve veritabanına ekle
                var finding = new InspectionFinding
                {
                    InspectionRecordId = newFinding.InspectionRecordId,
                    InspectionTypeId = newFinding.InspectionTypeId,
                    InspectionCriteriaId = newFinding.InspectionCriteriaId,
                    Severity = newFinding.Severity,
                    Description = newFinding.Description
                };

                _context.InspectionFindings.Add(finding);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Finding added successfully" });
            }
            catch
            {
                return StatusCode(500, "Error saving finding");
            }
        }




        [HttpDelete]
        public async Task<IActionResult> DeleteFinding([FromBody] InspectionFinding deleteFinding)
        {
            if (deleteFinding == null || deleteFinding.InspectionFindingId <= 0)
                return BadRequest("Invalid finding data");

            try
            {
                var finding = await _context.InspectionFindings.FindAsync(deleteFinding.InspectionFindingId);
                if (finding == null)
                    return NotFound("Finding not found");

                _context.InspectionFindings.Remove(finding);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Finding deleted successfully" });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, $"Error deleting finding: {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddParticipant([FromBody] InspectionParticipant newParticipant)
        {
            bool isAny = _context.InspectionParticipants.Any(m => m.EmployeeId == newParticipant.EmployeeId && m.InspectionRecordId == newParticipant.InspectionRecordId);
            if (isAny)
            {
                return null;
            }

            _context.InspectionParticipants.AddAsync(newParticipant);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveParticipant([FromBody] InspectionParticipant participant)
        {
            var p = _context.InspectionParticipants.FirstOrDefault(m => m.EmployeeId == participant.EmployeeId && m.InspectionRecordId == participant.InspectionRecordId);
            if (p != null)
            {
                _context.InspectionParticipants.Remove(p);
                await _context.SaveChangesAsync();
                return Ok();
            }
            return null;

        }

    }
}
