using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BeestjeOpJeFeestje.Controllers {

    [Authorize(Roles = "boerderij")]
    public class BoerderijController : Controller {
        private MyContext _context;

        public BoerderijController(MyContext context) {
            _context = context;
        }
        public IActionResult Index() {
            IEnumerable<Animal> animals = _context.Animals;
            return View(animals);
        }

        [HttpGet]
        public IActionResult Create() {
            Animal newAnimal = new Animal();
            return View(newAnimal);
        }
        [HttpPost]
        public IActionResult Create(Animal newAnimal) {
            if (ModelState.IsValid) {
                try {
                    _context.Animals.Add(newAnimal);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                } catch {
                    return View(newAnimal);
                }
            }
            return View(newAnimal);
        }
        [HttpGet]
        public IActionResult Edit(int id) {
            Animal? animal = _context.Animals.FirstOrDefault(a => a.Id == id);
            if (animal != null) {
                return View(animal);
            } else {
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Edit(Animal animal) {
            if (ModelState.IsValid) {
                Animal? animalUpdate = _context.Animals.FirstOrDefault(a => a.Id == animal.Id);
                if (animalUpdate != null) {
                    _context.Entry(animalUpdate).CurrentValues.SetValues(animal);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                } else {
                    return View();
                }
            }
            return View(animal);
        }

        public IActionResult Delete(int id) {
            Animal? animal = _context.Animals.FirstOrDefault(a => a.Id == id);
            if (animal != null) {
                _context.Remove(animal);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}
