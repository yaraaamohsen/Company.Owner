using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using AutoMapper;
using Company.Owner.BLL.Interfaces;
using Company.Owner.DAL.Models;
using Company.Owner.PL.Dtos;
using Company.Owner.PL.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.Owner.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _Mapper;

        public EmployeeController(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _Mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (String.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.employeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.employeeRepository.GetByNameAsync(SearchInput);  
            }

            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Search(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (String.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.employeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.employeeRepository.GetByNameAsync(SearchInput);
            }

            if (employees.Any())
            {
                return PartialView("EmployeePartialView/EmployeesTablePartialView", employees);
            }

            return Content("<tr><td colspan='15'>No employees found</td></tr>");
        }

        public async Task<IActionResult> Create()
        {
            var department = await _unitOfWork.departmentRepository.GetAllAsync();
            ViewData["department"] = department;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeDto model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    if (model.Image is not null)
                    {
                        model.ImageName = DocumentSettings.UploadFile(model.Image, "Images");
                    }
                    var employee = _Mapper.Map<Employee>(model);
                    await _unitOfWork.employeeRepository.AddAsync(employee);
                    var count = await _unitOfWork.CompleteAsync();
                    if (count > 0)
                    {
                        TempData["toastr-success"] = "Employee created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (Exception e)
                {
                    TempData["toastr-error"] = $"Error creating employee: {e.Message}";
                }
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetEmpByIdAsync(int? id, string ViewName)
        {
            if (id is null) return BadRequest("Id Is null");

            var employee = await _unitOfWork.employeeRepository.GetByIdAsync(id.Value);
            
            if (employee is null) return NotFound("There is no emplyee matches the ID ");
            
            var department = await _unitOfWork.departmentRepository.GetAllAsync();
            ViewData["department"] = department;
            
            if(ViewName == "Edit")
            {
                var createEmployeeDto = _Mapper.Map<CreateEmployeeDto>(employee);
                return View(ViewName, createEmployeeDto);
            }

            return View(ViewName, employee);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public Task<IActionResult> Details(int? id)
        {
            return GetEmpByIdAsync(id, "Details");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public Task<IActionResult> Edit(int? id)
        {
            return GetEmpByIdAsync(id, "Edit");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public Task<IActionResult> Delete(int? id)
        {
            return GetEmpByIdAsync(id, "Delete");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute]int id,CreateEmployeeDto model)
        {
            if (ModelState.IsValid)
            {
                if(model.ImageName is not null && model.Image is not null)
                {
                    DocumentSettings.DeleteFile(model.ImageName, "Images");
                }
                if(model.Image is not null)
                {
                    model.ImageName = DocumentSettings.UploadFile(model.Image, "Images");
                }
                var employee = _Mapper.Map<Employee>(model);
                employee.Id = id;
                _unitOfWork.employeeRepository.Update(employee);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    TempData["toastr-success"] = "Employee edited successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["toastr-error"] = "Validation errors";
            return View(model);
            }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id, Employee model)
        {
            if (ModelState.IsValid)
            {
                if (id != model.Id) return BadRequest("Not Selected Id");

                _unitOfWork.employeeRepository.Delete(model);

                var count = await _unitOfWork.CompleteAsync();

                if (count > 0)
                {
                    if(model.ImageName is not null)
                    {
                    DocumentSettings.DeleteFile(model.ImageName, "Images");
                    }
                    TempData["toastr-success"] = "Employee Deleted successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            TempData["toastr-error"] = "Error in deletion request";
            return View(model);
        }
    }
}
