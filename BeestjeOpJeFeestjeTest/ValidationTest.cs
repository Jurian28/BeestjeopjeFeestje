using BeestjeOpJeFeestje.Models;
using BeestjeOpJeFeestjeBusinessLayer;
using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using Moq.EntityFrameworkCore;

namespace BeestjeOpJeFeestjeTest {

    [TestClass]
    public class ValidationTest {

        [TestMethod]
        public void NoLionOrPolarbearWithFarmAnimalSuccess() {
            // Arrange
            var httpContextAccessor = GetHttpContextAccessorWithSession(); 
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
            var httpContextAccessor = GetHttpContextAccessorWithSession(); 

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

        //    [TestMethod]
        //    public void NoPinguinInWeekendSucces() {
        //        // Arrange
        //        List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
        //        Booking booking = new Booking();
        //        Animal animal1 = new Animal();
        //        animal1.Id = 1;
        //        animal1.Name = "Pinguïn";
        //        animal1.Type = "Sneeuw";
        //        BookingAnimal bookingAnimal1 = new BookingAnimal();
        //        bookingAnimal1.Animal = animal1;
        //        bookingAnimal1.AnimalId = animal1.Id;
        //        bookingAnimal1.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal1);

        //        AppUser user = new AppUser();
        //        booking.AppUser = user;
        //        booking.BookingAnimals = bookingAnimals;

        //        var myContextMock = new Mock<MyContext>();
        //        var entities = new List<Booking>() { booking };
        //        myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

        //        var myContextMockUser = new Mock<MyContext>();
        //        var entitiesUser = new List<AppUser>() { user };
        //        myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

        //        var myContextMockAnimals = new Mock<MyContext>();
        //        var entitiesAnimals = new List<Animal>() { animal1 };
        //        myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

        //        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        //        var defaultContext = new DefaultHttpContext();
        //        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

        //        var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);
        //        var errorList = new List<string>();

        //        foreach (BookingAnimal animal in booking.BookingAnimals) {
        //            bookingService.AddToAnimalList(animal.AnimalId);
        //        }

        //        DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025,01,24));
        //        bookingService.SetDateString(datenow.ToString());

        //        // Act
        //        var result = bookingService.ValidateAnimals(out errorList);

        //        // Assert
        //        Assert.AreEqual(result, true);
        //    }

        //    [TestMethod]
        //    public void NoPinguinInWeekendFail() {
        //        // Arrange
        //        List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
        //        Booking booking = new Booking();
        //        Animal animal1 = new Animal();
        //        animal1.Id = 1;
        //        animal1.Name = "Pinguïn";
        //        animal1.Type = "Sneeuw";
        //        BookingAnimal bookingAnimal1 = new BookingAnimal();
        //        bookingAnimal1.Animal = animal1;
        //        bookingAnimal1.AnimalId = animal1.Id;
        //        bookingAnimal1.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal1);

        //        AppUser user = new AppUser();
        //        booking.AppUser = user;
        //        booking.BookingAnimals = bookingAnimals;

        //        var myContextMock = new Mock<MyContext>();
        //        var entities = new List<Booking>() { booking };
        //        myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

        //        var myContextMockUser = new Mock<MyContext>();
        //        var entitiesUser = new List<AppUser>() { user };
        //        myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

        //        var myContextMockAnimals = new Mock<MyContext>();
        //        var entitiesAnimals = new List<Animal>() { animal1 };
        //        myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

        //        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        //        var defaultContext = new DefaultHttpContext();
        //        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

        //        var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);
        //        var errorList = new List<string>();

        //        foreach (BookingAnimal animal in booking.BookingAnimals) {
        //            bookingService.AddToAnimalList(animal.AnimalId);
        //        }

        //        DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 01, 26));
        //        bookingService.SetDateString(datenow.ToString());

        //        // Act
        //        var result = bookingService.ValidateAnimals(out errorList);

        //        // Assert
        //        Assert.AreEqual(result, false);
        //    }

        //    [TestMethod]
        //    public void NoDesertAnimalsInOctoberToFebruariSucces() {
        //        // Arrange
        //        List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
        //        Booking booking = new Booking();
        //        Animal animal1 = new Animal();
        //        animal1.Id = 1;
        //        animal1.Name = "Slang";
        //        animal1.Type = "Woestijn";
        //        BookingAnimal bookingAnimal1 = new BookingAnimal();
        //        bookingAnimal1.Animal = animal1;
        //        bookingAnimal1.AnimalId = animal1.Id;
        //        bookingAnimal1.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal1);

        //        AppUser user = new AppUser();
        //        booking.AppUser = user;
        //        booking.BookingAnimals = bookingAnimals;

        //        var myContextMock = new Mock<MyContext>();
        //        var entities = new List<Booking>() { booking };
        //        myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

        //        var myContextMockUser = new Mock<MyContext>();
        //        var entitiesUser = new List<AppUser>() { user };
        //        myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

        //        var myContextMockAnimals = new Mock<MyContext>();
        //        var entitiesAnimals = new List<Animal>() { animal1 };
        //        myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

        //        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        //        var defaultContext = new DefaultHttpContext();
        //        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

        //        var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);
        //        var errorList = new List<string>();

        //        foreach (BookingAnimal animal in booking.BookingAnimals) {
        //            bookingService.AddToAnimalList(animal.AnimalId);
        //        }

        //        DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 06, 26));
        //        bookingService.SetDateString(datenow.ToString());

        //        // Act
        //        var result = bookingService.ValidateAnimals(out errorList);

        //        // Assert
        //        Assert.AreEqual(result, true);
        //    }

        //    [TestMethod]
        //    public void NoDesertAnimalsInOctoberToFebruariFail() {
        //        // Arrange
        //        List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
        //        Booking booking = new Booking();
        //        Animal animal1 = new Animal();
        //        animal1.Id = 1;
        //        animal1.Name = "Slang";
        //        animal1.Type = "Woestijn";
        //        BookingAnimal bookingAnimal1 = new BookingAnimal();
        //        bookingAnimal1.Animal = animal1;
        //        bookingAnimal1.AnimalId = animal1.Id;
        //        bookingAnimal1.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal1);

        //        AppUser user = new AppUser();
        //        booking.AppUser = user;
        //        booking.BookingAnimals = bookingAnimals;

        //        var myContextMock = new Mock<MyContext>();
        //        var entities = new List<Booking>() { booking };
        //        myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

        //        var myContextMockUser = new Mock<MyContext>();
        //        var entitiesUser = new List<AppUser>() { user };
        //        myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

        //        var myContextMockAnimals = new Mock<MyContext>();
        //        var entitiesAnimals = new List<Animal>() { animal1 };
        //        myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

        //        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        //        var defaultContext = new DefaultHttpContext();
        //        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

        //        var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);
        //        var errorList = new List<string>();

        //        foreach (BookingAnimal animal in booking.BookingAnimals) {
        //            bookingService.AddToAnimalList(animal.AnimalId);
        //        }

        //        DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 01, 26));
        //        bookingService.SetDateString(datenow.ToString());

        //        // Act
        //        var result = bookingService.ValidateAnimals(out errorList);

        //        // Assert
        //        Assert.AreEqual(result, false);
        //    }

        //    [TestMethod]
        //    public void NoSnowAnimalsInJuneToAugustSucces() {
        //        // Arrange
        //        List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
        //        Booking booking = new Booking();
        //        Animal animal1 = new Animal();
        //        animal1.Id = 1;
        //        animal1.Name = "Zeehond";
        //        animal1.Type = "Sneeuw";
        //        BookingAnimal bookingAnimal1 = new BookingAnimal();
        //        bookingAnimal1.Animal = animal1;
        //        bookingAnimal1.AnimalId = animal1.Id;
        //        bookingAnimal1.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal1);

        //        AppUser user = new AppUser();
        //        booking.AppUser = user;
        //        booking.BookingAnimals = bookingAnimals;

        //        var myContextMock = new Mock<MyContext>();
        //        var entities = new List<Booking>() { booking };
        //        myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

        //        var myContextMockUser = new Mock<MyContext>();
        //        var entitiesUser = new List<AppUser>() { user };
        //        myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

        //        var myContextMockAnimals = new Mock<MyContext>();
        //        var entitiesAnimals = new List<Animal>() { animal1 };
        //        myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

        //        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        //        var defaultContext = new DefaultHttpContext();
        //        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

        //        var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);
        //        var errorList = new List<string>();

        //        foreach (BookingAnimal animal in booking.BookingAnimals) {
        //            bookingService.AddToAnimalList(animal.AnimalId);
        //        }

        //        DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 01, 26));
        //        bookingService.SetDateString(datenow.ToString());

        //        // Act
        //        var result = bookingService.ValidateAnimals(out errorList);

        //        // Assert
        //        Assert.AreEqual(result, true);
        //    }

        //    [TestMethod]
        //    public void NoSnowAnimalsInJuneToAugustFail() {
        //        // Arrange
        //        List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
        //        Booking booking = new Booking();
        //        Animal animal1 = new Animal();
        //        animal1.Id = 1;
        //        animal1.Name = "Zeehond";
        //        animal1.Type = "Sneeuw";
        //        BookingAnimal bookingAnimal1 = new BookingAnimal();
        //        bookingAnimal1.Animal = animal1;
        //        bookingAnimal1.AnimalId = animal1.Id;
        //        bookingAnimal1.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal1);

        //        AppUser user = new AppUser();
        //        booking.AppUser = user;
        //        booking.BookingAnimals = bookingAnimals;

        //        var myContextMock = new Mock<MyContext>();
        //        var entities = new List<Booking>() { booking };
        //        myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

        //        var myContextMockUser = new Mock<MyContext>();
        //        var entitiesUser = new List<AppUser>() { user };
        //        myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

        //        var myContextMockAnimals = new Mock<MyContext>();
        //        var entitiesAnimals = new List<Animal>() { animal1 };
        //        myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

        //        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        //        var defaultContext = new DefaultHttpContext();
        //        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

        //        var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);
        //        var errorList = new List<string>();

        //        foreach (BookingAnimal animal in booking.BookingAnimals) {
        //            bookingService.AddToAnimalList(animal.AnimalId);
        //        }

        //        DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 07, 26));
        //        bookingService.SetDateString(datenow.ToString());

        //        // Act
        //        var result = bookingService.ValidateAnimals(out errorList);

        //        // Assert
        //        Assert.AreEqual(result, false);
        //    }

        //    [TestMethod]
        //    public void AnimalLimitNoCardSucces() {
        //        // Arrange
        //        List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
        //        Booking booking = new Booking();
        //        Animal animal1 = new Animal();
        //        BookingAnimal bookingAnimal1 = new BookingAnimal();
        //        bookingAnimal1.Animal = animal1;
        //        bookingAnimal1.AnimalId = animal1.Id;
        //        bookingAnimal1.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal1);
        //        Animal animal2 = new Animal();
        //        BookingAnimal bookingAnimal2 = new BookingAnimal();
        //        bookingAnimal2.Animal = animal2;
        //        bookingAnimal2.AnimalId = animal2.Id;
        //        bookingAnimal2.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal2);
        //        Animal animal3 = new Animal();
        //        BookingAnimal bookingAnimal3 = new BookingAnimal();
        //        bookingAnimal3.Animal = animal3;
        //        bookingAnimal3.AnimalId = animal3.Id;
        //        bookingAnimal3.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal3);

        //        AppUser user = new AppUser();
        //        booking.AppUser = user;
        //        booking.BookingAnimals = bookingAnimals;

        //        var myContextMock = new Mock<MyContext>();
        //        var entities = new List<Booking>() { booking };
        //        myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

        //        var myContextMockUser = new Mock<MyContext>();
        //        var entitiesUser = new List<AppUser>() { user };
        //        myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

        //        var myContextMockAnimals = new Mock<MyContext>();
        //        var entitiesAnimals = new List<Animal>() { animal1, animal2, animal3 };
        //        myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

        //        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        //        var defaultContext = new DefaultHttpContext();
        //        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

        //        var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);
        //        var errorList = new List<string>();

        //        foreach (BookingAnimal animal in booking.BookingAnimals) {
        //            bookingService.AddToAnimalList(animal.AnimalId);
        //        }

        //        DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 08, 26));
        //        bookingService.SetDateString(datenow.ToString());
        //        bookingService.SetUser(user.Id);

        //        // Act
        //        var result = bookingService.ValidateUserCard(out errorList);

        //        // Assert
        //        Assert.AreEqual(result, true);
        //    }

        //    [TestMethod]
        //    public void AnimalLimitNoCardFail() {
        //        // Arrange
        //        List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
        //        Booking booking = new Booking();
        //        Animal animal1 = new Animal();
        //        BookingAnimal bookingAnimal1 = new BookingAnimal();
        //        bookingAnimal1.Animal = animal1;
        //        bookingAnimal1.AnimalId = animal1.Id;
        //        bookingAnimal1.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal1);
        //        Animal animal2 = new Animal();
        //        BookingAnimal bookingAnimal2 = new BookingAnimal();
        //        bookingAnimal2.Animal = animal2;
        //        bookingAnimal2.AnimalId = animal2.Id;
        //        bookingAnimal2.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal2);
        //        Animal animal3 = new Animal();
        //        BookingAnimal bookingAnimal3 = new BookingAnimal();
        //        bookingAnimal3.Animal = animal3;
        //        bookingAnimal3.AnimalId = animal3.Id;
        //        bookingAnimal3.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal3);
        //        Animal animal4 = new Animal();
        //        BookingAnimal bookingAnimal4 = new BookingAnimal();
        //        bookingAnimal4.Animal = animal4;
        //        bookingAnimal4.AnimalId = animal4.Id;
        //        bookingAnimal4.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal4);

        //        AppUser user = new AppUser();
        //        booking.AppUser = user;
        //        booking.BookingAnimals = bookingAnimals;

        //        var myContextMock = new Mock<MyContext>();
        //        var entities = new List<Booking>() { booking };
        //        myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

        //        var myContextMockUser = new Mock<MyContext>();
        //        var entitiesUser = new List<AppUser>() { user };
        //        myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

        //        var myContextMockAnimals = new Mock<MyContext>();
        //        var entitiesAnimals = new List<Animal>() { animal1, animal2, animal3 };
        //        myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

        //        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        //        var defaultContext = new DefaultHttpContext();
        //        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

        //        var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);
        //        var errorList = new List<string>();

        //        foreach (BookingAnimal animal in booking.BookingAnimals) {
        //            bookingService.AddToAnimalList(animal.AnimalId);
        //        }

        //        DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 08, 26));
        //        bookingService.SetDateString(datenow.ToString());
        //        bookingService.SetUser(user.Id);

        //        // Act
        //        var result = bookingService.ValidateUserCard(out errorList);

        //        // Assert
        //        Assert.AreEqual(result, false);
        //    }

        //    [TestMethod]
        //    public void AnimalLimitSilverCardSucces() {
        //        // Arrange
        //        List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
        //        Booking booking = new Booking();
        //        Animal animal1 = new Animal();
        //        BookingAnimal bookingAnimal1 = new BookingAnimal();
        //        bookingAnimal1.Animal = animal1;
        //        bookingAnimal1.AnimalId = animal1.Id;
        //        bookingAnimal1.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal1);
        //        Animal animal2 = new Animal();
        //        BookingAnimal bookingAnimal2 = new BookingAnimal();
        //        bookingAnimal2.Animal = animal2;
        //        bookingAnimal2.AnimalId = animal2.Id;
        //        bookingAnimal2.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal2);
        //        Animal animal3 = new Animal();
        //        BookingAnimal bookingAnimal3 = new BookingAnimal();
        //        bookingAnimal3.Animal = animal3;
        //        bookingAnimal3.AnimalId = animal3.Id;
        //        bookingAnimal3.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal3);
        //        Animal animal4 = new Animal();
        //        BookingAnimal bookingAnimal4 = new BookingAnimal();
        //        bookingAnimal4.Animal = animal4;
        //        bookingAnimal4.AnimalId = animal4.Id;
        //        bookingAnimal4.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal4);

        //        AppUser user = new AppUser();
        //        user.Card = "Zilver";
        //        booking.AppUser = user;
        //        booking.BookingAnimals = bookingAnimals;

        //        var myContextMock = new Mock<MyContext>();
        //        var entities = new List<Booking>() { booking };
        //        myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

        //        var myContextMockUser = new Mock<MyContext>();
        //        var entitiesUser = new List<AppUser>() { user };
        //        myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

        //        var myContextMockAnimals = new Mock<MyContext>();
        //        var entitiesAnimals = new List<Animal>() { animal1, animal2, animal3, animal4 };
        //        myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

        //        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        //        var defaultContext = new DefaultHttpContext();
        //        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

        //        var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);
        //        var errorList = new List<string>();

        //        foreach (BookingAnimal animal in booking.BookingAnimals) {
        //            bookingService.AddToAnimalList(animal.AnimalId);
        //        }

        //        DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 08, 26));
        //        bookingService.SetDateString(datenow.ToString());
        //        bookingService.SetUser(user.Id);

        //        // Act
        //        var result = bookingService.ValidateUserCard(out errorList);

        //        // Assert
        //        Assert.AreEqual(result, true);
        //    }

        //    [TestMethod]
        //    public void AnimalLimitSilverCardFail() {
        //        // Arrange
        //        List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
        //        Booking booking = new Booking();
        //        Animal animal1 = new Animal();
        //        BookingAnimal bookingAnimal1 = new BookingAnimal();
        //        bookingAnimal1.Animal = animal1;
        //        bookingAnimal1.AnimalId = animal1.Id;
        //        bookingAnimal1.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal1);
        //        Animal animal2 = new Animal();
        //        BookingAnimal bookingAnimal2 = new BookingAnimal();
        //        bookingAnimal2.Animal = animal2;
        //        bookingAnimal2.AnimalId = animal2.Id;
        //        bookingAnimal2.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal2);
        //        Animal animal3 = new Animal();
        //        BookingAnimal bookingAnimal3 = new BookingAnimal();
        //        bookingAnimal3.Animal = animal3;
        //        bookingAnimal3.AnimalId = animal3.Id;
        //        bookingAnimal3.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal3);
        //        Animal animal4 = new Animal();
        //        BookingAnimal bookingAnimal4 = new BookingAnimal();
        //        bookingAnimal4.Animal = animal4;
        //        bookingAnimal4.AnimalId = animal4.Id;
        //        bookingAnimal4.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal4);
        //        Animal animal5 = new Animal();
        //        BookingAnimal bookingAnimal5 = new BookingAnimal();
        //        bookingAnimal5.Animal = animal5;
        //        bookingAnimal5.AnimalId = animal5.Id;
        //        bookingAnimal5.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal5);

        //        AppUser user = new AppUser();
        //        user.Card = "Zilver";
        //        booking.AppUser = user;
        //        booking.BookingAnimals = bookingAnimals;

        //        var myContextMock = new Mock<MyContext>();
        //        var entities = new List<Booking>() { booking };
        //        myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

        //        var myContextMockUser = new Mock<MyContext>();
        //        var entitiesUser = new List<AppUser>() { user };
        //        myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

        //        var myContextMockAnimals = new Mock<MyContext>();
        //        var entitiesAnimals = new List<Animal>() { animal1, animal2, animal3, animal4, animal5 };
        //        myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

        //        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        //        var defaultContext = new DefaultHttpContext();
        //        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

        //        var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);
        //        var errorList = new List<string>();

        //        foreach (BookingAnimal animal in booking.BookingAnimals) {
        //            bookingService.AddToAnimalList(animal.AnimalId);
        //        }

        //        DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 08, 26));
        //        bookingService.SetDateString(datenow.ToString());
        //        bookingService.SetUser(user.Id);

        //        // Act
        //        var result = bookingService.ValidateUserCard(out errorList);

        //        // Assert
        //        Assert.AreEqual(result, false);

        //    }

        //    [TestMethod]
        //    public void VipAnimalsSucces() {
        //        // Arrange
        //        List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
        //        Booking booking = new Booking();
        //        Animal animal1 = new Animal();
        //        animal1.Id = 1;
        //        animal1.Name = "T-Rex";
        //        animal1.Type = "VIP";
        //        BookingAnimal bookingAnimal1 = new BookingAnimal();
        //        bookingAnimal1.Animal = animal1;
        //        bookingAnimal1.AnimalId = animal1.Id;
        //        bookingAnimal1.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal1);

        //        AppUser user = new AppUser();
        //        user.Card = "Platina";
        //        booking.AppUser = user;
        //        booking.BookingAnimals = bookingAnimals;

        //        var myContextMock = new Mock<MyContext>();
        //        var entities = new List<Booking>() { booking };
        //        myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

        //        var myContextMockUser = new Mock<MyContext>();
        //        var entitiesUser = new List<AppUser>() { user };
        //        myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

        //        var myContextMockAnimals = new Mock<MyContext>();
        //        var entitiesAnimals = new List<Animal>() { animal1 };
        //        myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

        //        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        //        var defaultContext = new DefaultHttpContext();
        //        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

        //        var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);
        //        var errorList = new List<string>();

        //        foreach (BookingAnimal animal in booking.BookingAnimals) {
        //            bookingService.AddToAnimalList(animal.AnimalId);
        //        }

        //        DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 07, 26));
        //        bookingService.SetDateString(datenow.ToString());
        //        bookingService.SetUser(user.Id);

        //        // Act
        //        var result = bookingService.ValidateUserCard(out errorList);

        //        // Assert
        //        Assert.AreEqual(result, true);
        //    }

        //    [TestMethod]
        //    public void VipAnimalsFail() {
        //        // Arrange
        //        List<BookingAnimal> bookingAnimals = new List<BookingAnimal>();
        //        Booking booking = new Booking();
        //        Animal animal1 = new Animal();
        //        animal1.Id = 1;
        //        animal1.Name = "T-Rex";
        //        animal1.Type = "VIP";
        //        BookingAnimal bookingAnimal1 = new BookingAnimal();
        //        bookingAnimal1.Animal = animal1;
        //        bookingAnimal1.AnimalId = animal1.Id;
        //        bookingAnimal1.Booking = booking;
        //        bookingAnimals.Add(bookingAnimal1);

        //        AppUser user = new AppUser();
        //        user.Card = "Goud";
        //        booking.AppUser = user;
        //        booking.BookingAnimals = bookingAnimals;

        //        var myContextMock = new Mock<MyContext>();
        //        var entities = new List<Booking>() { booking };
        //        myContextMock.Setup(c => c.Bookings).ReturnsDbSet(entities);

        //        var myContextMockUser = new Mock<MyContext>();
        //        var entitiesUser = new List<AppUser>() { user };
        //        myContextMock.Setup(c => c.AppUsers).ReturnsDbSet(entitiesUser);

        //        var myContextMockAnimals = new Mock<MyContext>();
        //        var entitiesAnimals = new List<Animal>() { animal1 };
        //        myContextMock.Setup(c => c.Animals).ReturnsDbSet(entitiesAnimals);

        //        var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        //        var defaultContext = new DefaultHttpContext();
        //        mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(defaultContext);

        //        var bookingService = new BookingService(myContextMock.Object, mockHttpContextAccessor.Object);
        //        var errorList = new List<string>();

        //        foreach (BookingAnimal animal in booking.BookingAnimals) {
        //            bookingService.AddToAnimalList(animal.AnimalId);
        //        }

        //        DateOnly datenow = DateOnly.FromDateTime(new DateTime(2025, 07, 26));
        //        bookingService.SetDateString(datenow.ToString());
        //        bookingService.SetUser(user.Id);

        //        // Act
        //        var result = bookingService.ValidateUserCard(out errorList);

        //        // Assert
        //        Assert.AreEqual(result, false);
        //    }

        private IHttpContextAccessor GetHttpContextAccessorWithSession() {
            var sessionMock = new Mock<ISession>();

            // Setup GetString to return a stored value (simulating real session behavior)
            var sessionStorage = new Dictionary<string, byte[]>();
            sessionMock.Setup(s => s.Set(It.IsAny<string>(), It.IsAny<byte[]>()))
                       .Callback<string, byte[]>((key, value) => sessionStorage[key] = value);

            sessionMock.Setup(s => s.TryGetValue(It.IsAny<string>(), out It.Ref<byte[]>.IsAny))
                       .Returns((string key, out byte[] value) => {
                           var exists = sessionStorage.TryGetValue(key, out var storedValue);
                           value = storedValue;
                           return exists;
                       });

            var contextMock = new Mock<HttpContext>();
            var httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            contextMock.Setup(ctx => ctx.Session).Returns(sessionMock.Object);
            httpContextAccessorMock.Setup(acc => acc.HttpContext).Returns(contextMock.Object);

            return httpContextAccessorMock.Object;
        }
    }
}
