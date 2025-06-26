using Book.API.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using IdentitySignInResult = Microsoft.AspNetCore.Identity.SignInResult;


namespace Book.API.Tests.Controllers
{
    public class UsersControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManager;
        private readonly Mock<SignInManager<AppUser>> _signInManager;
        private readonly Mock<IConfiguration> _config;
        private readonly Mock<ILogger<UsersController>> _logger;
        private readonly IConfiguration _configuration;

        public UsersControllerTests()
        {
            var userStore = new Mock<IUserStore<AppUser>>();
            _userManager = new Mock<UserManager<AppUser>>(userStore.Object, null, null, null, null, null, null, null, null);
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
            _signInManager = new Mock<SignInManager<AppUser>>(
                _userManager.Object, contextAccessor.Object, claimsFactory.Object, null, null, null, null
            );
            _config = new Mock<IConfiguration>();
            _logger = new Mock<ILogger<UsersController>>();
            _configuration = ConfigurationHelper.LoadConfiguration();
        }

        [Fact]
        public async Task Login_ShouldReturnToken_WhenCredentialsValid()
        {
            // Arrange
            var user = new AppUser { UserName = "test", Email = "test@mail.com", Id = "123" };
            var loginDto = new LoginDto { Email = "test@mail.com", Password = "123456" };

            _userManager.Setup(u => u.FindByEmailAsync(loginDto.Email)).ReturnsAsync(user);
            _signInManager.Setup(s => s.CheckPasswordSignInAsync(user, loginDto.Password, false))
                        .ReturnsAsync(IdentitySignInResult.Success);

            var config = ConfigurationHelper.LoadConfiguration(); // 🔥 Gerçek config yüklendi
            var controller = new UsersController(_userManager.Object, _signInManager.Object, config, _logger.Object);

            // Act
            var result = await controller.Login(loginDto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var tokenObj = (result as OkObjectResult)?.Value;
            tokenObj.Should().NotBeNull();
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenEmailNotFound()
        {
            var loginDto = new LoginDto { Email = "none@mail.com", Password = "123" };
            _userManager.Setup(u => u.FindByEmailAsync(loginDto.Email)).ReturnsAsync((AppUser?)null);

            var controller = new UsersController(_userManager.Object, _signInManager.Object, _config.Object, _logger.Object);
            var result = await controller.Login(loginDto);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenPasswordInvalid()
        {
            var user = new AppUser { UserName = "test", Email = "test@mail.com", Id = "123" };
            var loginDto = new LoginDto { Email = "test@mail.com", Password = "wrongpass" };

            _userManager.Setup(u => u.FindByEmailAsync(loginDto.Email)).ReturnsAsync(user);
            _signInManager.Setup(s => s.CheckPasswordSignInAsync(user, loginDto.Password, false))
                        .ReturnsAsync(IdentitySignInResult.Failed);

            var controller = new UsersController(_userManager.Object, _signInManager.Object, _config.Object, _logger.Object);
            var result = await controller.Login(loginDto);

            result.Should().BeOfType<UnauthorizedResult>();
        }
    }
}