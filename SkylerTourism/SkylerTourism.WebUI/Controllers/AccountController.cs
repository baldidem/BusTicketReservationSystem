using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkylerTourism.Core;
using SkylerTourism.WebUI.Identity;
using SkylerTourism.WebUI.Models;

namespace SkylerTourism.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<MyIdentityUser> _userManager;
        private readonly SignInManager<MyIdentityUser> _signInManager;

        public AccountController(UserManager<MyIdentityUser> userManager, SignInManager<MyIdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var myIdentityUser = new MyIdentityUser()
                {
                    FirstName = registerModel.FirstName,
                    LastName = registerModel.LastName,
                    UserName = registerModel.UserName,
                    Email = registerModel.Email
                };
                var result = await _userManager.CreateAsync(myIdentityUser, registerModel.Password);
                if (result.Succeeded)
                {

                    TempData["Message"] = Message.CreateMessage("Notification!", "Your registration has been successfully created.", "success");
                     return Redirect("~/");
                }
            }
            return View(registerModel);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var myIdentityUser = await _userManager.FindByEmailAsync(loginModel.Email);
                if (myIdentityUser == null)
                {
                    TempData["Message"] = Message.CreateMessage("ERROR!", "Email or password is incorrect", "danger");
                    return View(loginModel);
                }
                var result = await _signInManager.PasswordSignInAsync(myIdentityUser, loginModel.Password, loginModel.RememberMe, false);
                if (result.Succeeded)
                {
                    return Redirect("~/");
                }
                TempData["Message"] = Message.CreateMessage("ERROR!", "Email or password is incorrect", "danger");
                return View(loginModel);
            }
            return View(loginModel);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }

    }
}
