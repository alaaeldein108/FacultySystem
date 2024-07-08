using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Demo.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Demo.BLL.Repositories
{
    public class DepartmentRepository : GenericRepositiry<Department>, IDepartmentRepository
    {

        public DepartmentRepository(AppDbContext context) :base(context)
        {

        }
            
        /*public int Add(Department department)
        {
            _context.Add(department);
            return _context.SaveChanges();
        }


        public int Delete(Department department)
        {
            _context.Remove(department);
            return _context.SaveChanges();
        }

       

        public IEnumerable<Department> GetAll()
        {
            return _context.Departments.ToList();
        }

        public Department GetById(int id)
        {
           return _context.Departments.FirstOrDefault(x=>x.Id == id);
        }

        public int Update(Department department)
        {
            _context.Update(department);
            return _context.SaveChanges();
        }*/

       
    }
}
