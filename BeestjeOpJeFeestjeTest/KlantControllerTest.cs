using BeestjeOpJeFeestje.Controllers;
using BeestjeOpJeFeestje.Models;
using BeestjeOpJeFeestjeBusinessLayer;
using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace BeestjeOpJeFeestjeTest {
    [TestClass]
    public class KlantControllerTest {
        private Mock<IBookingService> _bookingServiceMock;
        private KlantController _controller;

        [TestInitialize]
        public void Setup() {
            _bookingServiceMock = new Mock<IBookingService>();
            _controller = new KlantController(_bookingServiceMock.Object);
        }

        [TestMethod]
        public void Index_SetsStepAndReturnsViewWithDate() {
            // Arrange
            DateOnly today = DateOnly.FromDateTime(DateTime.Now);

            // Act
            var result = _controller.Index(null) as ViewResult;

            // Assert
            _bookingServiceMock.Verify(b => b.SetBookingStep(1), Times.Once);
            Assert.IsNotNull(result);
            Assert.AreEqual(today, result.Model);
        }

        [TestMethod]
        public void IncreaseDate_ValidStep_RedirectsWithIncreasedDate() {
            var date = DateOnly.FromDateTime(DateTime.Today);
            _bookingServiceMock.Setup(b => b.ValidateBookingStep(1)).Returns(true);

            var result = _controller.IncreaseDate(date) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual(date.AddDays(1), result.RouteValues["date"]);
        }

        [TestMethod]
        public void DecreaseDate_InvalidStep_ReturnsNotFound() {
            _bookingServiceMock.Setup(b => b.ValidateBookingStep(1)).Returns(false);

            var result = _controller.DecreaseDate(DateOnly.FromDateTime(DateTime.Now));

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void ConfirmDate_ValidStep_SetsDateAndRedirects() {
            var date = DateOnly.FromDateTime(DateTime.Today);
            _bookingServiceMock.Setup(b => b.ValidateBookingStep(1)).Returns(true);

            var result = _controller.ConfirmDate(date) as RedirectToActionResult;

            _bookingServiceMock.Verify(b => b.SetDate(date), Times.Once);
            _bookingServiceMock.Verify(b => b.SetBookingStep(2), Times.Once);
            Assert.AreEqual("ChooseAnimals", result.ActionName);
        }

        [TestMethod]
        public void ChooseAnimals_Valid_ReturnsViewModel() {
            _bookingServiceMock.Setup(b => b.ValidateBookingStep(2)).Returns(true);
            _bookingServiceMock.Setup(b => b.GetDate()).Returns(DateOnly.FromDateTime(DateTime.Today));
            _bookingServiceMock.Setup(b => b.GetAvailableAnimals()).Returns(new List<Animal>());
            _bookingServiceMock.Setup(b => b.GetSelectedAnimalIds()).Returns(new List<int>());

            var result = _controller.ChooseAnimals() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(ChooseAnimalsViewModel));
        }

        [TestMethod]
        public void SelectAnimal_ValidStep_AddsAnimalAndRedirects() {
            _bookingServiceMock.Setup(b => b.ValidateBookingStep(2)).Returns(true);

            var result = _controller.SelectAnimal(1) as RedirectToActionResult;

            _bookingServiceMock.Verify(b => b.AddOrRemoveAnimalFromBooking(1), Times.Once);
            Assert.AreEqual("ChooseAnimals", result.ActionName);
        }

        [TestMethod]
        public void ConfirmSelectedAnimals_InvalidAnimalValidation_RedirectsToChooseAnimals() {
            _bookingServiceMock.Setup(b => b.ValidateBookingStep(2)).Returns(true);
            _bookingServiceMock.Setup(b => b.ValidateAnimals(out It.Ref<List<string>>.IsAny))
                .Returns(false)
                .Callback(new ValidateAnimalsCallback((out List<string> errors) => errors = new List<string> { "error" }));

            var result = _controller.ConfirmSelectedAnimals() as RedirectToActionResult;

            Assert.AreEqual("ChooseAnimals", result.ActionName);
            Assert.IsTrue(((List<string>)result.RouteValues["validationErrors"]).Contains("error"));
        }

        delegate void ValidateAnimalsCallback(out List<string> errors);

        [TestMethod]
        public void Authenticate_UnauthenticatedUser_ReturnsView() {
            _bookingServiceMock.Setup(b => b.ValidateBookingStep(3)).Returns(true);
            _bookingServiceMock.Setup(b => b.GetDate()).Returns(DateOnly.FromDateTime(DateTime.Today));
            _bookingServiceMock.Setup(b => b.GetSelectedAnimals()).Returns(new List<Animal>());

            var user = new ClaimsPrincipal(new ClaimsIdentity());
            _controller.ControllerContext = new ControllerContext() {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            var result = _controller.Authenticate() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(KlantAuthenticateViewModel));
        }

        [TestMethod]
        public async Task ConfirmBooking_Valid_ReturnsViewWithBooking() {
            _bookingServiceMock.Setup(b => b.ValidateBookingStep(4)).Returns(true);
            _bookingServiceMock.Setup(b => b.CalculateDiscount());
            _bookingServiceMock.Setup(b => b.GetBooking(false)).ReturnsAsync(new Booking());

            var result = await _controller.ConfirmBooking() as ViewResult;

            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result.Model, typeof(Booking));
        }

        [TestMethod]
        public async Task AddBooking_Valid_CallsConfirmBookingAndRedirects() {
            _bookingServiceMock.Setup(b => b.ValidateBookingStep(4)).Returns(true);

            var result = await _controller.AddBooking() as RedirectToActionResult;

            _bookingServiceMock.Verify(b => b.ConfirmBooking(), Times.Once);
            _bookingServiceMock.Verify(b => b.ResetBooking(), Times.Once);
            Assert.AreEqual("Index", result.ActionName);
        }
    }
}
