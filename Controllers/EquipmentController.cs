using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using Microsoft.EntityFrameworkCore;
using Seawise.Data;
using Seawise.Models;
using SixLabors.ImageSharp.Formats.Jpeg;

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

        public IActionResult Details(int equipmentId)
        {
            var equipment = _context.ShipEquipments
                .Include(e => e.ShipEquipmentCategory)
                .Include(e => e.Ship)
                .Include(e => e.Ship.InspectionRecords.OrderByDescending(ir => ir.Time))
                .Include(s => s.Ship.ShipEquipments)
                    .ThenInclude(se => se.MaintenanceRecords.OrderByDescending(mr => mr.Time))
                .FirstOrDefault(e => e.ShipEquipmentId == equipmentId);

            EquipmentCategories.equipmentCategories.Clear();
            EquipmentCategories.equipmentCategories = _context.ShipEquipmentCategories.ToList();

            ViewBag.ActiveTabId = "ShipDetails";
            return View(equipment);
        }


        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/equipmentPhotos");
        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile file, int equipmentId)
        {
            // !!!yüklenen ama kaydedilmeyen dosyaların silinmesinin sağlanması gerekiyor
            if (equipmentId == null)
                equipmentId = 0;

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
            var fileName = Path.GetFileName($"eq{equipmentId}_{timestamp}{Path.GetExtension(file.FileName)}");
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var image = Image.Load(file.OpenReadStream()))
            {

                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Stretch,
                    Size = new Size(640, 428)
                }));


                var encoder = new JpegEncoder
                {
                    Quality = 75
                };

                await image.SaveAsync(filePath, encoder);
            }


            return Ok(new { filePath = $"/images/equipmentPhotos/{fileName}" });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateEquipment([FromBody] ShipEquipment equipment)
        {
            ///model null geliyor!!!!!!!!!!!!!
            if (equipment.ShipEquipmentId != null)
            {
                var tempEquipment = _context.ShipEquipments.Find(equipment.ShipEquipmentId);
                tempEquipment.ShipEquipmentCategoryId = equipment.ShipEquipmentCategoryId;
                tempEquipment.Status = equipment.Status;
                tempEquipment.Description = equipment.Description;
                tempEquipment.EquipmentName = equipment.EquipmentName;


                DeleteOldPhoto(tempEquipment.PhotoUrl);
                tempEquipment.PhotoUrl = equipment.PhotoUrl;

                _context.Entry(tempEquipment).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Data saved successfully!" });
            }

            return Json(new { success = false, message = "Error saving data!" });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteEquipment([FromBody] ShipEquipment deleteEquipment)
        {

            return Ok();

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
