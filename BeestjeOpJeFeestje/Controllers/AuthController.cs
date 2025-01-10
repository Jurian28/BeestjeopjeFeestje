using BeestjeOpJeFeestje.Models;
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
        public IActionResult Register() {
            return View();
        }

        public async Task<IActionResult> Register(RegisterForm registerForm) {
            AppUser user = new AppUser() {
                UserName = registerForm.Name,
                Email = registerForm.Email
            };

            //Wat doet de await?
            var result = await _userManager.CreateAsync(user, registerForm.Password);

            //Als het gelukt is om de gebruiker aan te maken, kunnen we hem meteen inloggen.
            if (result.Succeeded) {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Redirect("/home");
            }
            return View();
        }
        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginForm loginForm) {
            var result = await _signInManager.PasswordSignInAsync(loginForm.Username,
                   loginForm.Password, true, false);

            if (result.Succeeded) {
                return Redirect("/Home");
            } else {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return Redirect("/home");
        }
    }
}
