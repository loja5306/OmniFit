using FluentValidation;
using Microsoft.AspNetCore.Identity;
using OmniFit.Application.DTOs;
using OmniFit.Application.Interfaces;
using System.Security.Authentication;

namespace OmniFit.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<IdentityUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> LoginUserAsync(LoginRequestDto request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user == null)
            {
                throw new AuthenticationException("The email and/or password was incorrect");
            }

            bool validPassword = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!validPassword)
            {
                throw new AuthenticationException("The email and/or password was incorrect");
            }

            string token = _tokenService.CreateToken(request.Email, user.Id);

            return new AuthResponseDto(token);
        }

        public async Task<AuthResponseDto> RegisterUserAsync(RegisterRequestDto request)
        {
            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email,
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                throw new ValidationException(result.Errors.Select(e =>
                    new FluentValidation.Results.ValidationFailure("Registration", e.Description)));
            }

            string token = _tokenService.CreateToken(request.Email, user.Id);

            return new AuthResponseDto(token);
        }
    }
}
