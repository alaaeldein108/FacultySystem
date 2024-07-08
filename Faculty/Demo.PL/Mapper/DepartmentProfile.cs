using AutoMapper;
using Demo.DAL.Entities;
using Demo.PL.Models;
using System.Runtime.InteropServices;

namespace Demo.PL.Mapper
{
    public class DepartmentProfile : Profile
    {
        public DepartmentProfile() 
        {
            CreateMap<Department,DepartmentViewModel>().ReverseMap();
        }
    }
}
