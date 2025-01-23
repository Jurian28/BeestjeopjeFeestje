using BeestjeOpJeFeestje.Models;
using BeestjeOpJeFeestjeBusinessLayer;
using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BeestjeOpJeFeestje.Controllers {

    public class KlantController : Controller {

        private readonly IBookingService _bookingService;

        public KlantController(IBookingService bookingService) {
            _bookingService = bookingService;
        }

        public IActionResult Index(DateOnly? date) {
            if (date == null) {
                date = DateOnly.FromDateTime(DateTime.Now);
            }
            return View(date);
        }

        public IActionResult IncreaseDate(DateOnly date) {
            date = date.AddDays(1);
            return RedirectToAction("Index", new { date });
        }
        public IActionResult DecreaseDate(DateOnly date) {
            date = date.AddDays(-1);
            return RedirectToAction("Index", new { date });
        }

        public IActionResult ConfirmDate(DateOnly date) {

            _bookingService.SetDate(date);
            return RedirectToAction("ChooseAnimals");
        }

        // step 2
        public IActionResult ChooseAnimals() {
            DateOnly? date = _bookingService.GetDate();
            if (date == null) 
                return NotFound();

            List<Animal> availableAnimals = _bookingService.GetAvailableAnimals();
            List<int> selectedAnimals = _bookingService.GetSelectedAnimalIds();

            ChooseAnimalsViewModel viewModel = new() {
                Date = (DateOnly)date,
                AvailableAnimals = availableAnimals,
                SelectedAnimals = selectedAnimals

            };
            return View(viewModel);
        }

        public IActionResult SelectAnimal(int animalId) {
            _bookingService.AddOrRemoveAnimalFromBooking(animalId);
            return RedirectToAction("ChooseAnimals");
        }

        // step 3
        public async Task<IActionResult> Authenticate() {
            //AppUser? user = await _userManager.GetUserAsync(User);
            //if (user == null) {
            //    return View();
            //}
            return RedirectToAction("ConfirmBooking");
        }


        // step 3
        public IActionResult ConfirmBooking() {
            Booking booking = new Booking() {

            };
            return View();
        }
    }
}
