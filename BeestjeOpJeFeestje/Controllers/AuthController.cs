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
        public IActionResult Login(string returnUrl = null) {
            ViewData["ReturnUrl"] = returnUrl; // Pass returnUrl to the view
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginForm loginForm, string returnUrl = null) {
            AppUser? user = await _userManager.FindByEmailAsync(loginForm.Email);
            if (user == null) {
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user,
                   loginForm.Password, true, false);

            if (!result.Succeeded) { 
                ViewData["ReturnUrl"] = returnUrl;
                return View();
            }

            if (!string.IsNullOrEmpty(returnUrl)) { // go back to the view the user came from
                return Redirect(returnUrl);
            }

            if (await _userManager.IsInRoleAsync(user, "Boerderij"))
                return RedirectToAction("index", "Boerderij");

            if (await _userManager.IsInRoleAsync(user, "Klant"))
                return RedirectToAction("index", "Klant");

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();

            string returnUrl = Request.Headers["Referer"].ToString(); // go back to where the user came from
            if (!string.IsNullOrEmpty(returnUrl)) {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Klant");
        }
    }
}
