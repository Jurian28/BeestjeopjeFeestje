using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeestjeOpJeFeestje.Controllers {

    [Authorize(Roles = "klant")]
    public class KlantController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
