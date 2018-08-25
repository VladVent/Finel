using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Archive1.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using database.DataModel;
using Archive1.Models.interfases;
namespace Archive1.Controllers
{
    public class ProfileController : Controller
    {
        private UserManager<Users> userManager;
        private SignInManager<Users> signInManager;
        private IGameRepositore db;
        public ProfileController(IGameRepositore _context, UserManager<Users> _userManager, SignInManager<Users> _signInManager)
        {
            db = _context;
            userManager = _userManager;
            signInManager = _signInManager;
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel details, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                Users users = new Users
                {
                    UserName = details.UserName,
                    Name = details.Name
                };
              IdentityResult result = await userManager.CreateAsync(users, details.Password);
               IdentityResult result2 = await userManager.AddToRoleAsync(users, "Users");
                if (result?.Succeeded == true && result2?.Succeeded == true)
                {
                    Microsoft.AspNetCore.Identity.SignInResult result1 = await signInManager.PasswordSignInAsync(users, details.Password, false, false);
                    if (result.Succeeded && result1.Succeeded && result2.Succeeded)
                    {
                        return Redirect(returnUrl ?? "/");
                    }
                }
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(details);
        }
        [HttpGet]
        public IActionResult Register()
        {
            ViewBag.Info = "Введіть свій логін та пароль, щоб увійти у систему";
            return View();
        }
        [HttpGet]
        public IActionResult Login()
        {
            ViewBag.Info = "Введіть свій логін та пароль, щоб увійти у систему";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel details, string returnUrl)
        {
            Users user = await userManager.FindByNameAsync(details.UserName);
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    await signInManager.SignOutAsync();
                    Microsoft.AspNetCore.Identity.SignInResult result = await signInManager.PasswordSignInAsync(user, details.Password, false, false);
                    if (result.Succeeded)
                    {
                        // перевірка на заблокованого якщо заблокованй то окрема вьюшка, на якій кнопка редіректу на головну сторінку
                        // всередині іфки                     await signInManager.SignOutAsync();
                        // елс
                        return Redirect(returnUrl ?? "/");
                    }
                }
                ModelState.AddModelError(nameof(LoginModel.UserName), "Неправильне імя користувача чи пароль!");
            }
            return View(details);
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        /*public IActionResult View(string name)
        {
            var game = from course in db.Games.Include(c => c.GameDev)
                       join dev in db.Games on course.Id equals dev.GameDev.Id
                       where dev.GameDev.Name == name
                       select course;
            return View(game);
        }*/
    }
}
