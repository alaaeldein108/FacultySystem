using AutoMapper;
using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Reflection.Metadata;

namespace Demo.PL.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IMapper _mapper;

        public EmployeeController(IUnitOfWork unitOfWork, ILogger<EmployeeController> logger,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        
        public IActionResult Index(string SearchValue="")
        {
            IEnumerable<Employee> employees;
            IEnumerable<EmployeeViewModel> employeesViewModel;
            if (string.IsNullOrEmpty(SearchValue)) 
            {
                 employees = _unitOfWork.EmployeeRepository.GetAll();
                employeesViewModel=_mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
            }
            else
            {
                 employees = _unitOfWork.EmployeeRepository.Search(SearchValue);
                employeesViewModel = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);
            }


            return View(employeesViewModel);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Departments=_unitOfWork.DepartmentRepository.GetAll();
            return View(new EmployeeViewModel());
        }
        [HttpPost]
        public IActionResult Create(EmployeeViewModel employeeViewModel)
        {
            if (ModelState.IsValid)
            {
                //Manual MApping
               /* Employee employee = new Employee
                {
                    Name = employeeViewModel.Name,
                    Address = employeeViewModel.Address,
                    DepartmentId= employeeViewModel.DepartmentId,
                    Email = employeeViewModel.Email,
                    HiringDate = employeeViewModel.HiringDate,
                    IsActive = employeeViewModel.IsActive,
                    Salary = employeeViewModel.Salary
                };*/
               var employee=_mapper.Map<Employee>(employeeViewModel);
                employee.ImageUrl = DocumentSetting.UploadFile(employeeViewModel.Image, "Images");
                _unitOfWork.EmployeeRepository.Add(employee);
                _unitOfWork.Complete();
                return RedirectToAction("Index");
            }
            return View(employeeViewModel);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
                var Employee = _unitOfWork.EmployeeRepository.GetById(id);
                var EmployeeViewModel=_mapper.Map<EmployeeViewModel>(Employee);
                return View(EmployeeViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            try
            {
                ViewBag.Departments = _unitOfWork.DepartmentRepository.GetAll();
                var Employee = _unitOfWork.EmployeeRepository.GetById(id);
                var EmployeeViewModel = _mapper.Map<EmployeeViewModel>(Employee);
                
                return View(EmployeeViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public IActionResult Update(EmployeeViewModel employeeViewModel)
        {
            var Employee=_mapper.Map<Employee>(employeeViewModel);
            Employee.ImageUrl = DocumentSetting.UploadFile(employeeViewModel.Image, "Images");
            _unitOfWork.EmployeeRepository.Update(Employee);
            _unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                var Employee = _unitOfWork.EmployeeRepository.GetById(id);
                var EmployeeViewModel = _mapper.Map<EmployeeViewModel>(Employee);
                return View(EmployeeViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);  
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public IActionResult Delete(EmployeeViewModel employeeViewModel)
        {
            string imageName = employeeViewModel.ImageUrl;
            var employee=_mapper.Map<Employee>(employeeViewModel);   
            _unitOfWork.EmployeeRepository.Delete(employee);
            _unitOfWork.Complete();
            DocumentSetting.DeleteFile(imageName, "Images");
            return RedirectToAction(nameof(Index));
        }
    }
}
