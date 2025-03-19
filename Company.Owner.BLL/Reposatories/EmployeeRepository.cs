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
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRemository
    {
        public CompanyDbContext _context { get; }

        public EmployeeRepository(CompanyDbContext context) : base(context)
        {
            _context = context;
        }

        public List<Employee> GetByName(string name)
        {
            return _context.employees.Include(E=> E.department).Where(E => E.Name.ToLower().Contains(name.ToLower())).ToList();
        }
    }
}
