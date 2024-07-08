using Demo.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class EmployeeViewModel
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
        public int DepartmentId { get; set; }
        public IFormFile Image { get; set; }
        public string? ImageUrl { get; set; }

    }
}
