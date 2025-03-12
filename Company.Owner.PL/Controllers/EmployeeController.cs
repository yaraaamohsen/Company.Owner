using Company.Owner.BLL.Interfaces;
using Company.Owner.DAL.Models;
using Company.Owner.PL.Dtos;
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
        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if(ModelState.IsValid)
            {
                var employee = new Employee()
                {
                    Name = model.Name,
                    Age = model.Age,
                    Address = model.Address,
                    Salary = model.Salary,
                    Email = model.Email,
                    Phone = model.Phone,
                    IsActive = model.IsActive,
                    IsDeleted = model.IsDeleted,
                    HiringDate = model.HiringDate,
                    CreateAt = model.CreateAt
                };
                var count = _employeeRepository.Add(employee);
                if(count > 0) return RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
