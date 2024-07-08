using Demo.BLL.Interfaces;
using Demo.DAL.Context;
using Demo.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Demo.BLL.Repositories
{
    public class EmployeeRepository : GenericRepositiry<Employee>, IEmployeeRepository
    {
        private readonly AppDbContext _context;

        public EmployeeRepository(AppDbContext context):base(context) 
        {
            _context = context;
        }
        public IEnumerable<Employee> GetEmployeesByDepartmentName(string departmentName)
        {
            throw new NotImplementedException();

        }

        public IEnumerable<Employee> Search(string name)
        {
            var result = _context.Employees.Where(x =>
            x.Name.Trim().ToLower().Contains(name.Trim().ToLower()) ||
            x.Email.Trim().ToLower().Contains(name.Trim().ToLower())
            );
            return result;
        }
        /* public int Add(Employee employee)
         {
             _context.Employees.Add(employee);
             return _context.SaveChanges();
         }

         public int Delete(Employee employee)
         {
             _context.Employees.Remove(employee);
             return _context.SaveChanges();
         }

         public IEnumerable<Employee> GetAll()
         {
             return _context.Employees.ToList();
         }

         public Employee GetById(int id)
         {
             return _context.Employees.FirstOrDefault(x => x.Id == id);
         }



         public int Update(Employee employee)
         {
             _context.Employees.Update(employee);
             return _context.SaveChanges();
         }*/

    }
}
