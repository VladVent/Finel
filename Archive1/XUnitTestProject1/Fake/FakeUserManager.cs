using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using database.DataModel;

namespace XUnitTestProject1.Fake 
{
    public class FakeUserManager : UserManager<Users>
    {
        public FakeUserManager()
          : base(new Mock<IUserStore<Users>>().Object,
              new Mock<IOptions<IdentityOptions>>().Object,
              new Mock<IPasswordHasher<Users>>().Object,
              new IUserValidator<Users>[0],
              new IPasswordValidator<Users>[0],
              new Mock<ILookupNormalizer>().Object,
              new Mock<IdentityErrorDescriber>().Object,
              new Mock<IServiceProvider>().Object,
              new Mock<ILogger<UserManager<Users>>>().Object)
        {
        }
    }
}