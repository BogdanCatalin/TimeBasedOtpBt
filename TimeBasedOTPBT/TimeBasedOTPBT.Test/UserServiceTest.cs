using Microsoft.Extensions.Localization;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using TimeBasedOTPBT.BusinessLogic.Services.TOTPService;
using TimeBasedOTPBT.BusinessLogic.Services.UserService;
using TimeBasedOTPBT.Common.Exceptions;
using TimeBasedOTPBT.Persistence.Contexts;
using TimeBasedOTPBT.Persistence.Entities;
using TimeBasedOTPBT.Test.Helpers;
using TimeBasedOTPBT.Test.Helpers.Password;
using Xunit;

namespace TimeBasedOTPBT.Test
{
    public class UserServiceTest : IDisposable
    {
        private readonly Factory _factory;
        private readonly SqliteDataContext _db;
        private readonly UserService _service;
        private readonly IPasswordHelper _passwordHelper;
        private readonly ITimeBasedOneTimePasswordService _totpService;

        public UserServiceTest()
        {
            var passwordHelperLocalizer = Substitute.For<IStringLocalizer<PasswordHelper>>();
            passwordHelperLocalizer[Arg.Any<string>()].Returns(p => new LocalizedString((string)p[0], (string)p[0]));

            _passwordHelper = new PasswordHelper(passwordHelperLocalizer);
            _totpService = new TimeBasedOneTimePasswordNuGetService();

            var dataHelper = new DataHelper();
            _db = dataHelper.CreateDbContext(_passwordHelper);
            _factory = dataHelper.Factory;

            _service = new UserService(_db, _totpService);
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        [Fact]
        public void Create_WhenCalled_CreatesUser()
        {
            // Arrange
            var (passwordHash, passwordSalt) = _passwordHelper.CreateHash("test_password");

            var user = new User
            {
                FirstName = "test first name",
                LastName = "test last name",
                Username = "test_username",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            // Act
            var response = _service.Create(user, "test_password");

            // Assert
            Assert.Equal(user.Username, response.Username);
            Assert.Equal(user.FirstName, response.FirstName);
            Assert.Equal(user.LastName, response.LastName);

            var userFromDb = _db.Users.Single(x => x.Username == user.Username);
            Assert.Equal(user.Username, userFromDb.Username);
            Assert.Equal(user.FirstName, userFromDb.FirstName);
            Assert.Equal(user.LastName, userFromDb.LastName);
            Assert.True(_passwordHelper.VerifyHash("test_password", userFromDb.PasswordHash, userFromDb.PasswordSalt));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Create_PasswordIsNullOrWhiteSpace_ThrowsAppException(string password)
        {
            // Arrange
            var user = new User
            {
                FirstName = "test first name",
                LastName = "test last name",
                Username = "test_username",
                PasswordHash = new byte[20],
                PasswordSalt = new byte[20]
            };

            // Act and Assert
            Assert.Throws<AppException>(() => _service.Create(user, password));
        }

        [Fact]
        public void Create_UsernameIsTaken_ThrowsUsernameTakenException()
        {
            // Arrange
            var user = _factory.CreateUsers(1, "test_password")[0];

            var newUser = new User
            {
                FirstName = "test first name",
                LastName = "test last name",
                Username = user.Username,
                PasswordHash = new byte[20],
                PasswordSalt = new byte[20]
            };

            // Act and Assert
            Assert.Throws<AppException>(() => _service.Create(user, "test_passwordd"));
        }

        [Fact]
        public void Preauthenticate_WhenCalled_ReturnsTotp()
        {
            // Arrange
            const string password = "test_password";
            var user = _factory.CreateUsers(2, password)[0];

            var usernameForAuth = user.Username;
            var passwordForAuth = password;

            var userFromDb = _db.Users.Single(x => x.Username == user.Username);

            // Act
            dynamic response = _service.PreAuthenticate(usernameForAuth, passwordForAuth);
            var totpPassword = response.GetType().GetProperty("TotpPassword").GetValue(response, null);

            // Assert
            Assert.True(_totpService.VerifyTotpPassword(totpPassword, userFromDb.Id));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Preauthenticate_UsernameIsNullOrEmpty_ReturnsNull(string username)
        {
            // Arrange

            // Act 
            var response = _service.PreAuthenticate(username, "test_password");

            // Assert
            Assert.Null(response);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Preauthenticate_PasswordIsNullOrEmpty_ReturnsNull(string password)
        {
            // Arrange

            // Act 
            var response = _service.PreAuthenticate("test_username", password);

            // Assert
            Assert.Null(response);
        }

        [Fact]
        public void Preauthenticate_UserDoesNotExists_ReturnsNull()
        {
            // Arrange

            // Act 
            var response = _service.PreAuthenticate("test_username", "test_password");

            // Assert
            Assert.Null(response);
        }

        [Fact]
        public void Authenticate_WhenTotpPasswordIsCorrent_ReturnsTrue()
        {
            // Arrange
            const string password = "test_password";
            var user = _factory.CreateUsers(2, password)[0];

            var usernameForAuth = user.Username;
            var passwordForAuth = password;

            var userFromDb = _db.Users.Single(x => x.Username == user.Username);

            // Act
            dynamic preauthenticateResponse = _service.PreAuthenticate(usernameForAuth, passwordForAuth);
            var totpPassword = preauthenticateResponse.GetType().GetProperty("TotpPassword").GetValue(preauthenticateResponse, null);

            var response = _service.Authenticate(userFromDb.Id.ToString(), totpPassword);

            // Assert
            Assert.True(response);
        }

        [Fact]
        public void Authenticate_WhenTotpPasswordIsIncorrent_ReturnsFalse()
        {
            // Arrange
            const string password = "test_password";
            var user = _factory.CreateUsers(2, password)[0];

            var usernameForAuth = user.Username;
            var passwordForAuth = password;

            var userFromDb = _db.Users.Single(x => x.Username == user.Username);

            // Act
            dynamic preauthenticateResponse = _service.PreAuthenticate(usernameForAuth, passwordForAuth);
            var totpPassword = preauthenticateResponse.GetType().GetProperty("TotpPassword").GetValue(preauthenticateResponse, null);

            var response = _service.Authenticate(userFromDb.Id.ToString(), "wrong_totpPassword");

            // Assert
            Assert.False(response);
        }
    }
}
