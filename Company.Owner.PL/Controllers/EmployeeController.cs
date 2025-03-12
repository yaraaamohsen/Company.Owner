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

        [HttpGet]
        public IActionResult GetEmpById(int? id, string ViewName)
        {
            if (id is null) return BadRequest("Id Is null");
            var employee = _employeeRepository.GetById(id.Value);
            if (employee is null) return NotFound("There is no emplyee matches the ID ");
            
            return View(ViewName, employee);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            return GetEmpById(id, "Details");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return GetEmpById(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id,Employee model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Not Selected Id");
                var count = _employeeRepository.Update(model);
                if(count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            return GetEmpById(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Employee model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Not Selected Id");

                var count = _employeeRepository.Delete(model);

                if (count > 0) return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
    }
}
