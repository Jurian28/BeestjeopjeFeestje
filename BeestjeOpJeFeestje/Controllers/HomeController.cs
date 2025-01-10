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
    }
}