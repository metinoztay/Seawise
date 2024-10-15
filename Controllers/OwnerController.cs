using Microsoft.AspNetCore.Mvc;
using Seawise.Models;

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
            if (ModelState.IsValid)
            {
                // ownerData ile ilgili işlemleri gerçekleştirin (veritabanına kaydetme vb.)

                // Başarılı yanıt döndürün
                return Json(new { success = true, message = "Data saved successfully!" });
            }

            // Hata olması durumunda JSON hata mesajı döndürün
            return Json(new { success = false, message = "Error saving data!" });
        }
    }
}
