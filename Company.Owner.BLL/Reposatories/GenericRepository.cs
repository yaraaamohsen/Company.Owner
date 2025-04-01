using System;
using System.Collections;
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
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly CompanyDbContext _context;
        public GenericRepository(CompanyDbContext companyDbContext)
        {
            _context = companyDbContext;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) await _context.employees.Include(E => E.department).ToListAsync();
            }
            return await _context.Set<T>().ToListAsync();
        }
        public async Task<T?> GetByIdAsync(int id)
        {
            if (typeof(T) == typeof(Employee))
            {
                return await _context.employees
                    .Include(E => E.department)
                    .FirstOrDefaultAsync(E => E.Id == id) as T;
            }
            return _context.Set<T>().Find(id);
        }
        public async Task AddAsync(T model)
        {
            await _context.AddAsync(model);
        }
        public void Update(T model)
        {
            _context.Set<T>().Update(model);
        }
        public void Delete(T model)
        {
            _context.Set<T>().Remove(model);
        }

    }
}
