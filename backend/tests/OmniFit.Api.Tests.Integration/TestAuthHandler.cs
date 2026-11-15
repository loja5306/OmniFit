using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace OmniFit.Api.Tests.Integration
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public const string AuthenticationScheme = "TestScheme";
        public const string UserIdHeader = "Test-UserId";
        public const string RolesHeader = "Test-Roles";

        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {

            if (!Context.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                return Task.FromResult(AuthenticateResult.Fail("No Authorization header found."));
            }

            var userId = Context.Request.Headers.TryGetValue(UserIdHeader, out var id) 
                ? id.ToString() : 
                TestData.Users.StandardUser.Id;

            var roles = Context.Request.Headers.TryGetValue(RolesHeader, out var userRoles) 
                ? userRoles.ToString().Split(',') : 
                [];

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.NameId, userId),
            };
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var identity = new ClaimsIdentity(claims, AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
