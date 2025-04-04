using BeestjeOpJeFeestje.Controllers;
using BeestjeOpJeFeestjeDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;

namespace BeestjeOpJeFeestjeTest {
    [TestClass]
    public class BoerderijControllerTest {

        [TestMethod]
        public void Index_ShouldReturn_ViewResult_WithAnimals_WithoutMocks() {
            var options = new DbContextOptionsBuilder<MyContext>()
                .UseInMemoryDatabase(databaseName: "AnimalTestDb")
                .Options;

            using (var context = new MyContext(options)) {
                context.Animals.Add(new Animal { Name = "Lion" });
                context.Animals.Add(new Animal { Name = "Paard" });
                context.SaveChanges();

                var controller = new BoerderijController(context);

                var result = controller.Index();

                Assert.IsInstanceOfType(result, typeof(ViewResult));

                var viewResult = result as ViewResult;
                Assert.IsNotNull(viewResult);

                var model = viewResult.Model as IEnumerable<Animal>;
                Assert.IsNotNull(model);
                Assert.AreEqual(2, model.Count());
                Assert.AreEqual("Lion", model.First().Name);
            }
        }
    }
}
