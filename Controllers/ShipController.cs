using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seawise.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Reflection.Metadata;

namespace Seawise.Controllers
{
    public class ShipController : Controller
    {
        private readonly Db8536Context _context;

        public ShipController()
        {
            _context = new Db8536Context();
        }
        public IActionResult List()
        {
            ViewBag.ActiveTabId = "ShipList";
            return View();
        }

        public IActionResult Details(int shipId)
        {
            var shipDetails = _context.Ships
                .Include(s => s.ShipOwner)
                .Include(s => s.ShipEquipments)
                .ThenInclude(m => m.MaintenanceRecords)
                .Include(s => s.CountryCodeNavigation)
                .FirstOrDefault(x => x.ShipId == shipId);
            ViewBag.ActiveTabId = "ShipDetails";
            return View(shipDetails);
        }

        [HttpPost]
        public async Task<IActionResult> AddNewShip([FromBody] Ship newShip)
        {
            newShip.Imonumber = "IMO" + newShip.Imonumber;
            newShip.LaunchDate = newShip.LaunchDate.AddDays(1);
            _context.Ships.Add(newShip);
            await _context.SaveChangesAsync();

            ViewBag.ActiveTabId = "ShipDetails";
            return Json(new { redirectUrl = Url.Action("Profile", "Owner", new { ownerId = newShip.ShipOwnerId }) });
        
         }

        [HttpPost]
        public async Task<IActionResult> UploadShipProfile([FromBody] Ship newProfile)
        {
            Ship ship = _context.Ships.FirstOrDefault(s => s.ShipId == newProfile.ShipId);

            ship.ShipName = newProfile.ShipName;
            ship.Imonumber = "IMO" + newProfile.Imonumber;
            ship.CountryCode = newProfile.CountryCode;
            ship.PhotoUrl = newProfile.PhotoUrl;

            _context.Entry(ship).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            ViewBag.ActiveTabId = "ShipDetails";
            return Ok();

        }


        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/shipPhotos");
        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile file, int ownerId)
        {
            // !!!yüklenen ama kaydedilmeyen dosyaların silinmesinin sağlanması gerekiyor
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }

            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Lütfen bir dosya yükleyin." });

            var validImageTypes = new[] { "image/jpeg", "image/jpg", "image/png" };
            if (!validImageTypes.Contains(file.ContentType))
                return BadRequest(new { message = "Please select only a valid photo file (JPEG, PNG, JPG)." });

            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            var fileName = Path.GetFileName($"ship{ownerId}_{timestamp}{Path.GetExtension(file.FileName)}");
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var image = Image.Load(file.OpenReadStream()))
            {

                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(640, 428)
                }));


                var encoder = new JpegEncoder
                {
                    Quality = 75
                };

                await image.SaveAsync(filePath, encoder);
            }

            return Ok(new { filePath = $"/images/shipPhotos/{fileName}" });
        }
    }
}
