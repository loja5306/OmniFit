using FluentAssertions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using OmniFit.Application.DTOs;
using OmniFit.Application.Interfaces;
using OmniFit.Infrastructure.Services;
using System.Security.Authentication;

namespace OmniFit.Infrastructure.Tests.Unit.Services
{
    public class AuthServiceTests
    {
        private readonly AuthService _sut;

        private readonly ITokenService _tokenService;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthServiceTests()
        {
            _tokenService = Substitute.For<ITokenService>();

            var store = Substitute.For<IUserStore<IdentityUser>>();
            _userManager = Substitute.For<UserManager<IdentityUser>>(
                store, null, null, null, null, null, null, null, null);

            _sut = new AuthService(_userManager, _tokenService);
        }

        [Fact]
        public async Task LoginUserAsync_ShouldReturnToken_WhenUserIsValid()
        {
            //Arrange
            var request = new LoginRequestDto("lukeatkinson@gmail.com", "Password123!");
            var identityUser = new IdentityUser
            {
                Id = Guid.NewGuid().ToString(),
                UserName = request.Email,
                Email = request.Email,
            };
            var token = Guid.NewGuid().ToString();

            _userManager.FindByEmailAsync(request.Email).Returns(identityUser);
            _userManager.CheckPasswordAsync(identityUser, request.Password).Returns(true);
            _tokenService.CreateToken(request.Email, identityUser.Id).Returns(token);

            //Act
            var result = await _sut.LoginUserAsync(request);

            //Assert
            result.Token.Should().Be(token);
        }

        [Fact]
        public async Task LoginUserAsync_ShouldThrowException_WhenUserEmailIsInvalid()
        {
            //Arrange
            var request = new LoginRequestDto("lukeatkinson@gmail.com", "Password123!");

            _userManager.FindByEmailAsync(request.Email).Returns((IdentityUser)null!);

            //Act
            Func<Task> result = async () => await _sut.LoginUserAsync(request);

            //Assert
            await result.Should().ThrowAsync<AuthenticationException>()
                .WithMessage("The email and/or password was incorrect");
        }

        [Fact]
        public async Task LoginUserAsync_ShouldThrowException_WhenUserPasswordIsInvalid()
        {
            //Arrange
            var request = new LoginRequestDto("lukeatkinson@gmail.com", "Password123!");
            var identityUser = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
            };

            _userManager.FindByEmailAsync(request.Email).Returns(identityUser);
            _userManager.CheckPasswordAsync(identityUser, request.Password).Returns(false);

            //Act
            Func<Task> result = async () => await _sut.LoginUserAsync(request);

            //Assert
            await result.Should().ThrowAsync<AuthenticationException>()
                .WithMessage("The email and/or password was incorrect");
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldReturnToken_WhenUserIsValid()
        {
            //Arrange
            var request = new RegisterRequestDto("lukeatkinson@gmail.com", "Password123!");
            var token = Guid.NewGuid().ToString();

            _userManager.CreateAsync(Arg.Any<IdentityUser>(), request.Password)
                .Returns(IdentityResult.Success);
            _tokenService.CreateToken(request.Email, Arg.Any<string>()).Returns(token);

            //Act
            var result = await _sut.RegisterUserAsync(request);

            //Assert
            result.Token.Should().Be(token);
        }

        [Fact]
        public async Task RegisterUserAsync_ShouldThrowValidationException_WhenUserIsInvalid()
        {
            //Arrange
            var request = new RegisterRequestDto("lukeatkinson@gmail.com", "pass");
            var identityUser = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
            };
            var identityError = new IdentityError
            {
                Code = "PasswordTooShort",
                Description = "Password is too short"
            };

            _userManager.CreateAsync(Arg.Any<IdentityUser>(), request.Password)
                .Returns(IdentityResult.Failed(identityError));
            
            //Act
            Func<Task> result = async () => await _sut.RegisterUserAsync(request);

            //Assert
            await result.Should().ThrowAsync<ValidationException>()
                .Where(ex => ex.Errors.Any(e => e.ErrorMessage == identityError.Description));
        }
    }
}
