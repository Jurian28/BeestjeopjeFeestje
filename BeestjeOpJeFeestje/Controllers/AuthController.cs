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
        public async Task<IActionResult> Register(RegisterForm registerForm) {
            AppUser user = new AppUser() {
                UserName = registerForm.Name,
                NormalizedUserName = registerForm.Name.ToUpper(),
                Address = registerForm.Address,
                Email = registerForm.Email,
                NormalizedEmail = registerForm.Email.ToUpper(),
                PhoneNumber = registerForm.PhoneNumber,
                Card = registerForm.Card
            };

            var result = await _userManager.CreateAsync(user, registerForm.Password);

            if (result.Succeeded) {
                var roleResult = await _userManager.AddToRoleAsync(user, "klant");
                _context.SaveChanges();
                return RedirectToAction("Index", "Boerderij");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserInfo(RegisterForm registerForm, string returnUrl) {
            AppUser user = new AppUser() {
                UserName = registerForm.Name,
                NormalizedUserName = registerForm.Name.ToUpper(),
                Email = registerForm.Email,
                NormalizedEmail = registerForm.Email.ToUpper(),
                PhoneNumber = registerForm.PhoneNumber,
                Address = registerForm.Address,
            };

            Random random = new();
            string _validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-+=<>?";
            string password = new string(Enumerable.Range(0, 12)
                .Select(_ => _validChars[random.Next(_validChars.Length)])
                .ToArray());

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded) {
                await _signInManager.PasswordSignInAsync(user,
                       password, true, false);

                var roleResult = await _userManager.AddToRoleAsync(user, "klant");
                _context.SaveChanges();
            }
            return Redirect(returnUrl);
        }

        private string GeneratePassword() {
            var number = new byte[6];
            var rng = RandomNumberGenerator.Create();
            rng.GetBytes(number);
            return Convert.ToBase64String(number);
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


            if (await _userManager.IsInRoleAsync(user, "boerderij"))
                return RedirectToAction("Index", "Boerderij");

            if (await _userManager.IsInRoleAsync(user, "Klant"))
                return RedirectToAction("Index", "Klant");

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
