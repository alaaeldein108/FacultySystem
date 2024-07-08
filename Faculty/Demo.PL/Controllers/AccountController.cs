using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Common;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountController> _logger;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,
            ILogger<AccountController> logger,SignInManager<ApplicationUser> signInManager) 
        {
            _userManager = userManager;
            _logger = logger;
			_signInManager = signInManager;
		}
        public IActionResult SignUp()
        {
            return View(new SignUpViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel input)
        {
			
                var user = new ApplicationUser()
                {
                    Email = input.Email,
                    UserName = input.Email.Split('@')[0],
                    IsActive = true
                };
              var result= await _userManager.CreateAsync(user,input.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
               foreach(var error in  result.Errors)
                {
                    _logger.LogError(error.Description);
                    ModelState.AddModelError("", error.Description);
                }
    
            return View(input);
        }
		public IActionResult Login()
		{
			return View(new SignInViewModel());
		}
		[HttpPost]
		public async Task<IActionResult> Login(SignInViewModel input)
		{
			var user = await _userManager.FindByEmailAsync(input.Email);
            if (user is null)
                ModelState.AddModelError("", "Email Doesn't Exist");
            bool IsCorrectPassword=await _userManager.CheckPasswordAsync(user, input.Password);
            if(user != null && IsCorrectPassword)
            {
                var result = await _signInManager.PasswordSignInAsync(user, input.Password,input.RememberMe,false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
			
			return View(input);
		}
		public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }
        public IActionResult ForgetPassword()
        {
            return View(new ForgetPasswordViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user is null)
                ModelState.AddModelError("", "Email Doesn't Exist");
            if(user !=null)
            {
                var token=await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPasswordLink = Url.Action("ResetPassword", "Account", new {Email=input.Email,Token= token },"https");
                var email = new Email()
                {
                    Title = "Reset Password",
                    Body = resetPasswordLink,
                    To= input.Email,
                };
                EmailSettings.SendEmail(email);
                return RedirectToAction("CompleteForgetPassword", "Account");
            }
            return View(input);
        }
        public IActionResult ResetPassword(string email,string token)
        {
            return View(new ResetPasswordViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel input)
        {
            var user = await _userManager.FindByEmailAsync(input.Email);
            if (user is null)
                ModelState.AddModelError("", "Email Doesn't Exist");
            if (user != null) 
            {
                var result = await _userManager.ResetPasswordAsync(user,input.Token,input.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                foreach (var error in result.Errors)
                {
                    _logger.LogError(error.Description);
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(input) ;
        }
        public IActionResult AccessDenied()
        {
            return View();
        } 
    }
}
