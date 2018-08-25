using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Moq;
using database.DataModel;


namespace XUnitTestProject1.Fake
{
    public class FakeIdentitySetuper
    {
        public static void Setup(out Mock<Fake.FakeUserManager> userManager, out Mock<Fake.FakeSignInManager> signInManager,
           IQueryable<Users> usersCollection)
        {
            userManager = new Mock<FakeUserManager>();
            userManager.Setup(s => s.Users).Returns(usersCollection);

            userManager.Setup(x => x.DeleteAsync(It.IsAny<Users>()))
                .ReturnsAsync(IdentityResult.Success);
            userManager.Setup(x => x.CreateAsync(It.IsAny<Users>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);
            userManager.Setup(x => x.UpdateAsync(It.IsAny<Users>()))
                .ReturnsAsync(IdentityResult.Success);
            userManager.Setup(x => x.ChangePasswordAsync(It.IsAny<Users>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            var uservalidator = new Mock<IUserValidator<Users>>();
            uservalidator.Setup(x => x.ValidateAsync(It.IsAny<UserManager<Users>>(), It.IsAny<Users>()))
                .ReturnsAsync(IdentityResult.Success);
            var passwordvalidator = new Mock<IPasswordValidator<Users>>();
            passwordvalidator.Setup(x =>
                    x.ValidateAsync(It.IsAny<UserManager<Users>>(), It.IsAny<Users>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success);

            signInManager = new Mock<FakeSignInManager>();

            signInManager.Setup(
                    x => x.PasswordSignInAsync(It.IsAny<Users>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success);
        }
    }
}