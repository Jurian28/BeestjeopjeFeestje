using BeestjeOpJeFeestje.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeestjeOpJeFeestje.Controllers {
    public class HomeController : Controller {
        private readonly MyContext _context;

        public HomeController(MyContext context) {
            _context = context;
        }

        public IActionResult Index() {
            return View();
        }

        [Authorize]
        public IActionResult Geheimen() {
            if (User.IsInRole("SecretKeeper")) {
                return View(_context.Geheimen.ToList());
            } else {
                return View(_context.Geheimen.Where(g => g.SecurityLevel < 3).ToList());
            }
        }
    }
}