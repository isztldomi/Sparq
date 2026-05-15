using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Moq;
using Sparq.DataAccess.Config;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using System.Security.Claims;

namespace Sparq.Tests.UnitTests
{
    public class UsersServiceTests : IDisposable
    {
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<SignInManager<User>> _mockSignInManager;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;

        private readonly UsersService _usersService;

        public UsersServiceTests()
        {
            _mockUserManager = MockUserManager();

            _mockSignInManager = MockSignInManager(_mockUserManager);

            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            var jwtSettings = Options.Create(new JwtSettings
            {
                SecretKey = "secret_keey_12345",
                Issuer = "test-issuer",
                Audience = "test-audience",
                AccessTokenExpirationMinutes = 1
            });

            _usersService = new UsersService(
                jwtSettings,
                _mockHttpContextAccessor.Object,
                _mockUserManager.Object,
                _mockSignInManager.Object);
        }

        #region AddUser

        [Fact]
        public async Task AddUserAsync_ShouldCreateUser()
        {
            // Arrange
            var user = new User
            {
                Email = "test@test.com"
            };

            _mockUserManager
                .Setup(x => x.CreateAsync(user, "Password123"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _usersService.AddUserAsync(
                user,
                "Password123");

            // Assert
            Assert.True(result.Succeeded);

            Assert.Equal(user.Email, user.UserName);

            Assert.NotEqual(Guid.Empty, user.RefreshToken);
        }

        #endregion

        #region Login

        [Fact]
        public async Task LoginAsync_ShouldFail_WhenPasswordInvalid()
        {
            // Arrange
            var user = new User
            {
                Email = "test@test.com"
            };

            _mockUserManager
                .Setup(x => x.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.CheckPasswordAsync(user, "wrong"))
                .ReturnsAsync(false);

            // Act
            var result = await _usersService.LoginAsync(
                user.Email,
                "wrong");

            // Assert
            Assert.Null(result.authToken);

            Assert.Equal(
                "Invalid email or password",
                result.error);
        }

        #endregion

        #region CurrentUser

        [Fact]
        public void GetCurrentUserId_ShouldReturnUserId()
        {
            // Arrange
            var claims = new List<Claim>
            {
                new Claim("id", "user1")
            };

            var identity = new ClaimsIdentity(claims);

            var principal = new ClaimsPrincipal(identity);

            var context = new DefaultHttpContext
            {
                User = principal
            };

            _mockHttpContextAccessor
                .Setup(x => x.HttpContext)
                .Returns(context);

            // Act
            var result = _usersService.GetCurrentUserId();

            // Assert
            Assert.Equal("user1", result);
        }

        #endregion

        #region UpdateNickname

        [Fact]
        public async Task UpdateNickNameAsync_ShouldUpdateNickname()
        {
            // Arrange
            var user = new User
            {
                Id = "user1",
                NickName = "oldnickname"
            };

            _mockUserManager
                .Setup(x => x.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            _mockUserManager
                .Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _usersService.UpdateNickNameAsync(
                user.Id,
                "newnickname");

            // Assert
            Assert.Equal("newnickname", result.NickName);
        }

        #endregion

        #region Helpers

        private Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();

            return new Mock<UserManager<User>>(
                store.Object,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null);
        }

        private Mock<SignInManager<User>> MockSignInManager(
            Mock<UserManager<User>> userManager)
        {
            return new Mock<SignInManager<User>>(
                userManager.Object,
                new Mock<IHttpContextAccessor>().Object,
                new Mock<IUserClaimsPrincipalFactory<User>>().Object,
                null,
                null,
                null,
                null);
        }

        #endregion

        public void Dispose()
        {
        }
    }
}