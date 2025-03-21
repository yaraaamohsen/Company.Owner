using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Company.Owner.BLL.Interfaces;
using Company.Owner.BLL.Reposatories;
using Company.Owner.DAL.Data.Contexts;

namespace Company.Owner.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CompanyDbContext _context;

        public IDepartmentRepository departmentRepository { get; } // NULL

        public IEmployeeRemository employeeRemository { get; } // NULL
        public UnitOfWork(CompanyDbContext context)
        {
            _context = context;
            departmentRepository = new DepartmentRepository(_context);
            employeeRemository = new EmployeeRepository(_context);
        }
    }
}
