using BeestjeOpJeFeestjeBusinessLayer;
using BeestjeOpJeFeestjeDb.Models;
using Moq;
using Moq.EntityFrameworkCore;

namespace BeestjeOpJeFeestjeTest {

    [TestClass]
    public class ValidationTest {

        [TestMethod]
        public void NoLionOrPolarbearWithFarmAnimalSuccess() {
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

            DateOnly datenow = DateOnly.FromDateTime(DateTime.Now);
            bookingService.SetDate(datenow);

            // Act
            var result = bookingService.ValidateAnimals(out errorList);

            // Assert
            Assert.IsTrue(result, "Expected the validation to pass but it failed.");
        }

        [TestMethod]
        public void NoLionOrPolarbearWithFarmAnimalFail() {
            // Arrange
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingAnimals = new List<BookingAnimal>();
            var booking = new Booking();

            // Create animals
            var animal1 = new Animal { Id = 1, Name = "Hond", Type = "Boerderij" };
            var animal2 = new Animal { Id = 2, Name = "Leeuw", Type = "Jungle" }; // Correct animalId assignment

            // Create BookingAnimal objects
            var bookingAnimal1 = new BookingAnimal { Animal = animal1, AnimalId = animal1.Id, Booking = booking };
            var bookingAnimal2 = new BookingAnimal { Animal = animal2, AnimalId = animal2.Id, Booking = booking };

            // Add to the booking animals list
            bookingAnimals.Add(bookingAnimal1);
            bookingAnimals.Add(bookingAnimal2);

            var user = new AppUser();
            booking.AppUser = user;
            booking.BookingAnimals = bookingAnimals;

            // Set up MyContext mock
            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal1, animal2 });

