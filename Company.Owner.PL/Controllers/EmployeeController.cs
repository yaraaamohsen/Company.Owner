using Company.Owner.BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Company.Owner.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRemository _employeeRepository;

        public EmployeeController(IEmployeeRemository employeeRepository)
        {
            _employeeRepository = employeeRepository; 
        }
        public IActionResult Index()
        {
            var employees = _employeeRepository.GetAll();
            return View(employees);
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
