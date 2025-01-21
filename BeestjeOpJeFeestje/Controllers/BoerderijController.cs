using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeestjeOpJeFeestje.Controllers {

    [Authorize(Roles = "boerderij")]
    public class BoerderijController : Controller {
        public IActionResult Index() {
            return View();
        }
    }
}
