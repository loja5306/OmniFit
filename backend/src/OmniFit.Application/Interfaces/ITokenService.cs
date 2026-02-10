namespace OmniFit.Application.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(string email, string userId);
    }
}
