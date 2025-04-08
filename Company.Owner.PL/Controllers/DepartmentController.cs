using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using Company.Owner.BLL.Interfaces;
using Company.Owner.BLL.Reposatories;
using Company.Owner.DAL.Models;
using Company.Owner.PL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Company.Owner.PL.Controllers
{
    [Authorize(Roles = "Admin")]

    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        // Ask CLR Create Instant Object From DepartmentRepository
        public DepartmentController(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet] // GET : Department/index
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.departmentRepository.GetAllAsync();
            return View(departments); // Sent department As A Model
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? SearchInput)
        {
            IEnumerable<Department> departments;
            if (String.IsNullOrEmpty(SearchInput))
            {
                departments = await _unitOfWork.departmentRepository.GetAllAsync();
            }
            else
            {
                departments = await _unitOfWork.departmentRepository.GetByNameAsync(SearchInput);
            }

            if (departments.Any())
            {
                return PartialView("DepartmentPartialView/DepartmentsTablePartialView", departments);
            }

            return Content("<tr><td colspan='15'>No departmens found</td></tr>");
        }



        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateDepartmentDto model)
        {
            if (ModelState.IsValid) // Server Side Validation
            {
                var department = _mapper.Map<Department>(model);

                await _unitOfWork.departmentRepository.AddAsync(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["toastr-success"] = "Department edited successfully!";
                    return RedirectToAction(nameof(Index));
                }
                TempData["toastr-error"] = "Request Failed";

            }
            TempData["toastr-error"] = "Validation errors";
            return View();

        }

        [HttpGet]
        public async Task<IActionResult> GetDeptDataAsync(int? id, string ViewName)
        {
            if (id is null) return BadRequest();
            var department = await _unitOfWork.departmentRepository.GetByIdAsync(id.Value);
            if (department is null) return NotFound();

            return View(ViewName,department);
        }

        [HttpGet]
        public Task<IActionResult> Details(int? id)
        {
            return GetDeptDataAsync(id, "Details");
        }

        [HttpGet]
        public Task<IActionResult> Edit(int? id)
        {
            return GetDeptDataAsync(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id, Department department)
        {
            if(ModelState.IsValid)
            {
                if(id != department.Id) return BadRequest("Not Selected Id");
                _unitOfWork.departmentRepository.Update(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["toastr-success"] = "Department Updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["toastr-error"] = "Validation errors";
            return View(department);
        }

        [HttpGet]
        public Task<IActionResult> Delete(int? id)
        {
            return GetDeptDataAsync(id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, Department department)
        {
            if (ModelState.IsValid)
            {
                if(id != department.Id) return NotFound("Invalid Id");

                _unitOfWork.departmentRepository.Delete(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["toastr-success"] = "Department Deleted successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["toastr-error"] = "Validation errors";
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
