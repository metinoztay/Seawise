using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Seawise.Data;
using Seawise.Models;

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
            DepartmentPrositions.departments = _context.EmployeeDepartments.ToList();
            DepartmentPrositions.positions = _context.EmployeePositions.ToList();
            ViewBag.ActiveTabId = "EmployeeList";
            return View();
        }

        public IActionResult Details(int employeeId)
        {
            var employee = _context.Employees.FirstOrDefault(e => e.EmployeeId == employeeId);

            ViewBag.ActiveTabId = "EmployeeDetails";
            return View(employee);
        }
        public IActionResult AddEmployee()
        {
            ViewBag.ActiveTabId = "AddEmployee";
            return View();
        }
    }
}
