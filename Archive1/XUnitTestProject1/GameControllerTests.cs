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
using Archive1.Models.interfases;
using Microsoft.Extensions.FileProviders;
using System.IO;

namespace XUnitTestProject1
{
    public class GameControllerTests
    {
        private readonly IQueryable<Users> users;
        private Mock<Fake.FakeUserManager> userManager;
        private Mock<Fake.FakeSignInManager> signInManager;

        public GameControllerTests()
        {
            users = new[] { new Users() { UserName = "Bohdan" }, new Users { UserName = "Modest" } }
                .AsQueryable();
            Fake.FakeIdentitySetuper.Setup(out userManager, out signInManager, users);
        }
        public void ViewGameTesting()
        {
            //Arrange
            Mock<IGameRepositore> mock = new Mock<IGameRepositore>();
            //  Development game = Developments.Where(c => c.Id == 1).SingleOrDefault();
            Development dev = new Development("Name", Convert.ToDateTime("12.12.2018 19:00"), 5);
            Game pps = new Game("Проектування", "Опис", Convert.ToDateTime("12.12.2018 19:00"), "Its", dev) { Id = 1 };
          //  mock.Setup(m => m.Games).Returns(courses.AsQueryable());

            GameController controller = new GameController(mock.Object, new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")), null);
            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, "bohdan.romaniuk"),
                        new Claim(ClaimTypes.Role, "Admin")
                    }, "Authentication"))
                }
            };
            //Act
            bool isCourseListener = (bool)(controller.ViewGame("1") as ViewResult).ViewData["IsCourseListener"];

            //Assert
            Assert.True(isCourseListener);
        }
    }
}