            // Create the booking service instance
            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);
            var errorList = new List<string>();

            // Act: Add animals to the list and set the date
            foreach (var animal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(animal.AnimalId);
            }

            DateOnly datenow = DateOnly.FromDateTime(DateTime.Now);
            bookingService.SetDate(datenow);

            // Act: Validate the animals (should fail because "Leeuw" is a "Jungle" animal)
            var result = bookingService.ValidateAnimals(out errorList);

            // Assert: The validation should fail
            Assert.IsFalse(result, "Validation should fail for animals with invalid types (like lions and polar bears).");
        }

        [TestMethod]
        public void NoPinguinInWeekendSucces() {
            // Arrange
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();  // Use your method here

            var animal = new Animal { Id = 1, Name = "Pinguïn", Type = "Sneeuw" };
            var user = new AppUser();
            var booking = new Booking {
                AppUser = user,
                BookingAnimals = new List<BookingAnimal>
                {
                    new BookingAnimal { Animal = animal, AnimalId = animal.Id }
                }
            };

            // Set up MyContext mock
            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal });

            // Create the booking service instance
            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);
            var errorList = new List<string>();

            // Act: Add animals to the list and set the date (simulating animal addition)
            foreach (var bookingAnimal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(bookingAnimal.AnimalId);
            }

            // Set date to a weekend day (e.g., 24th January 2025)
            DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 01, 24));
            bookingService.SetDate(datenow);

            // Act: Validate the animals (this test should succeed)
            var result = bookingService.ValidateAnimals(out errorList);

            // Assert: The validation should succeed as Pinguïn is valid for weekends
            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void NoPinguinInWeekendFail() {
            // Arrange
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();  // Use your method here

            var animal = new Animal { Id = 1, Name = "Pinguïn", Type = "Sneeuw" };
            var user = new AppUser();
            var booking = new Booking {
                AppUser = user,
                BookingAnimals = new List<BookingAnimal>
                {
                    new BookingAnimal { Animal = animal, AnimalId = animal.Id }
                }
            };

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal });

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);
            var errorList = new List<string>();

            // Act
            foreach (var bookingAnimal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(bookingAnimal.AnimalId);
            }

            DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 01, 26));
            bookingService.SetDate(datenow);

            var result = bookingService.ValidateAnimals(out errorList);

            // Assert
            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void NoDesertAnimalsInOctoberToFebruariSucces() {
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var animal = new Animal { Id = 1, Name = "Slang", Type = "Woestijn" };
            var user = new AppUser();
            var booking = new Booking {
                AppUser = user,
                BookingAnimals = new List<BookingAnimal> {
            new BookingAnimal { Animal = animal, AnimalId = animal.Id }
        }
            };

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal });

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);
            var errorList = new List<string>();

            foreach (var bookingAnimal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(bookingAnimal.AnimalId);
            }

            DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 06, 26));
            bookingService.SetDate(datenow);

            var result = bookingService.ValidateAnimals(out errorList);

            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void NoDesertAnimalsInOctoberToFebruariFail() {
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var animal = new Animal { Id = 1, Name = "Slang", Type = "Woestijn" };
            var user = new AppUser();
            var booking = new Booking {
                AppUser = user,
                BookingAnimals = new List<BookingAnimal> {
            new BookingAnimal { Animal = animal, AnimalId = animal.Id }
        }
            };

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal });

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);
            var errorList = new List<string>();

            foreach (var bookingAnimal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(bookingAnimal.AnimalId);
            }

            DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 01, 26));
            bookingService.SetDate(datenow);

            var result = bookingService.ValidateAnimals(out errorList);

            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void NoSnowAnimalsInJuneToAugustSucces() {
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var animal = new Animal { Id = 1, Name = "Zeehond", Type = "Sneeuw" };
            var user = new AppUser();
            var booking = new Booking {
                AppUser = user,
                BookingAnimals = new List<BookingAnimal> {
            new BookingAnimal { Animal = animal, AnimalId = animal.Id }
        }
            };

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal });

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);
            var errorList = new List<string>();

            foreach (var bookingAnimal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(bookingAnimal.AnimalId);
            }

            DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 01, 26));
            bookingService.SetDate(datenow);

            var result = bookingService.ValidateAnimals(out errorList);

            Assert.AreEqual(result, true);
        }

        [TestMethod]
        public void NoSnowAnimalsInJuneToAugustFail() {
            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var animal = new Animal { Id = 1, Name = "Zeehond", Type = "Sneeuw" };
            var user = new AppUser();
            var booking = new Booking {
                AppUser = user,
                BookingAnimals = new List<BookingAnimal> {
            new BookingAnimal { Animal = animal, AnimalId = animal.Id }
        }
            };

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal });

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);
            var errorList = new List<string>();

            foreach (var bookingAnimal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(bookingAnimal.AnimalId);
            }

            DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 07, 26));
            bookingService.SetDate(datenow);

            var result = bookingService.ValidateAnimals(out errorList);

            Assert.AreEqual(result, false);
        }

        [TestMethod]
        public void AnimalLimitNoCardFail() {
            // Arrange
            var booking = new Booking();
            var user = new AppUser();
            booking.AppUser = user;

            var animals = new List<Animal>
            {
                new Animal { Id = 1 },
                new Animal { Id = 2 },
                new Animal { Id = 3 },
                new Animal { Id = 4 }
            };
            booking.BookingAnimals = animals.Select(a => new BookingAnimal { Animal = a, AnimalId = a.Id, Booking = booking }).ToList();

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(animals);

            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);
            var errorList = new List<string>();

            foreach (var animal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(animal.AnimalId);
            }
            bookingService.SetDate(DateOnly.FromDateTime(new DateTime(2025, 08, 26)));
            bookingService.SetAppUserId(user.Id);

            // Act
            var result = bookingService.ValidateUserCard(out errorList);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void AnimalLimitSilverCardSucces() {
            // Arrange
            var booking = new Booking();
            var user = new AppUser { Card = "Zilver" };
            booking.AppUser = user;

            var animals = new List<Animal>
            {
                new Animal { Id = 1 } , new Animal { Id = 2 } , new Animal { Id = 3 } , new Animal { Id = 4 }
            };
            booking.BookingAnimals = animals.Select(a => new BookingAnimal { Animal = a, AnimalId = a.Id, Booking = booking }).ToList();

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(animals);

            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);
            var errorList = new List<string>();

            foreach (var animal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(animal.AnimalId);
            }
            bookingService.SetDate(DateOnly.FromDateTime(new DateTime(2025, 08, 26)));
            bookingService.SetAppUserId(user.Id);

            // Act
            var result = bookingService.ValidateUserCard(out errorList);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void AnimalLimitSilverCardFail() {
            // Arrange
            var booking = new Booking();
            var user = new AppUser { Card = "Zilver" };
            booking.AppUser = user;

            var animals = new List<Animal>
            {
                new Animal { Id = 1 },
                new Animal { Id = 2 },
                new Animal { Id = 3 },
                new Animal { Id = 4 },
                new Animal { Id = 5 }
            };
            booking.BookingAnimals = animals.Select(a => new BookingAnimal { Animal = a, AnimalId = a.Id, Booking = booking }).ToList();

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(animals);

            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);
            var errorList = new List<string>();

            foreach (var animal in booking.BookingAnimals) {
                bookingService.AddOrRemoveAnimalFromBooking(animal.AnimalId);
            }
            bookingService.SetDate(DateOnly.FromDateTime(new DateTime(2025, 08, 26)));
            bookingService.SetAppUserId(user.Id);

            // Act
            var result = bookingService.ValidateUserCard(out errorList);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void VipAnimalsSucces() {
            // Arrange
            var booking = new Booking();
            var user = new AppUser { Card = "Platina" };
            booking.AppUser = user;

            var animal = new Animal { Id = 1, Name = "T-Rex", Type = "VIP" };
            booking.BookingAnimals = new List<BookingAnimal> { new BookingAnimal { Animal = animal, AnimalId = animal.Id, Booking = booking } };

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal });

            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);
            var errorList = new List<string>();

            bookingService.AddOrRemoveAnimalFromBooking(animal.Id);
            bookingService.SetDate(DateOnly.FromDateTime(new DateTime(2025, 07, 26)));
            bookingService.SetAppUserId(user.Id);

            // Act
            var result = bookingService.ValidateUserCard(out errorList);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void VipAnimalsFail() {
            // Arrange
            var booking = new Booking();
            var user = new AppUser { Card = "Goud" };
            booking.AppUser = user;

            var animal = new Animal { Id = 1, Name = "T-Rex", Type = "VIP" };
            booking.BookingAnimals = new List<BookingAnimal> { new BookingAnimal { Animal = animal, AnimalId = animal.Id, Booking = booking } };

            var myContextMock = new Mock<MyContext>();
            myContextMock.Setup(c => c.Bookings).ReturnsDbSet(new List<Booking> { booking });
            myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(new List<AppUser> { user });
            myContextMock.Setup(c => c.Animals).ReturnsDbSet(new List<Animal> { animal });

            var httpContextAccessor = HttpContextAccessorFactory.GetHttpContextAccessorWithSession();

            var bookingService = new BookingService(myContextMock.Object, httpContextAccessor);
            var errorList = new List<string>();

            bookingService.AddOrRemoveAnimalFromBooking(animal.Id);
            bookingService.SetDate(DateOnly.FromDateTime(new DateTime(2025, 07, 26)));
            bookingService.SetAppUserId(user.Id);

            // Act
            var result = bookingService.ValidateUserCard(out errorList);

            // Assert
            Assert.AreEqual(false, result);
        }

    }
}
