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

        public IDepartmentRepository departmentRepository { get; }

        public IEmployeeRemository employeeRepository { get; }
        public UnitOfWork(CompanyDbContext context)
        {
            _context = context;
            departmentRepository = new DepartmentRepository(_context);
            employeeRepository = new EmployeeRepository(_context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }


        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
    }
}
