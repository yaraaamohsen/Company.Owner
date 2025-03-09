using Company.Owner.BLL.Interfaces;
using Company.Owner.BLL.Reposatories;
using Company.Owner.DAL.Models;
using Company.Owner.PL.Dtos;
using Microsoft.AspNetCore.Mvc;

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
                    return RedirectToAction(nameof(Index ));
                }
            }
            return View();

        }
    }
}
