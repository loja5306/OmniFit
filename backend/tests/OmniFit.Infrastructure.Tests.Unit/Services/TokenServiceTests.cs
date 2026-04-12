using System.Security.Claims;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using OmniFit.Infrastructure.Services;

namespace OmniFit.Infrastructure.Tests.Unit.Services
{
    public class TokenServiceTests
    {
        private readonly TokenService _sut;
        private readonly IConfiguration _config;

        public TokenServiceTests()
        {
            _config = new ConfigurationBuilder()
                .AddInMemoryCollection(new List<KeyValuePair<string, string?>>
                {
                    new KeyValuePair<string, string?>("Jwt:Key", "Zu9vO06qz1LLp9Ly8hdgvnkU1FEquGJTB56Ang13KNu8Hulroc31rqA4Wle7TCnS"),
                    new KeyValuePair<string, string?>("Jwt:Issuer", "omnifit_api" ),
                    new KeyValuePair<string, string?>("Jwt:Audience", "omnifit_web")
                }).Build();

            _sut = new TokenService(_config);
        }

        [Fact]
        public void CreateToken_ShouldReturnValidToken_WithCorrectClaims()
        {
            //Arrange
            var email = "lukeatkinson@gmail.com";
            var userId = Guid.NewGuid().ToString();

            //Act
            var token = _sut.CreateToken(email, userId);

            //Assert
            var handler = new JsonWebTokenHandler();
            var jwtToken = handler.ReadJsonWebToken(token);

            jwtToken.GetClaim(JwtRegisteredClaimNames.NameId).Value.Should().Be(userId);
            jwtToken.GetClaim(JwtRegisteredClaimNames.Email).Value.Should().Be(email);
            jwtToken.Issuer.Should().Be(_config["Jwt:Issuer"]);
            jwtToken.Audiences.Should().Contain(_config["Jwt:Audience"]);
            jwtToken.ValidTo.Should().BeAfter(DateTime.UtcNow.AddDays(6));
        }
    }
}
