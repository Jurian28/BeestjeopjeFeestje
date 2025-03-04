using BeestjeOpJeFeestjeBusinessLayer;
using BeestjeOpJeFeestjeDb.Models;
using Moq;
using Moq.EntityFrameworkCore;

namespace BeestjeOpJeFeestjeTest {
    [TestClass]
    public class DiscountTest {

        [TestMethod]
        public void DiscountMoreThanOrThreeAnimalsSucces() {
            // Arrange

            var booking = new Booking {
                AppUser = new AppUser(),
                BookingAnimals = new List<BookingAnimal>
                {
                    new() { Animal = new Animal { Id = 1 } },
                    new() { Animal = new Animal { Id = 2 } },
                    new() { Animal = new Animal { Id = 3 } }
                }
            };

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { booking.AppUser });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(booking.BookingAnimals.Select(ba => ba.Animal).ToList());

            var httpContextAccessorMock = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessorMock);
            bookingService.SetAppUserId(booking.AppUser.Id);

            foreach (var bookingAnimal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(bookingAnimal.Animal.Id);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscount();

            // Assert
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) {
                Assert.AreEqual(25, result);
            } else {
                Assert.AreEqual(10, result);
            }
        }

        [TestMethod]
        public void DiscountMoreThanOrThreeAnimalsFail() {
            // Arrange
            var booking = new Booking {
                AppUser = new AppUser(),
                BookingAnimals = new List<BookingAnimal>
                {
                    new() { Animal = new Animal { Id = 1 } },
                    new() { Animal = new Animal { Id = 2 } },
                }
            };

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { booking.AppUser });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(booking.BookingAnimals.Select(ba => ba.Animal).ToList());

            var mockHttpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();
            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor);

            foreach (var bookingAnimal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(bookingAnimal.Animal.Id);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscount();

            // Assert
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) {
                Assert.AreEqual(15, result);
            } else {
                Assert.AreEqual(0, result);
            }
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

            var mockHttpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor);

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscount();

            // Assert
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) {
                Assert.AreEqual(15, result);
            } else {
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

            var mockHttpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor);
            bookingService.SetAppUserId(user.Id);

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscount();

            // Assert
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) {
                Assert.AreEqual(25, result);
            } else {
                Assert.AreEqual(10, result);
            }
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


            var mockHttpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor);
            bookingService.SetAppUserId(user.Id);

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscount();

            // Assert
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) {
                Assert.AreEqual(15, result);
            } else {
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void DiscountDuckSucces() {
            // Arrange
            List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
            Booking booking = new Booking();
            Animal animal1 = new Animal();
            animal1.Name = "Eend";
            animal1.Id = 1;
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

            var mockHttpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor);

            foreach (BookingAnimal animal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(animal.AnimalId);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscount();

            // Assert
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) {
                Assert.IsTrue(result == 15 || result == 65);
            } else {
                Assert.IsTrue(result == 0 || result == 50);
            }
        }

        [TestMethod]
        public void DiscountDuckFail() {
            // Arrange
            List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
            Booking booking = new Booking();
            Animal animal1 = new Animal();
            animal1.Name = "Hond";
            animal1.Id = 1;
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

            var mockHttpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor);

            foreach (BookingAnimal bookingAnimal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(bookingAnimal.Animal.Id);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscount();

            // Assert
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) {
                Assert.AreEqual(15, result);
            } else {
                Assert.AreEqual(0, result);
            }
        }
        [TestMethod]
        public void DiscountAlphabetOneLetterSucces() {
            // Arrange
            List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
            Booking booking = new Booking();
            Animal animal1 = new Animal();
            animal1.Name = "Aap";
            animal1.Id = 1;
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

            var mockHttpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor);

            foreach (BookingAnimal bookingAnimal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(bookingAnimal.Animal.Id);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscount();

            // Assert
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) {
                Assert.AreEqual(17, result);
            } else {
                Assert.AreEqual(2, result);
            }
        }

        [TestMethod]
        public void DiscountAlphabetFourLetterSucces() {
            // Arrange
            List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
            Booking booking = new Booking();
            Animal animal1 = new Animal();
            animal1.Name = "Abcdop";
            animal1.Id = 1;
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

            var mockHttpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor);

            foreach (BookingAnimal bookingAnimal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(bookingAnimal.Animal.Id);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscount();

            // Assert
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) {
                Assert.AreEqual(23, result);
            } else {
                Assert.AreEqual(8, result);
            }
        }
        [TestMethod]
        public void DiscountAlphabetFail() {
            // Arrange
            List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
            Booking booking = new Booking();
            Animal animal1 = new Animal();
            animal1.Name = "Bcde";
            animal1.Id = 1;
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

            var mockHttpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor);

            foreach (BookingAnimal bookingAnimal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(bookingAnimal.Animal.Id);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscount();

            // Assert
            if (DateTime.Now.DayOfWeek == DayOfWeek.Monday || DateTime.Now.DayOfWeek == DayOfWeek.Tuesday) {
                Assert.AreEqual(15, result);
            } else {
                Assert.AreEqual(0, result);
            }
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
            animal1.Id = 1;
            animal2.Id = 2;
            animal3.Id = 3;
            BookingAnimal bookingAnimal1 = new BookingAnimal();
            bookingAnimal1.Animal = animal1;
            bookingAnimal1.Booking = booking;
            bookingAnimals.Add(bookingAnimal1);
            BookingAnimal bookingAnimal2 = new BookingAnimal();
            bookingAnimal2.Animal = animal2;
            bookingAnimal2.Booking = booking;
            bookingAnimals.Add(bookingAnimal2);
            BookingAnimal bookingAnimal3 = new BookingAnimal();
            bookingAnimal3.Animal = animal3;
            bookingAnimal3.Booking = booking;
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

            var mockHttpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor);

            foreach (BookingAnimal bookingAnimal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(bookingAnimal.Animal.Id);
            }

            // Act
            bookingService.CalculateDiscount();
            var result = bookingService.GetDiscount();

            // Assert
            Assert.AreEqual(60, result);
        }
    }
}
