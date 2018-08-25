using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication;
using database.DataModel;
using Moq;
namespace XUnitTestProject1.Fake
{
    public class FakeSignInManager : SignInManager<Users>
    {
        //public SignInManager(UserManager<TUser> userManager, IHttpContextAccessor contextAccessor, IUserClaimsPrincipalFactory<TUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor, ILogger<SignInManager<TUser>> logger, IAuthenticationSchemeProvider schemes);
        public FakeSignInManager()
            : base(
                new FakeUserManager(),
                new HttpContextAccessor(),
                new Mock<IUserClaimsPrincipalFactory<Users>>().Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<ILogger<SignInManager<Users>>>().Object,
                            null)
    //new Mock<AuthenticationSchemeProvider>(
    //    MockBehavior.Default, 
    //    new Mock<IOptions<AuthenticationOptions>>().Object).Object)
    { }
}
}