using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication34.Models;

namespace WebApplication34.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public HomeController(ILogger<HomeController> logger,UserManager<AppUser> userManager,SignInManager<AppUser>signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            List<AppUser> user = _userManager.Users.ToList();
            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Login(string username,string password)
        {

            AppUser user = await _userManager.FindByNameAsync(username);
            if (User==null)
            {
                return NotFound();
            }
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded)
            {
                return Content("Sifre yalnisdir");
            }



            return RedirectToAction("Index");
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Register(AppUser user,string password)
        {

            var result = await _userManager.CreateAsync(user,password);

            if (!result.Succeeded)
            {
                return Content("User yaradilmadi");
            }

            return RedirectToAction("Login");
        }
    }
}
