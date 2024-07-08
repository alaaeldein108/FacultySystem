﻿using Demo.DAL.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Demo.PL.Models
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Department Name is Required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Department Code is Required")]
        public string Code { get; set; }
        public DateTime CarateAt { get; set; } = DateTime.Now;
    }
}
