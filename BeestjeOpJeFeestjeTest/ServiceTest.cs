using BeestjeOpJeFeestjeBusinessLayer;
using BeestjeOpJeFeestjeDb.Models;
using Moq;
using Moq.EntityFrameworkCore;

namespace BeestjeOpJeFeestjeTest {
    [TestClass]
    public class ServiceTest {
        [TestMethod]
        public void GetSetDateSucces() {
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();
            var myContextMock = new Mock<MyContext>();
            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);

            DateOnly datenow = DateOnly.FromDateTime(DateTime.Now);
            bookingService.SetDate(datenow);
            var resultDate = bookingService.GetDate();

            Assert.AreEqual(resultDate, datenow);
        }

        [TestMethod]
        public void AddOrRemoveAnimalFromBookingSucces() {
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();
            var animal1 = new Animal { Type = "Boerderij", Id = 1 };
            var animal2 = new Animal { Name = "Hond", Id = 2 };
            var animal3 = new Animal { Name = "Kip", Id = 3 };
            var animalList = new List<Animal> { animal1, animal2 };

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal1, animal2, animal3 });

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);

            foreach (var animal in animalList) {
                bookingService.AddOrRemoveAnimalFromBooking(animal.Id);
            }
            bookingService.AddOrRemoveAnimalFromBooking(animal3.Id);
            bookingService.AddOrRemoveAnimalFromBooking(animal3.Id);

            var animals = bookingService.GetSelectedAnimals();
            Assert.IsTrue(animalList.ToHashSet().SetEquals(animals));
        }

        [TestMethod]
        public void GetAvailableAnimalsSucces() {
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            Animal animal1 = new Animal { Type = "Boerderij", Id = 1 };
            Animal animal2 = new Animal { Name = "Hond", Id = 2 };
            List<Animal> animalList = new List<Animal> { animal1, animal2 };

            DateOnly date = new DateOnly(2025, 2, 23);
            Booking booking = new Booking { EventDate = date };
            BookingAnimal bookingAnimal = new BookingAnimal { Booking = booking };
            var animal3 = new Animal { BookingAnimals = [bookingAnimal], Name = "Kip", Id = 3 };

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal1, animal2, animal3 });

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);
            bookingService.SetDate(date);

            var availableAnimals = bookingService.GetAvailableAnimals();
            Assert.IsTrue(availableAnimals.ToHashSet().SetEquals(animalList));
        }

        [TestMethod]
        public void GetSelectedAnimlIdsSucces() {
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();
            var animal1 = new Animal { Type = "Boerderij", Id = 1 };
            var animal2 = new Animal { Name = "Hond", Id = 3 };
            var animalIdList = new List<int> { animal1.Id, animal2.Id };

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal1, animal2 });

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);

            foreach (var animalId in animalIdList) {
                bookingService.AddOrRemoveAnimalFromBooking(animalId);
            }

            var animals = bookingService.GetSelectedAnimalIds();
            Assert.IsTrue(animals.ToHashSet().SetEquals(animalIdList));
        }

        [TestMethod]
        public void ValidateBookingStepEqualSucces() {
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();
            var myContextMock = new Mock<MyContext>();
            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);

            bookingService.SetBookingStep(3);
            bool correctBookingStep = bookingService.ValidateBookingStep(3);
            Assert.IsTrue(correctBookingStep);
        }

        [TestMethod]
        public void ValidateBookingStepLowerSucces() {
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();
            var myContextMock = new Mock<MyContext>();
            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);

            bookingService.SetBookingStep(3);
            bool correctBookingStep = bookingService.ValidateBookingStep(2);
            Assert.IsTrue(correctBookingStep);
        }

        [TestMethod]
        public void ValidateBookingStepFail() {
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();
            var myContextMock = new Mock<MyContext>();
            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);

            bookingService.SetBookingStep(3);
            bool correctBookingStep = bookingService.ValidateBookingStep(5);
            Assert.IsFalse(correctBookingStep);
        }

        [TestMethod]
        public async Task ResetBooking() {
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

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal1, animal2 });

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);

            foreach (var animal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(animal.AnimalId);
            }
            bookingService.SetAppUserId(booking.AppUser.Id);
            bookingService.SetBookingStep(1);
            bookingService.CalculateDiscount();

            DateOnly datenow = DateOnly.FromDateTime(DateTime.Now);
            bookingService.SetDate(datenow);

            Booking? before = await bookingService.GetBooking(true);
            bookingService.ResetBooking();
            Booking? after = await bookingService.GetBooking(true);

            Assert.IsTrue(before != after && after == null);
        }
    }
}
