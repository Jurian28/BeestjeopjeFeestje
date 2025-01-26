using BeestjeOpJeFeestje.Models;
using BeestjeOpJeFeestjeDb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using BeestjeOpJeFeestjeBusinessLayer;
using Microsoft.AspNetCore.Identity;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.SignalR;
using BeestjeOpJeFeestjeDb;
using Moq.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace BeestjeOpJeFeestjeTest {
    [TestClass]
    public class DiscountTest {

        [TestMethod]
        public void DiscountMoreThanOrThreeAnimalsSucces() {
            // Arrange
            List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
            Booking booking = new Booking();
            Animal animal1 = new Animal();
            Animal animal2 = new Animal();
            Animal animal3 = new Animal();
            BookingAnimal bookingAnimal1 = new BookingAnimal();
            bookingAnimal1.Animal = animal1;
            bookingAnimal1.Booking = booking;
            bookingAnimals.Add(bookingAnimal1);
            BookingAnimal bookingAnimal2 = new BookingAnimal();
            bookingAnimal1.Animal = animal2;
            bookingAnimal1.Booking = booking;
            bookingAnimals.Add(bookingAnimal2);
            BookingAnimal bookingAnimal3 = new BookingAnimal();
            bookingAnimal1.Animal = animal3;
            bookingAnimal1.Booking = booking;
            bookingAnimals.Add(bookingAnimal3);

            AppUser user = new AppUser();
            booking.AppUser = user;

            booking.BookingAnimals = bookingAnimals;

            var myContextMock = new Mock<MyContext>();
            var entities = new List<Booking>() { booking };
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

            var myContextMockUser = new Mock<MyContext>();
            var entitiesUser = new List<AppUser>() { user };
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

            var myContextMockAnimals = new Mock<MyContext>();
            var entitiesAnimals = new List<Animal>() { animal1, animal2, animal3 };
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var defaultContext = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);
            bookingService.SetAppUserId(user.Id);

            foreach (BookingAnimal animal in booking.BookingAnimals) {
                bookingService.AddToAnimalList(animal.AnimalId);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscountForTest();

            // Assert
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void DiscountMoreThanOrThreeAnimalsFail() {
            // Arrange
            List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
            Booking booking = new Booking();
            Animal animal1 = new Animal();
            Animal animal2 = new Animal();
            BookingAnimal bookingAnimal1 = new BookingAnimal();
            bookingAnimal1.Animal = animal1;
            bookingAnimal1.Booking = booking;
            bookingAnimals.Add(bookingAnimal1);
            BookingAnimal bookingAnimal2 = new BookingAnimal();
            bookingAnimal1.Animal = animal2;
            bookingAnimal1.Booking = booking;
            bookingAnimals.Add(bookingAnimal2);

            AppUser user = new AppUser();
            booking.AppUser = user;

            booking.BookingAnimals = bookingAnimals;

            var myContextMock = new Mock<MyContext>();
            var entities = new List<Booking>() { booking };
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

            var myContextMockUser = new Mock<MyContext>();
            var entitiesUser = new List<AppUser>() { user };
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

            var myContextMockAnimals = new Mock<MyContext>();
            var entitiesAnimals = new List<Animal>() { animal1, animal2 };
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var defaultContext = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);

            foreach (BookingAnimal animal in booking.BookingAnimals) {
                bookingService.AddToAnimalList(animal.AnimalId);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscountForTest();

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void DiscountMondayOrTuesday() {
            // Arrange
            Booking booking = new Booking();

            AppUser user = new AppUser();
            booking.AppUser = user;

            var myContextMock = new Mock<MyContext>();
            var entities = new List<Booking>() { booking };
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

            var myContextMockUser = new Mock<MyContext>();
            var entitiesUser = new List<AppUser>() { user };
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var defaultContext = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscountForTest();

            // Assert
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) {
                Assert.AreEqual(15, result);
            }
            else {
                Assert.AreEqual(0, result);
            }
        }

        [TestMethod]
        public void DiscountHasCardSucces() {
            // Arrange
            Booking booking = new Booking();

            AppUser user = new AppUser();
            user.Card = "Silver";
            booking.AppUser = user;

            var myContextMock = new Mock<MyContext>();
            var entities = new List<Booking>() { booking };
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

            var myContextMockUser = new Mock<MyContext>();
            var entitiesUser = new List<AppUser>() { user };
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var defaultContext = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);
            bookingService.SetUser(user.Id);

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscountForTest();

            // Assert
            Assert.AreEqual(10, result);
        }

        [TestMethod]
        public void DiscountHasCardFail() {
            // Arrange
            Booking booking = new Booking();

            AppUser user = new AppUser();
            user.Card = "Geen";
            booking.AppUser = user;

            var myContextMock = new Mock<MyContext>();
            var entities = new List<Booking>() { booking };
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

            var myContextMockUser = new Mock<MyContext>();
            var entitiesUser = new List<AppUser>() { user };
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var defaultContext = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);
            bookingService.SetUser(user.Id);

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscountForTest();

            // Assert
            Assert.AreEqual(0, result);
        }
        [TestMethod]
        public void DiscountDuckSucces() {
            // Arrange
            List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
            Booking booking = new Booking();
            Animal animal1 = new Animal();
            animal1.Name = "Eend";
            BookingAnimal bookingAnimal1 = new BookingAnimal();
            bookingAnimal1.Animal = animal1;
            bookingAnimal1.Booking = booking;
            bookingAnimals.Add(bookingAnimal1);


            AppUser user = new AppUser();
            booking.AppUser = user;
            booking.BookingAnimals = bookingAnimals;

            var myContextMock = new Mock<MyContext>();
            var entities = new List<Booking>() { booking };
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

            var myContextMockUser = new Mock<MyContext>();
            var entitiesUser = new List<AppUser>() { user };
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

            var myContextMockAnimals = new Mock<MyContext>();
            var entitiesAnimals = new List<Animal>() { animal1 };
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var defaultContext = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);

            foreach (BookingAnimal animal in booking.BookingAnimals) {
                bookingService.AddToAnimalList(animal.AnimalId);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscountForTest();

            // Assert
            Assert.IsTrue(result == 0 || result == 50);
        }

        [TestMethod]
        public void DiscountDuckFail() {
            // Arrange
            List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
            Booking booking = new Booking();
            Animal animal1 = new Animal();
            animal1.Name = "Hond";
            BookingAnimal bookingAnimal1 = new BookingAnimal();
            bookingAnimal1.Animal = animal1;
            bookingAnimal1.Booking = booking;
            bookingAnimals.Add(bookingAnimal1);


            AppUser user = new AppUser();
            booking.AppUser = user;
            booking.BookingAnimals = bookingAnimals;

            var myContextMock = new Mock<MyContext>();
            var entities = new List<Booking>() { booking };
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

            var myContextMockUser = new Mock<MyContext>();
            var entitiesUser = new List<AppUser>() { user };
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

            var myContextMockAnimals = new Mock<MyContext>();
            var entitiesAnimals = new List<Animal>() { animal1 };
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var defaultContext = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);

            foreach (BookingAnimal animal in booking.BookingAnimals) {
                bookingService.AddToAnimalList(animal.AnimalId);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscountForTest();

            // Assert
            Assert.AreEqual(0, result);
        }
        [TestMethod]
        public void DiscountAlphabetOneLetterSucces() {
            // Arrange
            List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
            Booking booking = new Booking();
            Animal animal1 = new Animal();
            animal1.Name = "Aap";
            BookingAnimal bookingAnimal1 = new BookingAnimal();
            bookingAnimal1.Animal = animal1;
            bookingAnimal1.Booking = booking;
            bookingAnimals.Add(bookingAnimal1);

            AppUser user = new AppUser();
            booking.AppUser = user;
            booking.BookingAnimals = bookingAnimals;

            var myContextMock = new Mock<MyContext>();
            var entities = new List<Booking>() { booking };
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

            var myContextMockUser = new Mock<MyContext>();
            var entitiesUser = new List<AppUser>() { user };
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

            var myContextMockAnimals = new Mock<MyContext>();
            var entitiesAnimals = new List<Animal>() { animal1 };
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var defaultContext = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);

            foreach (BookingAnimal animal in booking.BookingAnimals) {
                bookingService.AddToAnimalList(animal.AnimalId);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscountForTest();

            // Assert
            Assert.AreEqual(2, result);
        }

        [TestMethod]
        public void DiscountAlphabetFourLetterSucces() {
            // Arrange
            List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
            Booking booking = new Booking();
            Animal animal1 = new Animal();
            animal1.Name = "Abcdop";
            BookingAnimal bookingAnimal1 = new BookingAnimal();
            bookingAnimal1.Animal = animal1;
            bookingAnimal1.Booking = booking;
            bookingAnimals.Add(bookingAnimal1);

            AppUser user = new AppUser();
            booking.AppUser = user;
            booking.BookingAnimals = bookingAnimals;

            var myContextMock = new Mock<MyContext>();
            var entities = new List<Booking>() { booking };
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

            var myContextMockUser = new Mock<MyContext>();
            var entitiesUser = new List<AppUser>() { user };
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

            var myContextMockAnimals = new Mock<MyContext>();
            var entitiesAnimals = new List<Animal>() { animal1 };
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var defaultContext = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);

            foreach (BookingAnimal animal in booking.BookingAnimals) {
                bookingService.AddToAnimalList(animal.AnimalId);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscountForTest();

            // Assert
            Assert.AreEqual(8, result);
        }
        [TestMethod]
        public void DiscountAlphabetFail() {
            // Arrange
            List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
            Booking booking = new Booking();
            Animal animal1 = new Animal();
            animal1.Name = "Bcde";
            BookingAnimal bookingAnimal1 = new BookingAnimal();
            bookingAnimal1.Animal = animal1;
            bookingAnimal1.Booking = booking;
            bookingAnimals.Add(bookingAnimal1);

            AppUser user = new AppUser();
            booking.AppUser = user;
            booking.BookingAnimals = bookingAnimals;

            var myContextMock = new Mock<MyContext>();
            var entities = new List<Booking>() { booking };
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

            var myContextMockUser = new Mock<MyContext>();
            var entitiesUser = new List<AppUser>() { user };
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

            var myContextMockAnimals = new Mock<MyContext>();
            var entitiesAnimals = new List<Animal>() { animal1 };
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var defaultContext = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);

            foreach (BookingAnimal animal in booking.BookingAnimals) {
                bookingService.AddToAnimalList(animal.AnimalId);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscountForTest();

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void DiscountHigherThanSixty() {
            // Arrange
            List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
            Booking booking = new Booking();
            Animal animal1 = new Animal();
            animal1.Name = "Abcdefghijklmnopqrstuvwxy";
            Animal animal2 = new Animal();
            animal1.Name = "Abcdefghijklmnopqrstuvwxy";
            Animal animal3 = new Animal();
            animal1.Name = "Abcdefghijklmnopqrstuvwxy";
            BookingAnimal bookingAnimal1 = new BookingAnimal();
            bookingAnimal1.Animal = animal1;
            bookingAnimal1.Booking = booking;
            bookingAnimals.Add(bookingAnimal1);
            BookingAnimal bookingAnimal2 = new BookingAnimal();
            bookingAnimal1.Animal = animal2;
            bookingAnimal1.Booking = booking;
            bookingAnimals.Add(bookingAnimal2);
            BookingAnimal bookingAnimal3 = new BookingAnimal();
            bookingAnimal1.Animal = animal3;
            bookingAnimal1.Booking = booking;
            bookingAnimals.Add(bookingAnimal3);

            AppUser user = new AppUser();
            booking.AppUser = user;
            booking.BookingAnimals = bookingAnimals;

            var myContextMock = new Mock<MyContext>();
            var entities = new List<Booking>() { booking };
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

            var myContextMockUser = new Mock<MyContext>();
            var entitiesUser = new List<AppUser>() { user };
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

            var myContextMockAnimals = new Mock<MyContext>();
            var entitiesAnimals = new List<Animal>() { animal1, animal2, animal3 };
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var defaultContext = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);

            foreach (BookingAnimal animal in booking.BookingAnimals) {
                bookingService.AddToAnimalList(animal.AnimalId);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscountForTest();

            // Assert
            Assert.AreEqual(60, result);
        }
    }
}
