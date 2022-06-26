using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using taskSquare.Models;
using taskSquare.ViewModels;

namespace taskSquare.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser user = new AppUser
            {
                Name=viewModel.Name,
                Surname=viewModel.Surname,
                Email=viewModel.Email,
                UserName=viewModel.PersonUsername
                
            };

            IdentityResult result = await userManager.CreateAsync(user, viewModel.Password);

            if (!result.Succeeded)
            {
                foreach (IdentityError item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(viewModel);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            AppUser loginUser = await userManager.FindByEmailAsync(viewModel.Email);
            if (loginUser == null)
            {
                ModelState.AddModelError("", "Email or Password is wrong");
                return View(viewModel);
            }

            Microsoft.AspNetCore.Identity.SignInResult result =await signInManager.PasswordSignInAsync(loginUser, viewModel.Password, viewModel.RememberMe, true);

            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "you locked out wait 30 seconds");
                }
                else
                {
                    ModelState.AddModelError("", "Email or Password is wrong");

                }

                return View(viewModel);
            }

            if ((await userManager.GetRolesAsync(loginUser)).Count > 0 && ((await userManager.GetRolesAsync(loginUser))[0] == "Admin"))
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }


        //public async Task<IActionResult> CreateRoles()
        //{
        //    await roleManager.CreateAsync(new IdentityRole() { Name = "Member" });
        //    await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
        //    await roleManager.CreateAsync(new IdentityRole() { Name = "SuperAdmin" });

        //    return Content("Roles Added :)");
        //}


        //public async Task<IActionResult> AddRolesToUsers()
        //{
        //    AppUser user = await userManager.FindByNameAsync("Huseyn");

        //    await userManager.AddToRoleAsync(user, "Admin");
        //    return Content("Ok");
        //}
    }
}
