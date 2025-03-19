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
        public IEnumerable<T> GetAll()
        {
            if(typeof(T) == typeof(Employee))
            {
                return (IEnumerable<T>) _context.employees.Include(E => E.department).ToList();
            }
            return _context.Set<T>().ToList();
        }
        public T GetById(int id)
        {
            return _context.Set<T>().Find(id);
        }
        public int Add(T model)
        {
            _context.Set<T>().Add(model);
            return _context.SaveChanges();
        }
        public int Update(T model)
        {
            _context.Set<T>().Update(model);
            return _context.SaveChanges();
        }
        public int Delete(T model)
        {
            _context.Set<T>().Remove(model);
            return _context.SaveChanges();
        }

    }
}
