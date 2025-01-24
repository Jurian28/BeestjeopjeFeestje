using BeestjeOpJeFeestje.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace Bumbo.Controllers {
    public class AuthController : Controller {
        private UserManager<AppUser> _userManager;
        private SignInManager<AppUser> _signInManager;
        private MyContext _context;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, MyContext context) {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "boerderij")]
        public IActionResult Register() {
            RegisterForm registerForm = new RegisterForm();
            registerForm.Password = GeneratePassword();

            return View(registerForm);
        }

        [HttpPost]
        [Authorize(Roles = "boerderij")]
        public async Task<IActionResult> Register(RegisterForm registerForm) {
            AppUser user = new AppUser() {
                UserName = registerForm.Name,
                NormalizedUserName = registerForm.Name.ToUpper(),
                Email = registerForm.Email,
                PhoneNumber = registerForm.PhoneNumber,
                Card = registerForm.Card
            };

            var result = await _userManager.CreateAsync(user, registerForm.Password);

            IdentityUserRole<string> role = new IdentityUserRole<string> { UserId = user.Id, RoleId = "2" };
            _context.UserRoles.Add(role);
            _context.SaveChanges();

            if (result.Succeeded) {
                return RedirectToAction("Index", "Boerderij");
            }
            return View();
        }

        private string GeneratePassword() {
            var number = new byte[6];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(number);
            return Convert.ToBase64String(number);
        }

        [HttpGet]
        public IActionResult Login() {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginForm loginForm) {
            var user = await _userManager.FindByEmailAsync(loginForm.Email);
            var result = await _signInManager.PasswordSignInAsync(user,
                   loginForm.Password, true, false);

            if (!result.Succeeded)
                return View();


            if (await _userManager.IsInRoleAsync(user, "boerderij"))
                return RedirectToAction("Index", "Boerderij");

            if (await _userManager.IsInRoleAsync(user, "Klant"))
                return RedirectToAction("Index", "Klant");

            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> Logout() {
            await _signInManager.SignOutAsync();
            return Redirect("/home");
        }
    }
}
