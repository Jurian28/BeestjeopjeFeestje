using BeestjeOpJeFeestje.Controllers;
using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BeestjeOpJeFeestjeTest {
    [TestClass]
    public class AccountControllerTest {
        private DbContextOptions<MyContext> _contextOptions;

        public AccountControllerTest() {
            _contextOptions = new DbContextOptionsBuilder<MyContext>()
                .UseInMemoryDatabase("BeestjeOpJeFeestjeTestDb")
                .Options;
        }

        private MyContext GetContext() {
            var myContext = new MyContext(_contextOptions);

            myContext.AppUsers.RemoveRange(myContext.AppUsers);
            return myContext;
        }

        [TestMethod]
        public void Index_ShouldReturn_ViewResultWithUsers() {
            // Arrange
            using (var context = GetContext()) {
                context.AppUsers.Add(new AppUser { UserName = "Ethan", Address = "Den Bosch" });
                context.AppUsers.Add(new AppUser { UserName = "Jurian", Address = "Den Bosch" });
                context.SaveChanges();

                var controller = new AccountsController(context);

                // Act
                var result = controller.Index();

                // Assert
                Assert.IsInstanceOfType(result, typeof(ViewResult));
                var viewResult = result as ViewResult;
                Assert.IsNotNull(viewResult);

                var model = viewResult.Model as IEnumerable<AppUser>;
                var modelList = model.ToList();
                Assert.IsNotNull(model);
                Assert.AreEqual(2, model.Count());
                Assert.AreEqual("Ethan", model.First().UserName);
            }
        }

        [TestMethod]
        public void SetCard_Silver_To_Gold() {
            using (var context = GetContext()) {
                var user1 = new AppUser { UserName = "Ethan", Id = "1", Address = "Den Bosch" };
                var user2 = new AppUser { UserName = "Jurian", Id = "2", Address = "Den Bosch", Card = "Zilver" };

                context.AppUsers.Add(user1);
                context.AppUsers.Add(user2);

                context.SaveChanges();
                var controller = new AccountsController(context);

                // Act
                var result = controller.SetCard(user2.Id);

                // Assert
                Assert.AreEqual("Goud", user2.Card);

                Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
                var redirectResult = result as RedirectToActionResult;
                Assert.AreEqual("Index", redirectResult.ActionName);
            }
        }

        [TestMethod]
        public void SetCard_Gold_To_Platinum() {
            using (var context = GetContext()) {
                var user1 = new AppUser { UserName = "Ethan", Id = "1", Address = "Den Bosch" };
                var user2 = new AppUser { UserName = "Jurian", Id = "2", Address = "Den Bosch", Card = "Goud" };

                context.AppUsers.Add(user1);
                context.AppUsers.Add(user2);

                context.SaveChanges();
                var controller = new AccountsController(context);

                // Act
                var result = controller.SetCard(user2.Id);

                // Assert
                Assert.AreEqual("Platina", user2.Card);

                Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
                var redirectResult = result as RedirectToActionResult;
                Assert.AreEqual("Index", redirectResult.ActionName);
            }
        }

        [TestMethod]
        public void SetCard_Platinum_To_None() {
            using (var context = GetContext()) {
                var user1 = new AppUser { UserName = "Ethan", Id = "1", Address = "Den Bosch" };
                var user2 = new AppUser { UserName = "Jurian", Id = "2", Address = "Den Bosch", Card = "Platina" };

                context.AppUsers.Add(user1);
                context.AppUsers.Add(user2);

                context.SaveChanges();
                var controller = new AccountsController(context);

                // Act
                var result = controller.SetCard(user2.Id);

                // Assert
                Assert.AreEqual("Geen", user2.Card);

                Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
                var redirectResult = result as RedirectToActionResult;
                Assert.AreEqual("Index", redirectResult.ActionName);
            }
        }

        [TestMethod]
        public void SetCard_NoCard_To_Silver() {
            using (var context = GetContext()) {
                var user1 = new AppUser { UserName = "Ethan", Id = "1", Address = "Den Bosch" };
                var user2 = new AppUser { UserName = "Jurian", Id = "2", Address = "Den Bosch", Card = "Zilver" };

                context.AppUsers.Add(user1);
                context.AppUsers.Add(user2);

                context.SaveChanges();
                var controller = new AccountsController(context);

                // Act
                var result = controller.SetCard(user1.Id);

                // Assert
                Assert.AreEqual("Zilver", user2.Card);

                Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
                var redirectResult = result as RedirectToActionResult;
                Assert.AreEqual("Index", redirectResult.ActionName);
            }
        }

        [TestMethod]
        public void SetCard_UserIdDoesNotExist_Redirects() {
            using (var context = GetContext()) {
                var user1 = new AppUser { UserName = "Ethan", Id = "1", Address = "Den Bosch" };
                var user2 = new AppUser { UserName = "Jurian", Id = "2", Address = "Den Bosch", Card = "Zilver" };

                context.AppUsers.Add(user1);
                context.AppUsers.Add(user2);

                context.SaveChanges();
                var controller = new AccountsController(context);

                // Act
                var result = controller.SetCard("4");

                // Assert
                Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
                var redirectResult = result as RedirectToActionResult;
                Assert.AreEqual("Index", redirectResult.ActionName);
            }
        }
    }
}
