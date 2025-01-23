using BeestjeOpJeFeestje.Models;
using BeestjeOpJeFeestjeDb.Models;
using Bumbo.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeestjeOpJeFeestje.Controllers {

    public class KlantController : Controller {

        private readonly MyContext _context;
        private readonly UserManager<AppUser> _userManager;
        public KlantController(MyContext context, UserManager<AppUser> userManager) {
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index(DateOnly? date) {
            if (date == null) {
                date = DateOnly.FromDateTime(DateTime.Now);
            }
            return View(date);
        }

        public IActionResult IncreaseDate(DateOnly date) {
            date = date.AddDays(1);
            return RedirectToAction("Index", "Klant", new { date });
        }
        public IActionResult DecreaseDate(DateOnly date) {
            date = date.AddDays(-1);
            return RedirectToAction("Index", "Klant", new { date });
        }

        // step 2
        public IActionResult ChooseAnimals(DateOnly date) {
            HttpContext.Session.SetString("Date", date.ToString("yyyy-MM-dd"));

            List<Animal> animals = _context.Animals.ToList();
            // TODO: Filter available animals based on logic
            return View(animals);
        }

        // TODO
        // select animal
        // unselect animal


        public async Task<IActionResult> Authenticate() {
            AppUser? user = await _userManager.GetUserAsync(User);
            if (user == null) {
                return View();
            }
            return RedirectToAction("ConfirmBooking");
        }

        public IActionResult ConfirmBooking() {
            return View();
        }
    }
}
