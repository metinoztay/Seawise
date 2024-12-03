using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Seawise.Data;
using Seawise.Models;

namespace Seawise.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly Db8536Context _context;

        public AdminController()
        {
            _context = new Db8536Context();
        }        
    }
}
