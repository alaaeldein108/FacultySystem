using Demo.DAL.Entities;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly ILogger<RoleController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<ApplicationRole> roleManager,
            ILogger<RoleController> logger,UserManager<ApplicationUser> userManager) 
        {
            _roleManager = roleManager;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string SearchValue = "")
        {
            List<ApplicationRole> roles;
            if (string.IsNullOrEmpty(SearchValue))
            {
                roles = await _roleManager.Roles.ToListAsync();
            }
            else
            {
                roles = await _roleManager.Roles
                    .Where(role => role.Name.ToLower().Trim().Contains(SearchValue.Trim().ToLower())).ToListAsync();
            }
            return View(roles);
        }
        public async Task<IActionResult> Create()
        {
            return View(new ApplicationRole());
        }
        [HttpPost]
        public async Task<IActionResult> Create(ApplicationRole applicationRole)
        {
            var result= await _roleManager.CreateAsync(applicationRole);
            if (result.Succeeded)
            {
                 return RedirectToAction("Index");
            }
            return View(applicationRole);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string id, string viewName = "Details")
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                return View(viewName, role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Update(string id)
        {
            return await Details(id, "Update");
        }
        [HttpPost]
        public async Task<IActionResult> Update(string id, ApplicationRole applicationRole)
        {
            if (id != applicationRole.Id)
            {
                return NotFound();
            }

            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                role.Name = applicationRole.Name;
                role.NormalizedName = applicationRole.Name.ToUpper();
                var result = await _roleManager.UpdateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
            }
            return View(applicationRole);
        }
        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (role is null)
                {
                    return NotFound();
                }
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return NotFound();
            }

            ViewBag.RoleId = roleId;
            var usersInRole=new List<UserInRoleViewModel>();
            var users=await _userManager.Users.ToListAsync();
            foreach(var user in users)
            {
                var userInRole = new UserInRoleViewModel()
                {
                    UserId = user.Id,
                    UserName= user.UserName,
                };
                if(await _userManager.IsInRoleAsync(user,role.Name))
                    userInRole.IsSelected= true;
                else 
                    userInRole.IsSelected= false;
                usersInRole.Add(userInRole);
            }
            return View(usersInRole);
        }
        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId,
            List<UserInRoleViewModel> users )
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser != null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);

                    }
                }
                return RedirectToAction("Update", new { id = roleId });
            }
            return View(users);
        }
    }
}
