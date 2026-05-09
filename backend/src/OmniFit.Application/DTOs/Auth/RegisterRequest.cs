namespace OmniFit.Application.DTOs.Auth
{
    public record RegisterRequest(
        string Email, 
        string Password
    );
}
