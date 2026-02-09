namespace OmniFit.Application.DTOs
{
    public record RegisterRequestDto(string Email, string Password);
    public record LoginRequestDto(string Email, string Password);
    public record AuthResponseDto(string Token);
}
