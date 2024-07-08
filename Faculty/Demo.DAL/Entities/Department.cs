using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Demo.DAL.Entities
{
    public class Department: BaseEntity
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Department Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Department Code is Required")]
        public string Code { get; set; }
        public DateTime CarateAt { get; set; } = DateTime.Now;
    }
}
