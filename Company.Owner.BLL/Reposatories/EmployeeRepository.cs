using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Owner.BLL.Interfaces;
using Company.Owner.DAL.Data.Contexts;
using Company.Owner.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Company.Owner.BLL.Reposatories
{
    public class EmployeeRepository : IEmployeeRemository
    {
        private readonly CompanyDbContext _context;
        public EmployeeRepository(CompanyDbContext companyDbContext)
        {
            _context = companyDbContext;
        }
        public IEnumerable<Employee> GetAll()
        {
            return _context.employees.ToList();
        }
        public Employee GetById(int id)
        {
            return _context.employees.Find(id);
        }
        public int Add(Employee employee)
        {
            _context.employees.Add(employee);
            return _context.SaveChanges();
        }
        public int Update(Employee employee)
        {
            _context.employees.Update(employee);
            return _context.SaveChanges();
        }
        public int Delete(Employee employee)
        {
            _context.employees.Remove(employee);
            return _context.SaveChanges();
        }
    }
}
