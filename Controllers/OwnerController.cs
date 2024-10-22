using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seawise.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using Microsoft.AspNetCore.Components.Forms;
using Seawise.Data;

namespace Seawise.Controllers
{
    public class OwnerController : Controller
    {
        private readonly Db8536Context _context;

        public OwnerController()
        {
            _context = new Db8536Context();
        }

        public IActionResult Profile(int ownerId)
        {
            ViewBag.ActiveTabId = "OwnerProfile";

            Countries.countries.Clear();
            Countries.countries = _context.Countries.ToList();
            ShipTypes.shipTypes.Clear();
            ShipTypes.shipTypes = _context.ShipTypes.OrderBy(s => s.ShipTypeName).ToList();

            var owner = _context.ShipOwners.Include(o => o.Ships).ThenInclude(o => o.CountryCodeNavigation).FirstOrDefault(o => o.ShipOwnerId == ownerId);

            return View(owner);
        }

        public IActionResult List()
        {
            ViewBag.ActiveTabId = "OwnerList";
            return View();
        }


        [HttpPost]
        public IActionResult UpdateShipOwner ([FromBody] ShipOwner owner)
        {
            if (owner.ShipOwnerId != null)
            {
                var tempOwner = _context.ShipOwners.Find(owner.ShipOwnerId);
                tempOwner.Name = owner.Name;
                tempOwner.Email = owner.Email;
                tempOwner.Phone = owner.Phone;
                tempOwner.Address = owner.Address;
                tempOwner.CountryCode = owner.CountryCode;

                DeleteOldPhoto(tempOwner.PhotoUrl);
                tempOwner.PhotoUrl = owner.PhotoUrl;
                
                _context.Entry(tempOwner).State = EntityState.Modified;
                _context.SaveChanges();

                return Json(new { success = true, message = "Data saved successfully!" });
            }

            return Json(new { success = false, message = "Error saving data!" });
        }



        private readonly string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ownerPhotos");
        [HttpPost]
        public async Task<IActionResult> UploadPhoto(IFormFile file, int ownerId)
        {
            // !!!yüklenen ama kaydedilmeyen dosyaların silinmesinin sağlanması gerekiyor
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
            var fileName = Path.GetFileName($"owner{ownerId}_{timestamp}{Path.GetExtension(file.FileName)}");
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var image = Image.Load(file.OpenReadStream()))
            {

                image.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(300, 300)
                }));


                var encoder = new JpegEncoder
                {
                    Quality = 75
                };

                await image.SaveAsync(filePath, encoder);
            }
            

            return Ok(new { filePath = $"/images/ownerPhotos/{fileName}" });
        }

        private void DeleteOldPhoto (string photoUrl)
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
