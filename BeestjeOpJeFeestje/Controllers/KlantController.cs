using BeestjeOpJeFeestje.Models;
using BeestjeOpJeFeestjeBusinessLayer;
using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;

namespace BeestjeOpJeFeestje.Controllers {

    public class KlantController : Controller {

        private readonly IBookingService _bookingService;

        public KlantController(IBookingService bookingService) {
            _bookingService = bookingService;
        }

        // step 1
        public IActionResult Index(DateOnly? date) {
            _bookingService.SetBookingStep(1);

            if (date == null) {
                date = DateOnly.FromDateTime(DateTime.Now);
            }
            return View(date);
        }

        public IActionResult IncreaseDate(DateOnly date) {
            if (!_bookingService.ValidateBookingStep(1)) {
                return NotFound();
            }

            date = date.AddDays(1);
            return RedirectToAction("Index", new { date });
        }
        public IActionResult DecreaseDate(DateOnly date) {
            if (!_bookingService.ValidateBookingStep(1)) {
                return NotFound();
            }

            date = date.AddDays(-1);
            return RedirectToAction("Index", new { date });
        }

        public IActionResult ConfirmDate(DateOnly date) {
            if (!_bookingService.ValidateBookingStep(1)) {
                return NotFound();
            }

            _bookingService.SetDate(date);
            _bookingService.SetBookingStep(2);
            return RedirectToAction("ChooseAnimals");
        }

        // step 2
        public IActionResult ChooseAnimals() {
            if (!_bookingService.ValidateBookingStep(2)) {
                return NotFound();
            }

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
            if (!_bookingService.ValidateBookingStep(2)) {
                return NotFound();
            }

            _bookingService.AddOrRemoveAnimalFromBooking(animalId);
            return RedirectToAction("ChooseAnimals");
        }

        public IActionResult ConfirmSelectedAnimals() {
            if (!_bookingService.ValidateBookingStep(2)) {
                return NotFound();
            }

            if (!_bookingService.ValidateAnimals()) {
                return RedirectToAction("ChooseAnimals"); // TODO: iets met ModelErrors erbij doen
            }

            _bookingService.SetBookingStep(3);
            return RedirectToAction("Authenticate");
        }

        // step 3
        public IActionResult Authenticate() {
            if (!_bookingService.ValidateBookingStep(3)) {
                return NotFound();
            }

            if (!User.Identity.IsAuthenticated) {
                return View();
            }

            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _bookingService.SetAppUserId(userId);

            _bookingService.SetBookingStep(4);
            return RedirectToAction("ConfirmBooking");
        }

        // step 4
        public async Task<IActionResult> ConfirmBooking() {
            if (!_bookingService.ValidateBookingStep(4)) {
                return NotFound();
            }

            decimal discount = _bookingService.CalculateDiscount();
            Booking booking = await _bookingService.GetBooking();
            return View(booking);
        }

        public async Task<IActionResult> AddBooking() {
            if (!_bookingService.ValidateBookingStep(4)) {
                return NotFound();
            }

            await _bookingService.ConfirmBooking();
            return RedirectToAction("Index");
        }
    }
}
