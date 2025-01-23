using BeestjeOpJeFeestje.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bumbo.Controllers {
    public class AuthController : Controller {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Authorize(Roles = "boerderij")]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "boerderij")]
        public async Task<IActionResult> Register(RegisterForm registerForm) {
            AppUser user = new AppUser() {
                UserName = registerForm.Name,
                Email = registerForm.Email
            };

            var result = await _userManager.CreateAsync(user, registerForm.Password);

            if (result.Succeeded) {
                return RedirectToAction("Index", "Boerderij");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginForm loginForm) {
            AppUser? user = await _userManager.FindByEmailAsync(loginForm.Email);
            if (user == null) {
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user,
                   loginForm.Password, true, false);

            if (!result.Succeeded)
                return View();

            if (await _userManager.IsInRoleAsync(user, "Boerderij"))
                return RedirectToAction("index", "Boerderij");

            if (await _userManager.IsInRoleAsync(user, "Klant"))
                return RedirectToAction("index", "Klant");

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return Redirect("/home");
        }
    }
}
