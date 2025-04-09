using BeestjeOpJeFeestje.Models;
using BeestjeOpJeFeestjeDb.Models;
using Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace BeestjeOpJeFeestjeTest {
    [TestClass]
    public class AuthControllerTest {
        private Mock<UserManager<AppUser>> _userManagerMock;
        private Mock<SignInManager<AppUser>> _signInManagerMock;
        private Mock<MyContext> _contextMock;
        private AuthController _controller;

        [TestInitialize]
        public void Setup() {
            var store = new Mock<IUserStore<AppUser>>();
            _userManagerMock = new Mock<UserManager<AppUser>>(store.Object, null, null, null, null, null, null, null, null);

            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
            _signInManagerMock = new Mock<SignInManager<AppUser>>(
                _userManagerMock.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null);

            _contextMock = new Mock<MyContext>();
            _controller = new AuthController(_userManagerMock.Object, _signInManagerMock.Object, _contextMock.Object);
        }

        [TestMethod]
        public void Register_Get_ReturnsViewWithPassword() {
            var result = _controller.Register() as ViewResult;

            Assert.IsNotNull(result);
            var model = result.Model as RegisterForm;
            Assert.IsNotNull(model);
        }

        [TestMethod]
        public async Task Register_Post_Successful_CreatesUserAndRedirects() {
            var form = new RegisterForm {
                Name = "Ethan",
                Email = "test@example.com",
                Password = "123456"
            };

            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<AppUser>(), "klant"))
                .ReturnsAsync(IdentityResult.Success);
            _contextMock.Setup(c => c.SaveChanges());

            var result = await _controller.Register(form) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Boerderij", result.ControllerName);
        }

        [TestMethod]
        public async Task Register_Post_Failed_ReturnsView() {
            var form = new RegisterForm { Name = "Ethan", Address = "Den Bosch", Card = "Geen", Email = "email@test.com", Password = "123456", PasswordRepeat = "123456", PhoneNumber = "0612345678" };
            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "error" }));

            var result = await _controller.Register(form);

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task AddUserInfo_CreatesAndSignsInAndRedirects() {
            var form = new RegisterForm { Name = "Ethan", Email = "email@test.com" };
            var returnUrl = "Index";

            _userManagerMock.Setup(m => m.CreateAsync(It.IsAny<AppUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            _signInManagerMock.Setup(m => m.PasswordSignInAsync(It.IsAny<AppUser>(), It.IsAny<string>(), true, false))
                .ReturnsAsync(SignInResult.Success);
            _userManagerMock.Setup(m => m.AddToRoleAsync(It.IsAny<AppUser>(), "klant"))
                .ReturnsAsync(IdentityResult.Success);

            var result = await _controller.AddUserInfo(form, returnUrl) as RedirectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(returnUrl, result.Url);
        }

        [TestMethod]
        public void Login_Get_ReturnsViewWithReturnUrl() {
            var returnUrl = "Index";
            var result = _controller.Login(returnUrl) as ViewResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(returnUrl, result.ViewData["ReturnUrl"]);
        }

        [TestMethod]
        public async Task Login_Post_InvalidEmail_ReturnsView() {
            var form = new LoginForm { Email = "invalid@test.com", Password = "123" };
            _userManagerMock.Setup(m => m.FindByEmailAsync(form.Email))
                .ReturnsAsync((AppUser)null);

            var result = await _controller.Login(form) as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Login_Post_WrongPassword_ReturnsView() {
            var user = new AppUser { Email = "test@test.com" };
            var form = new LoginForm { Email = user.Email, Password = "wrongpass" };

            _userManagerMock.Setup(m => m.FindByEmailAsync(form.Email))
                .ReturnsAsync(user);
            _signInManagerMock.Setup(m => m.PasswordSignInAsync(user, form.Password, true, false))
                .ReturnsAsync(SignInResult.Failed);

            var result = await _controller.Login(form) as ViewResult;

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Login_Post_Success_RedirectsByRole() {
            var user = new AppUser { Email = "test@test.com" };
            var form = new LoginForm { Email = user.Email, Password = "123456" };

            _userManagerMock.Setup(m => m.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);
            _signInManagerMock.Setup(m => m.PasswordSignInAsync(user, form.Password, true, false))
                .ReturnsAsync(SignInResult.Success);
            _userManagerMock.Setup(m => m.IsInRoleAsync(user, "boerderij")).ReturnsAsync(true);

            var result = await _controller.Login(form) as RedirectToActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Boerderij", result.ControllerName);
        }
    }
}
