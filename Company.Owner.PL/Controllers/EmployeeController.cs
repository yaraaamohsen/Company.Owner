using System.Reflection.Metadata;
using AutoMapper;
using Company.Owner.BLL.Interfaces;
using Company.Owner.DAL.Models;
using Company.Owner.PL.Dtos;
using Company.Owner.PL.Helper;
using Microsoft.AspNetCore.Mvc;

namespace Company.Owner.PL.Controllers
{
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
        public IActionResult Index(string? SearchInput)
        {
            IEnumerable<Employee> employees;
            if (String.IsNullOrEmpty(SearchInput))
            {
                employees = _unitOfWork.employeeRemository.GetAll();
            }
            else
            {
                employees = _unitOfWork.employeeRemository.GetByName(SearchInput);  
            }
            //// Dictionary  : this 3 property imherited from Controller Class
            //// 1. ViewData : Transfer Extra Information From Controller (Action) To View
            //ViewData["Message"] = "Hello Form ViewData"; // Set - Get to Update


            //// 2. ViewBag  : Transfer Extra Information From Controller (Action) To View
            //ViewBag.Message = "Hello From ViewBag";
            //// 3. TempData

            return View(employees);
        }
        
        public IActionResult Create()
        {
            var department = _unitOfWork.departmentRepository.GetAll();
            ViewData["department"] = department;
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(CreateEmployeeDto model)
        {
            if(ModelState.IsValid)
            {
                if (model.Image is not null)
                {
                   model.ImageName = DocumentSettings.UploadFile(model.Image, "Images");
                }
                var employee = _Mapper.Map<Employee>(model);
                var count = _unitOfWork.employeeRemository.Add(employee);
                if (count > 0)
                {
                    TempData["Message"] = "Employee Is Created !!";
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult GetEmpById(int? id, string ViewName)
        {
            if (id is null) return BadRequest("Id Is null");

            var employee = _unitOfWork.employeeRemository.GetById(id.Value);
            
            if (employee is null) return NotFound("There is no emplyee matches the ID ");
            
            var department = _unitOfWork.departmentRepository.GetAll();
            ViewData["department"] = department;
            
            if(ViewName == "Edit")
            {
                var createEmployeeDto = _Mapper.Map<CreateEmployeeDto>(employee);
                return View(ViewName, createEmployeeDto);
            }

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
        public IActionResult Edit([FromRoute]int id,CreateEmployeeDto model)
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
                var count = _unitOfWork.employeeRemository.Update(employee);
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

                var count = _unitOfWork.employeeRemository.Delete(model);

                if (count > 0)
                {
                    if(model.ImageName is not null)
                    {
                    DocumentSettings.DeleteFile(model.ImageName, "Images");
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }
    }
}
