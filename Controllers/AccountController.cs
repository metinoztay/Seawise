using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seawise.Data;
using Seawise.Models;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Seawise.Controllers
{
    public class AccountController : Controller
    {
        private readonly Db8536Context _context;

        public AccountController()
        {
            _context = new Db8536Context();
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string swCode, string password)
        {
            if (swCode == null || password == null)
            {
                return RedirectToAction("Login", "Account");
            }
            swCode = "SW" + swCode.Replace("-","");
            string hashPassword = CreateHash(password);
            var userInformations = _context.Employees.FirstOrDefault(x => x.InternalEmployeeCode == swCode && x.Password == hashPassword);


            if (userInformations != null) //kullanıcı bulundu
            {
                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,swCode),
                };
                var userIdentity = new ClaimsIdentity(claims, "Login"); //kullanıcı kimliği oluşturuldu
                ClaimsPrincipal principal = new ClaimsPrincipal(userIdentity);
                await HttpContext.SignInAsync(principal);


                ActiveUser.activeUser = userInformations;

                return RedirectToAction("List", "Ship", userInformations);
               
            }
            return View();
        }

        //[HttpPost]
        public async Task<IActionResult> Logout()
        {
            ActiveUser.activeUser = new Employee();
            await HttpContext.SignOutAsync();

            return RedirectToAction("Login", "Account");
        }

        public static string CreateHash(string password) //kullanıcı parolarları şifrelendiğinde kullanılacak
        {
            
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] tempString = Encoding.UTF8.GetBytes(password);
            tempString = md5.ComputeHash(tempString);
            StringBuilder sb = new StringBuilder();

            foreach (byte ba in tempString)
            {
                sb.Append(ba.ToString("x2").ToLower());
            }

            return sb.ToString();
        }
    }
}
