using BeestjeOpJeFeestje.Models;
using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BeestjeOpJeFeestje.Controllers {
    public class AccountsController : Controller {

        private MyContext _context;
        public AccountsController(MyContext context) { 
            _context = context;
        }
        public IActionResult Index() {
            IEnumerable <AppUser> users = _context.Users;
            List<AppUser> usersForView = new List<AppUser>();
            return View(users);
        }

        public IActionResult SetCard(string id) {
            AppUser? userUpdate = _context.Users.FirstOrDefault(u => u.Id == id);
            if (userUpdate != null) {
                if (userUpdate.Card == "Geen" || userUpdate.Card == null) {
                    userUpdate.Card = "Zilver";
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                if (userUpdate.Card == "Zilver") {
                    userUpdate.Card = "Goud";
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                if (userUpdate.Card == "Goud") {
                    userUpdate.Card = "Platina";
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                if (userUpdate.Card == "Platina") {
                    userUpdate.Card = "Geen";
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
    }
}
