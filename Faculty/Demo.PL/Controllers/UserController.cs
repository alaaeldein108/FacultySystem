using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace Demo.PL.Controllers
{
    [Authorize(Roles ="Admin")]

    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly ILogger<UserController> _logger;

        public UserController(UserManager<ApplicationUser> userManger, ILogger<UserController> logger) 
        {
            _userManger = userManger;
            _logger = logger;
        }
        public async Task<IActionResult> Index(string SearchValue = "")
        {
            List<ApplicationUser> users;
            if (string.IsNullOrEmpty(SearchValue))
            {
                users =await _userManger.Users.ToListAsync();
            }
            else
            {
                users = await _userManger.Users
                    .Where(user=>user.Email.ToLower().Trim().Contains(SearchValue.Trim().ToLower())).ToListAsync();
            }
            return View(users);
        }
        [HttpGet]
        public async Task<IActionResult> Details(string id,string viewName= "Details")
        {
            try
            {
                var user = await _userManger.FindByIdAsync(id);
                return View(viewName, user);
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
        public async Task<IActionResult> Update(string id,ApplicationUser applicationUser)
        {
            if(id != applicationUser.Id) 
            {
                return NotFound();
            }

            try
            {
                var user = await _userManger.FindByIdAsync(id);
                user.UserName = applicationUser.UserName;
                user.NormalizedUserName = applicationUser.UserName.ToUpper();
                var result = await _userManger.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
            }
            return View (applicationUser);
        }
        public async Task<IActionResult> Delete(string id)
        {

            try
            {
                var user = await _userManger.FindByIdAsync(id);
                if(user is null) 
                {
                    return NotFound();
                }
                var result = await _userManger.DeleteAsync(user);
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


    }
}
