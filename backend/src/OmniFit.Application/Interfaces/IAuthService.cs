using OmniFit.Application.DTOs;

namespace OmniFit.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterUserAsync(RegisterRequestDto request);
        Task<AuthResponseDto> LoginUserAsync(LoginRequestDto request);
    }
}
