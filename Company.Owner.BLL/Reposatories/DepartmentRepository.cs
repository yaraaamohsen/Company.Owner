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
    public class DepartmentRepository : GenericRepository<Department>, IDepartmentRepository
    {
        private readonly CompanyDbContext _context;

        public DepartmentRepository(CompanyDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<IEnumerable<Department>> GetByNameAsync(string name)
        {
            return await _context.Departments
                .Where(D => D.Name.ToLower()
                .Contains(name.ToLower()))
                .ToListAsync();
        }

    }
}
