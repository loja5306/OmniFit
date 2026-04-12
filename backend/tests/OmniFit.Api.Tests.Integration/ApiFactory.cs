using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Npgsql;
using OmniFit.Infrastructure.Data;
using Respawn;
using System.Data.Common;
using System.Net.Http.Headers;
using Testcontainers.PostgreSql;

namespace OmniFit.Api.Tests.Integration
{
    public class ApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _dbContainer;
        private DbConnection _dbConnection = default!;
        private Respawner _respawner = default!;

        private const string JwtTestKey = "Bd7rL1unRzLMP02R8vKrxmh30G2LBiHMm8pJNA1rJR5";
        private const string JwtIssuer = "omnifit_api";
        private const string JwtAudience = "omnifit_web";

        public ApiFactory()
        {
            _dbContainer = new PostgreSqlBuilder("postgres:16-alpine")
                .WithUsername("postgres")
                .WithPassword("7Er2zgu526lCAN9")
                .WithDatabase("omnifit")
                .Build();
        }

        public HttpClient HttpClient { get; private set; } = default!;

        public async Task ResetDatabaseAsync()
        {
            await _respawner.ResetAsync(_dbConnection);
        }

        public async Task InitializeAsync()
        {
            await _dbContainer.StartAsync();
            _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
            HttpClient = CreateClient();
            HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Guid.NewGuid().ToString());
            await InitializeRespawner();
        }

        private async Task InitializeRespawner()
        {
            await _dbConnection.OpenAsync();
            _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions
            {
                DbAdapter = DbAdapter.Postgres,
                SchemasToInclude = new[] { "public" }
            });
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((config) =>
            {
                config.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"] = JwtTestKey,
                    ["Jwt:Issuer"] = JwtIssuer,
                    ["Jwt:Audience"] = JwtAudience
                });
            });
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(AppDbContext));
                
                services.AddDbContext<AppDbContext>(options =>
                    options.UseNpgsql(_dbConnection));

                services.RemoveAll(typeof(AuthenticationOptions));
                services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = TestAuthHandler.AuthenticationScheme;
                    options.DefaultChallengeScheme = TestAuthHandler.AuthenticationScheme;
                })
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(TestAuthHandler.AuthenticationScheme, options => { });
            });
        }

        async Task IAsyncLifetime.DisposeAsync()
        {
            await _dbContainer.DisposeAsync();
        }
    }
}
