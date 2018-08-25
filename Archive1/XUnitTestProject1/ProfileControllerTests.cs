using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Moq;
using Archive1.Models;
using database.DataModel;
using Archive1.Controllers;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Archive1.Models.interfases;

namespace XUnitTestProject1
{
   public class ProfileControllerTests
    {
        private readonly IQueryable<Users> users;
        private Mock<Fake.FakeUserManager> userManager;
        private Mock<Fake.FakeSignInManager> signInManager;

        public ProfileControllerTests()
        {
            users = new[] { new Users() { UserName = "Bohdan" }, new Users { UserName = "Modest" } }
                .AsQueryable();
            Fake.FakeIdentitySetuper.Setup(out userManager, out signInManager, users);
        }
        [Fact]
        public void LoginWrongPasswordTest()
        {
            Mock<IGameRepositore> mock = new Mock<IGameRepositore>();
            Users u1 = new Users { Name = "a1", UserName = "a1", PasswordHash = "123456" };
            Users[] users = new Users[] { u1 };

            mock.Setup(m => m.User).Returns(users.AsQueryable());
            LoginModel lm = new LoginModel();
            lm.UserName = "new user";
            lm.Password = "123456";
            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = !(controller.Login(lm, "a").Result is RedirectResult);

            Assert.True(actual);
        }
        [Fact]
        public void LoginGetTest()
        {
            Mock<IGameRepositore> mock = new Mock<IGameRepositore>();
            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = (controller.Login() is ViewResult);

            Assert.True(actual);
        }
        [Fact]
        public void RegisterGetTest()
        {
            Mock<IGameRepositore> mock = new Mock<IGameRepositore>();
            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = (controller.Register() is ViewResult);

            Assert.True(actual);
        }
        [Fact]
        public void LoginWrongUsernameTest()
        {
            Mock<IGameRepositore> mock = new Mock<IGameRepositore>();
            Users u1 = new Users { Name = "a1", UserName = "a1", PasswordHash = "123456" };
            Users[] users = new Users[] { u1 };

            mock.Setup(m => m.User).Returns(users.AsQueryable());
            LoginModel lm = new LoginModel();
            lm.UserName = "a1";
            lm.Password = "12345";
            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = !(controller.Login(lm, "a").Result is RedirectResult);

            Assert.True(actual);
        }
        [Fact]
        public void LogingUserTest()
        {
            Mock<IGameRepositore> mock = new Mock<IGameRepositore>();
            Users u1 = new Users { Name = "a1", UserName = "a1", PasswordHash = "123456" };
            Users[] users = new Users[] { u1 };

            mock.Setup(m => m.User).Returns(users.AsQueryable());
            LoginModel lm = new LoginModel();
            lm.UserName = "a1";
            lm.Password = "123456";
            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = !(controller.Login(lm, "a").Result is RedirectResult);

            Assert.True(actual);
        }
        [Fact]
        public void LogingOutTest()
        {
            ProfileController controller = new ProfileController(null, null, signInManager.Object);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "romaniuk.bohdan"),
                        new Claim(ClaimTypes.Role, "Student")
                    }, "Authentication"))
                }
            };
            bool redirected = (controller.Logout().Result is RedirectToActionResult);

            Assert.True(redirected);
        }
        [Fact]
        public void RegisterWrongUsernameTest()
        {
            Mock<IGameRepositore> mock = new Mock<IGameRepositore>();
            Users u1 = new Users { Name = "a1", UserName = "a1", PasswordHash = "123456" };
            Users[] users = new Users[] { u1 };

            mock.Setup(m => m.User).Returns(users.AsQueryable());
            RegisterModel rm = new RegisterModel() { UserName = "a1", Password = "1", Name = "a" };

            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = !(controller.Register(rm, "a").Result is RedirectResult);

            Assert.True(actual);
        }
        [Fact]
        public void RegisterNullPasswordTest()
        {
            Mock<IGameRepositore> mock = new Mock<IGameRepositore>();
            Users u1 = new Users { Name = "a1", UserName = "a1", PasswordHash = "123456" };
            Users[] users = new Users[] { u1 };

            mock.Setup(m => m.User).Returns(users.AsQueryable());
            RegisterModel rm = new RegisterModel() { UserName = "a1", Password = null, Name = "a" };

            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = !(controller.Register(rm, "a").Result is RedirectResult);

            Assert.True(actual);
        }
        [Fact]
        public void RegisterNullNameTest()
        {
            Mock<IGameRepositore> mock = new Mock<IGameRepositore>();
            Users u1 = new Users { Name = "a1", UserName = "a1", PasswordHash = "123456" };
            Users[] users = new Users[] { u1 };

            mock.Setup(m => m.User).Returns(users.AsQueryable());
            RegisterModel rm = new RegisterModel() { UserName = null, Password = "1", Name = "a" };

            ProfileController controller = new ProfileController(mock.Object, userManager.Object, signInManager.Object);
            bool actual = !(controller.Register(rm, "a").Result is RedirectResult);

            Assert.True(actual);
        }
    }
}