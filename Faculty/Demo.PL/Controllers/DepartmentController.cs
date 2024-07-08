using Demo.BLL.Interfaces;
using Demo.DAL.Entities;
using Demo.BLL.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using AutoMapper;
using Demo.PL.Models;

namespace Demo.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IMapper _mapper;

        public DepartmentController(IUnitOfWork unitOfWork,ILogger<DepartmentController> logger,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Department> departments;
            IEnumerable<DepartmentViewModel> departmentViewModels;
             departments= _unitOfWork.DepartmentRepository.GetAll();
            departmentViewModels = _mapper.Map<IEnumerable<DepartmentViewModel>>(departments);
            ViewBag.Message = "Hello Alaa";

            return View(departmentViewModels);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new DepartmentViewModel());
        }
        [HttpPost]
        public IActionResult Create(DepartmentViewModel departmentViewModel)
        {
            
            if (ModelState.IsValid)
            {
                var department=_mapper.Map<Department>(departmentViewModel);
                _unitOfWork.DepartmentRepository.Add(department);
                _unitOfWork.Complete();
                return RedirectToAction(nameof(Index));
            }
            return View(departmentViewModel);
        }
        [HttpGet]
        public IActionResult Details(int id)
        {
            try
            {
               var Department= _unitOfWork.DepartmentRepository.GetById(id);
                var departmentViewModel = _mapper.Map<DepartmentViewModel>(Department);
                return View(departmentViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error","Home");
            }
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            try
            {
                var Department = _unitOfWork.DepartmentRepository.GetById(id);
                var departmentViewModel = _mapper.Map<DepartmentViewModel>(Department);
                return View(departmentViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public IActionResult Update(DepartmentViewModel  departmentViewModel)
        {
            var department = _mapper.Map<Department>(departmentViewModel);
            _unitOfWork.DepartmentRepository.Update(department);
            _unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                var Department = _unitOfWork.DepartmentRepository.GetById(id);
                var departmentViewModel = _mapper.Map<DepartmentViewModel>(Department);
                return View(departmentViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpPost]
        public IActionResult Delete(DepartmentViewModel departmentViewModel)
        {
            var department= _mapper.Map<Department>(departmentViewModel);
            _unitOfWork.DepartmentRepository.Delete(department);
            _unitOfWork.Complete();
            return RedirectToAction(nameof(Index));
        }

    }
}
