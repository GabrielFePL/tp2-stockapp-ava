using Microsoft.AspNetCore.Mvc.Testing;
using ikvm.runtime;
using javax.management.relation;
using StockApp.Application.DTOs;
using System.Net.Http.Json;

namespace StockApp.Infra.Data.Test
{
    public class IntegrationTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        public IntegrationTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task RegisterAndLogin_ValidCredentials_ReturnsToken()
        {
            var userRegisterDto = new UserRegisterDto
            {
                Username = "integration_test",
                Password = "integration_test",
                Role = "User"
            };

            var userLoginDto = new UserLoginDto
            {
                Username = "integration_test",
                Password = "integration_test"
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/auth/register", userRegisterDto);
            registerResponse.EnsureSuccessStatusCode();

            var loginResponse = await _client.PostAsJsonAsync("/api/auth/login", userLoginDto);
            loginResponse.EnsureSuccessStatusCode();

            var tokenResponse = await loginResponse.Content.ReadFromJsonAsync<TokenResponseDto>();

            Assert.NotNull(tokenResponse);
            Assert.NotNull(tokenResponse.Token);
            Assert.True(tokenResponse.Expiration > DateTime.UtcNow);
        }
    }
}
