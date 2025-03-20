using System.Reflection.Metadata.Ecma335;
using AutoMapper;
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
        private readonly IMapper _mapper;

        // Ask CLR Create Instant Object From DepartmentRepository
        public DepartmentController(IDepartmentRepository departmentRepository,
            IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
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
                var department = _mapper.Map<Department>(model);

                var count = _departmentRepository.Add(department);
                if(count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();

        }

        [HttpGet]
        public IActionResult GetDeptData(int? id, string ViewName)
        {
            if (id is null) return BadRequest();
            var department = _departmentRepository.GetById(id.Value);
            if (department is null) return NotFound();

            return View(ViewName,department);
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            return GetDeptData(id, "Details");
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            return GetDeptData(id, "Edit");
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
            return GetDeptData(id, "Delete");
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


        //[HttpGet] // to delete department without get view
        ////[ValidateAntiForgeryToken]
        //public IActionResult Delete([FromRoute] int id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //         var department = _departmentRepository.GetById(id);
        //        if (department is null) return BadRequest();
        //        var count = _departmentRepository.Delete(department);

        //        if (count > 0) return RedirectToAction(nameof(Index));
        //    }
        //    return View("Index");
        //}

    }
}
