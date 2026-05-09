using OmniFit.Application.DTOs;
using OmniFit.Application.DTOs.Auth;

namespace OmniFit.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> RegisterUserAsync(RegisterRequest request);
        Task<AuthResponse> LoginUserAsync(LoginRequest request);
    }
}
