using BeestjeOpJeFeestje.Models;
using BeestjeOpJeFeestjeBusinessLayer;
using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.EntityFrameworkCore;

namespace BeestjeOpJeFeestjeTest {

    [TestClass]
    public class ServiceTest {

        //[TestMethod]
        //public void GetSetHttpContextString() {
        //    // Arrange
        //    // Act
        //    // Assert
        //    Assert.IsTrue();
        //}
        [TestMethod]
        public void GetSetDateSucces() {
            // Arrange

            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();
            //var bookingAnimals = new List<BookingAnimal>();
            //var booking = new Booking();
            //var animal1 = new Animal { Type = "Boerderij", Id = 1 };
            //var animal2 = new Animal { Name = "Hond", Id = 2 };

            //var bookingAnimal1 = new BookingAnimal { Animal = animal1, AnimalId = animal1.Id, Booking = booking };
            //var bookingAnimal2 = new BookingAnimal { Animal = animal2, AnimalId = animal2.Id, Booking = booking };

            //bookingAnimals.Add(bookingAnimal1);
            //bookingAnimals.Add(bookingAnimal2);

            //var user = new AppUser();
            //booking.AppUser = user;
            //booking.BookingAnimals = bookingAnimals;

            // Use ONE mock context and set up all DbSets
            var myContextMock = new Mock<MyContext>();
            //myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            //myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            //myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal1, animal2 });

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);

            var errorList = new List<string>();

            //foreach (var animal in booking.BookingAnimals) {
            //    bookingService.AddOrRemoveAnimalFromBooking(animal.AnimalId);
            //}

            DateOnly datenow = DateOnly.FromDateTime(DateTime.Now);
            // Act
            bookingService.SetDate(datenow);
            var resultDate = bookingService.GetDate();
            // Assert
            Assert.AreEqual(resultDate, datenow);
        }
        [TestMethod]
        public void AddOrRemoveAnimalFromBookingSucces() {
            // Arrange

            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();
            var animal1 = new Animal { Type = "Boerderij", Id = 1 };
            var animal2 = new Animal { Name = "Hond", Id = 2 };
            var animal3 = new Animal { Name = "Kip", Id = 3 };

            var animalList = new List<Animal> { animal1, animal2 };

            //var user = new AppUser();
            //booking.AppUser = user;
            //booking.BookingAnimals = bookingAnimals;

            // Use ONE mock context and set up all DbSets
            var myContextMock = new Mock<MyContext>();
            //myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            //myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal1, animal2, animal3 });

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);

            //var errorList = new List<string>();

            // Act
            foreach (var animal in animalList) {
                bookingService.AddOrRemoveAnimalFromBooking(animal.Id);
            }
            bookingService.AddOrRemoveAnimalFromBooking(animal3.Id);
            bookingService.AddOrRemoveAnimalFromBooking(animal3.Id); // add and remove

