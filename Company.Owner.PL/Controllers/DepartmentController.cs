using System.Reflection.Metadata.Ecma335;
using Company.Owner.BLL.Interfaces;
using Company.Owner.BLL.Reposatories;
using Company.Owner.DAL.Models;
using Company.Owner.PL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Company.Owner.PL.Controllers
{
    // MVC Controller
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;

        // Ask CLR Create Instant Object From DepartmentRepository
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet] // GET : Department/index
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            return View(departments); // Sent department As A Model
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var department = new Department()
                {
                    Code = model.Code,
                    Name = model.Name,
                    CreateAt = model.CreateAt,
                };

                var count = _departmentRepository.Add(department);
                if(count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();

        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if(id is null) return BadRequest("Invalid Id");

            var SpecDepartment = _departmentRepository.Get(id.Value);
            if(SpecDepartment is null) return NotFound();   
            
            return View(SpecDepartment);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if(id is null) return BadRequest();
            var department = _departmentRepository.Get(id.Value);
            if(department is null) return NotFound();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int id, Department department)
        {
            if(ModelState.IsValid)
            {
                if(id != department.Id) return BadRequest("Not Selected Id");
                var count = _departmentRepository.Update(department);

                if (count > 0) return RedirectToAction(nameof(Index));

            }
            return View(department);
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id is null) return BadRequest();
            var department = _departmentRepository.Get(id.Value);
            if(department is null) return NotFound();

            return View(department);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                if(id != department.Id) return NotFound("Invalid Id");

                var count = _departmentRepository.Delete(department);

                if (count > 0) return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
    }
}
