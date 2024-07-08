using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using Microsoft.AspNetCore.Identity;

namespace Demo.DAL.Entities
{
    public class Employee: BaseEntity
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public string Address { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Column(TypeName = ("money"))]
        public int Salary { get; set; }
        public bool IsActive { get; set; }
        public DateTime HiringDate { get; set; } = DateTime.Now;
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public string? ImageUrl { get; set; }
    }
}
