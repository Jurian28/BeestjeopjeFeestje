using BeestjeOpJeFeestje.Controllers;
using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace BeestjeOpJeFeestjeTest {
    [TestClass]
    public class BoerderijControllerTest {
        private DbContextOptions<MyContext> _contextOptions;

        public BoerderijControllerTest() {
            _contextOptions = new DbContextOptionsBuilder<MyContext>()
                .UseInMemoryDatabase("BeestjeOpJeFeestjeTestDb") 
                .Options;
        }

        private MyContext GetContext() {
            var myContext = new MyContext(_contextOptions);

            myContext.Animals.RemoveRange(myContext.Animals); // to remove the animal data thats in the migration
            return myContext;
        }


        [TestMethod]
        public void Index_ShouldReturn_ViewResult_WithAnimals() {
            // Arrange
            using (var context = GetContext()) {
                context.Animals.Add(new Animal { Name = "Lion" });
                context.Animals.Add(new Animal { Name = "Ant" });
                context.SaveChanges();

                var controller = new BoerderijController(context);

                // Act
                var result = controller.Index();

                // Assert
                Assert.IsInstanceOfType(result, typeof(ViewResult)); 
                var viewResult = result as ViewResult;
                Assert.IsNotNull(viewResult);

                var model = viewResult.Model as IEnumerable<Animal>;
                var modelList = model.ToList();
                Assert.IsNotNull(model);
                Assert.AreEqual(2, model.Count()); 
                Assert.AreEqual("Lion", model.First().Name);
            }
        }

        [TestMethod]
        public void Create_Get_ShouldReturn_ViewResult_WithNewAnimal() {
            // Arrange
            using (var context = GetContext()) {
                var controller = new BoerderijController(context);

                // Act
                var result = controller.Create();

                // Assert
                Assert.IsInstanceOfType(result, typeof(ViewResult)); 
                var viewResult = result as ViewResult;
                Assert.IsNotNull(viewResult);

                var model = viewResult.Model as Animal;
                Assert.IsNotNull(model); 
                Assert.AreEqual("", model.Type); 
                Assert.AreEqual(0, model.Price); 
                Assert.AreEqual("", model.ImageUrl); 
            }
        }

        [TestMethod]
        public void Create_Post_ShouldRedirectToIndex_WhenModelIsValid() {
            // Arrange
            using (var context = GetContext()) {
                var controller = new BoerderijController(context);

                var newAnimal = new Animal {
                    Name = "Tiger" 
                };

                // Act
                var result = controller.Create(newAnimal);

                // Assert
                Assert.IsInstanceOfType(result, typeof(RedirectToActionResult)); 
                var redirectResult = result as RedirectToActionResult;
                Assert.AreEqual("Index", redirectResult.ActionName);


                var animalInDb = context.Animals.FirstOrDefault(a => a.Name == "Tiger");
                Assert.IsNotNull(animalInDb);
                Assert.AreEqual("Tiger", animalInDb.Name);
            }
        }

        [TestMethod]
        public void Edit_Get_ShouldReturn_ViewResult_WithAnimal() {
            // Arrange
            using (var context = GetContext()) {
                var animal = new Animal {
                    Name = "Elephant" 
                };

                context.Animals.Add(animal);
                context.SaveChanges();

                var controller = new BoerderijController(context);

                // Act
                var result = controller.Edit(animal.Id);

                // Assert
                Assert.IsInstanceOfType(result, typeof(ViewResult)); 
                var viewResult = result as ViewResult;
                Assert.IsNotNull(viewResult);

                var model = viewResult.Model as Animal;
                Assert.IsNotNull(model); 
                Assert.AreEqual("Elephant", model.Name); 
            }
        }


        [TestMethod]
        public void Edit_Post_ShouldRedirectToIndex_WhenModelIsValid() {
            // Arrange
            using (var context = GetContext()) {
                var animal = new Animal {
                    Name = "Elephant" 
                };

                context.Animals.Add(animal);
                context.SaveChanges();

                var controller = new BoerderijController(context);

                animal.Name = "Updated Elephant";

                // Act
                var result = controller.Edit(animal);

                // Assert
                Assert.IsInstanceOfType(result, typeof(RedirectToActionResult)); 
                var redirectResult = result as RedirectToActionResult;
                Assert.AreEqual("Index", redirectResult.ActionName);

                var updatedAnimal = context.Animals.FirstOrDefault(a => a.Name == "Updated Elephant");
                Assert.IsNotNull(updatedAnimal);
                Assert.AreEqual("Updated Elephant", updatedAnimal.Name);
            }
        }


        [TestMethod]
        public void Delete_ShouldRedirectToIndex_WhenAnimalIsDeleted() {
            // Arrange
            using (var context = GetContext()) {
                var animal = new Animal {
                    Name = "Lion" 
                };

                context.Animals.Add(animal);
                context.SaveChanges();

                var controller = new BoerderijController(context);

                // Act
                var result = controller.Delete(animal.Id);

                // Assert
                Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));

                var deletedAnimal = context.Animals.FirstOrDefault(a => a.Name == "Lion");
                Assert.IsNull(deletedAnimal); 
            }
        }
    }
}
