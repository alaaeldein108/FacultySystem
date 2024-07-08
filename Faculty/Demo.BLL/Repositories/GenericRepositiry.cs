using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Demo.BLL.Repositories
{
    public class GenericRepositiry<T> : IGenericRepositorty<T> where T : BaseEntity
    {
        private readonly AppDbContext _context;

        public GenericRepositiry(AppDbContext context) 
        {
            _context = context;
        }
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }
            
        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            return _context.Set<T>().Find(id); 
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }

       
    }
}
