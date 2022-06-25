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
    public class AccauntController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        public AccauntController(UserManager<AppUser> _userManager, SignInManager<AppUser> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
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

            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
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

            return View();
        }
    }
}