            var animals = bookingService.GetSelectedAnimals();
            // Assert
            Assert.IsTrue(animalList.ToHashSet().SetEquals(animals));
        }
        [TestMethod]
        public void GetAvailableAnimalsSucces() {
            // Arrange
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            Animal animal1 = new Animal { Type = "Boerderij", Id = 1 };
            Animal animal2 = new Animal { Name = "Hond", Id = 2 };
            List<Animal> animalList = new List<Animal>(){ animal1, animal2 };

            DateOnly date = new DateOnly(2025, 2, 23);

            Booking booking = new Booking() { EventDate = date };
            BookingAnimal bookingAnimal = new BookingAnimal() { Booking = booking };
            var animal3 = new Animal { BookingAnimals = [bookingAnimal], Name = "Kip", Id = 3 };

            //var user = new AppUser();
            //booking.AppUser = user;
            //booking.BookingAnimals = bookingAnimals;

            // Use ONE mock context and set up all DbSets
            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal1, animal2, animal3 });

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);
            bookingService.SetDate(date);

            // add animals to context i think
            // Act
            var availableAnimals = bookingService.GetAvailableAnimals();

            // Assert
            Assert.IsTrue(availableAnimals.ToHashSet().SetEquals(animalList)); 
        }


        [TestMethod]
        public void GetSelectedAnimlIdsSucces() {
            // Arrange
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();
            var animal1 = new Animal { Type = "Boerderij", Id = 1 };
            var animal2 = new Animal { Name = "Hond", Id = 3 };

            var animalIdList = new List<int> { animal1.Id, animal2.Id };

            //var user = new AppUser();
            //booking.AppUser = user;
            //booking.BookingAnimals = bookingAnimals;

            // Use ONE mock context and set up all DbSets
            var myContextMock = new Mock<MyContext>();
            //myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            //myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal1, animal2 });

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);

            foreach (var animalId in animalIdList) {
                bookingService.AddOrRemoveAnimalFromBooking(animalId);
            }

            // Act

            var animals = bookingService.GetSelectedAnimalIds();
            // Assert
            Assert.IsTrue(animals.ToHashSet().SetEquals(animalIdList));
        }
        [TestMethod]
        public void SetAppUserIdSucces() {
            // Arrange
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var user = new AppUser();
            string userId = user.Id;

            // Use ONE mock context and set up all DbSets
            var myContextMock = new Mock<MyContext>();
            //myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            //myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal1, animal2 });

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);

            // Act
            bookingService.SetAppUserId(userId);
            // TODO afmaken. er is geen GetAppUserId() dus dan moet je iets anders verzinnen

            // Assert
            Assert.IsTrue(false);
        }
        //[TestMethod]
        //public void ConfirmBooking() {
        //    // Arrange
        //    // Act
        //    // Assert
        //}
        [TestMethod]
        public void ValidateBookingStepEqualSucces() {
            // Arrange
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            // Use ONE mock context and set up all DbSets
            var myContextMock = new Mock<MyContext>();
            //myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            //myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            //myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal2, animal2 });
            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);

            // Act
            bookingService.SetBookingStep(3);
            bool correctBookingStep = bookingService.ValidateBookingStep(3);
            // Assert
            Assert.IsTrue(correctBookingStep);
        }
        [TestMethod]
        public void ValidateBookingStepLowerSucces() {
            // Arrange
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            // Use ONE mock context and set up all DbSets
            var myContextMock = new Mock<MyContext>();
            //myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            //myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            //myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal2, animal2 });
            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);

            // Act
            bookingService.SetBookingStep(3);
            bool correctBookingStep = bookingService.ValidateBookingStep(2);
            // Assert
            Assert.IsTrue(correctBookingStep);
        }
        [TestMethod]
        public void ValidateBookingStepFail() {
            // Arrange
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            // Use ONE mock context and set up all DbSets
            var myContextMock = new Mock<MyContext>();
            //myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            //myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            //myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal2, animal2 });
            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);

            // Act
            bookingService.SetBookingStep(3);
            bool correctBookingStep = bookingService.ValidateBookingStep(5);
            // Assert
            Assert.IsFalse(correctBookingStep);
        }
        [TestMethod]
        public async Task ResetBooking() {
            // Arrange
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();
            var bookingAnimals = new List<BookingAnimal>();
            var booking = new Booking();
            var animal1 = new Animal { Type = "Boerderij", Id = 1 };
            var animal2 = new Animal { Name = "Hond", Id = 2 };

            var bookingAnimal1 = new BookingAnimal { Animal = animal1, AnimalId = animal1.Id, Booking = booking };
            var bookingAnimal2 = new BookingAnimal { Animal = animal2, AnimalId = animal2.Id, Booking = booking };

            bookingAnimals.Add(bookingAnimal1);
            bookingAnimals.Add(bookingAnimal2);

            var user = new AppUser();
            booking.AppUser = user;
            booking.BookingAnimals = bookingAnimals;

            // Use ONE mock context and set up all DbSets
            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal1, animal2 });

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);

            var errorList = new List<string>();

            foreach (var animal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(animal.AnimalId);
            }
            bookingService.SetAppUserId(booking.AppUser.Id);
            bookingService.SetBookingStep(1);
            bookingService.CalculateDiscount();

            DateOnly datenow = DateOnly.FromDateTime(DateTime.Now);
            bookingService.SetDate(datenow);

            // Act
            Booking before = await bookingService.GetBooking(true);
            bookingService.ResetBooking();
            Booking after = await bookingService.GetBooking(true);
            // Assert
            Assert.IsTrue(false);
        }
    }
}
