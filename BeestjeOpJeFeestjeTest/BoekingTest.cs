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
    public class BoekingTest {
        private AppUser user;
        private Booking booking;

        [TestInitialize]
        public void Init() {
            user = new AppUser();
            booking = new Booking();
        }

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

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var defaultContext = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscount();

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
   
            booking.BookingAnimals = bookingAnimals;

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
            var result = bookingService.GetDiscount();

            // Assert
            Assert.AreEqual(0, result);
        }

        [TestMethod]
        public void DiscountMondayOrTuesday() {
            var myContextMock = new Mock<MyContext>();
            var entities = new List<Booking>() { booking };
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

            var myContextMockUser = new Mock<MyContext>();
            var entitiesUser = new List<AppUser>() { user };
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var defaultContext = new DefaultHttpContext();
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

            var bookingService = new BookingService(myContextMock.Object , mockHttpContextAccessor.Object);

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscount();

            // Assert
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) {
                Assert.AreEqual(15, result);
            }
            else {
                Assert.AreEqual(0, result);
            }
        }
    }
}
