using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seawise.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using Microsoft.AspNetCore.Components.Forms;

namespace Seawise.Controllers
{
    public class OwnerController : Controller
    {
        private readonly Db8536Context _context;

        public OwnerController()
        {
            _context = new Db8536Context();
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public IActionResult UpdateShipOwner (ShipOwner owner)
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

                // Başarılı yanıt döndürün
                return Json(new { success = true, message = "Data saved successfully!" });
            }

            // Hata olması durumunda JSON hata mesajı döndürün
            return Json(new { success = false, message = "Error saving data!" });
        }



        string _uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/ownerPhotos");
        [HttpPost("UploadPhoto")]
        public async Task<IActionResult> UploadPhoto (IFormFile file, int ownerId)
        {
            
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Lütfen bir dosya yükleyin." });

            // Geçerli bir fotoğraf türü olup olmadığını kontrol edin
            var validImageTypes = new[] { "image/jpeg", "image/jpg", "image/png" };
            if (!validImageTypes.Contains(file.ContentType))
                return BadRequest(new { message = "Please select only a valid photo file (JPEG, PNG, JPG)." });

            // Dosya adını oluştur
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
            

            // Yüklenen dosyanın yolunu geri döndürün
            return Ok(new { filePath = $"/images/ownerPhotos/{fileName}" });
        }

        private void DeleteOldPhoto (string photoUrl)
        {
            // URL'den dosya adını ayıkla
            var fileName = Path.GetFileName(new Uri(photoUrl).LocalPath);

            // Sunucudaki dosya yolunu oluştur
            var filePath = Path.Combine(_uploadPath, fileName);

            // Dosyanın var olup olmadığını kontrol et
            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine("Dosya bulunamadı.");
                return;
            }

            try
            {
                // Dosyayı sil
                System.IO.File.Delete(filePath);
                Console.WriteLine("Dosya başarıyla silindi.");
            }
            catch (IOException ex)
            {
                // Dosya silme işlemi sırasında bir hata meydana geldiğinde
                Console.WriteLine("Dosya silme işlemi başarısız: " + ex.Message);
            }
        }
    }
}
