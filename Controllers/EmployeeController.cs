using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Seawise.Data;
using Seawise.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Seawise.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly Db8536Context _context;

        public EmployeeController()
        {
            _context = new Db8536Context();
        }

        public IActionResult List()
        {
            DepartmentPositions.departments = _context.EmployeeDepartments.ToList();
            DepartmentPositions.positions = _context.EmployeePositions.ToList();
            ViewBag.ActiveTabId = "EmployeeList";
            return View();
        }

        public IActionResult Details(int employeeId)
        {
            DepartmentPositions.departments = _context.EmployeeDepartments.ToList();
            DepartmentPositions.positions = _context.EmployeePositions.ToList();
            var employee = _context.Employees
                .Include(e => e.EmployeePosition).ThenInclude(p => p.EmployeeDepartment)
                .FirstOrDefault(e => e.EmployeeId == employeeId);

            var maintenanceRecords = _context.MaintenanceRecords
                .Include(mr => mr.ShipEquipment).ThenInclude(e => e.Ship).ThenInclude(s => s.ShipType)
                .Include(mr => mr.ShipEquipment).ThenInclude(e => e.ShipEquipmentCategory)
                .Where(mr => mr.MaintenanceParticipants.Any(mp => mp.EmployeeId == employeeId)).OrderByDescending(mr => mr.Time).ToList();
            var inspectionRecords = _context.InspectionRecords
                .Include(ir => ir.InspectionType)
                .Include(ir => ir.Ship).ThenInclude(s => s.ShipType)
                .Where(ir => ir.InspectionParticipants
                .Any(ip => ip.EmployeeId == employeeId)).OrderByDescending(ip => ip.Time).ToList();

            ViewBag.InspectionRecords = inspectionRecords;
            ViewBag.MaintenanceRecords = maintenanceRecords;

            if (employee == null)
            {
                return NotFound(); // Eğer çalışan bulunamazsa hata dön
            }

            ViewBag.ActiveTabId = "EmployeeDetails";
            return View(employee);
        }

        public IActionResult AddEmployee()
        {
            Random random = new Random();
            int code = random.Next(10000000, 99999999);

            while (_context.Employees.Any(e => e.InternalEmployeeCode == ("SW" + code)))
            {
                code = random.Next(10000000, 99999999);
            }
             
            ViewBag.InternalEmployeeCode = code.ToString();  
            ViewBag.ActiveTabId = "AddEmployee";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewEmployee([FromBody] Employee newEmployee)
        {
            
            newEmployee.InternalEmployeeCode = "SW" + newEmployee.InternalEmployeeCode;
            newEmployee.HireDate = newEmployee.HireDate.AddDays(1);
            newEmployee.Password = newEmployee.Phone;
            _context.Employees.Add(newEmployee);
            await _context.SaveChangesAsync();

            return Ok(new { redirectUrl = $"/Employee/Details?employeeId={newEmployee.EmployeeId}" });
        }

        [HttpPost]
        public IActionResult UpdateEmployee([FromBody] Employee employeeData)
        {
            if (employeeData == null)
            {
                return BadRequest(new { message = "Invalid data." });
            }

            // Veritabanından çalışanı bul
            var employee = _context.Employees
                .FirstOrDefault(e => e.EmployeeId == employeeData.EmployeeId);

            if (employee == null)
            {
                return NotFound(new { message = "Employee not found." });
            }

            // Alanları güncelle
            employee.EmployeePositionId = employeeData.EmployeePositionId;
            employee.Phone = employeeData.Phone;
            employee.Email = employeeData.Email;
            employee.Salary = employeeData.Salary;
            //employee.HireDate = DateOnly.Parse(employeeData.HireDate.ToShortDateString());
            employee.LeaveDate = employeeData.LeaveDate;
            employee.PhotoUrl = employeeData.PhotoUrl;

            // Veritabanına kaydet
            try
            {
                _context.SaveChanges();
                return Ok(new { message = "Employee data updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while saving the data.", details = ex.Message });
            }
        }

        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/employeePhotos");
        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile file, int employeeId)
        {
            // !!!yüklenen ama kaydedilmeyen dosyaların silinmesinin sağlanması gerekiyor
            if (employeeId == null)
                employeeId = 0;

            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }


            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Please upload a file" });

            var validImageTypes = new[] { "image/jpeg", "image/jpg", "image/png" };
            if (!validImageTypes.Contains(file.ContentType))
                return BadRequest(new { message = "Please select only a valid photo file (JPEG, PNG, JPG)." });

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = Path.GetFileName($"employee{employeeId}_{timestamp}{Path.GetExtension(file.FileName)}");
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var image = Image.Load(file.OpenReadStream()))
            {

                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Stretch,
                    Size = new Size(300, 300)
                }));


                var encoder = new JpegEncoder
                {
                    Quality = 75
                };

                await image.SaveAsync(filePath, encoder);
            }


            return Ok(new { filePath = $"/images/employeePhotos/{fileName}" });
        }

        private void DeleteOldPhoto(string photoUrl)
        {

            var fileName = Path.GetFileName(new Uri(photoUrl).LocalPath);
            var filePath = Path.Combine(_uploadPath, fileName);

            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine("Dosya bulunamadı.");
                return;
            }

            try
            {
                System.IO.File.Delete(filePath);
                Console.WriteLine("Dosya başarıyla silindi.");
            }
            catch (IOException ex)
            {
                Console.WriteLine("Dosya silme işlemi başarısız: " + ex.Message);
            }
        }
    }
}
