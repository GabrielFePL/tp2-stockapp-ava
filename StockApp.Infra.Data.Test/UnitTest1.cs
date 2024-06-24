using Microsoft.Extensions.Configuration;
using Moq;
using StockApp.Domain.Interfaces;
using StockApp.Infra.Data.Identity;
using StockApp.Domain.Entities;
using StockApp.Application.DTOs;

namespace StockApp.Infra.Data.Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task AuthenticateAsync_ValidCredentials_ReturnsToken()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var configurationMock = new Mock<IConfiguration>();
            var authService = new AuthService(userRepositoryMock.Object, configurationMock.Object);

            userRepositoryMock.Setup(repo => repo.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new User { 
                    Username = "TestUser",
                    Password = BCrypt.Net.BCrypt.HashPassword("TestPassword"),
                    Role = "User"
                }
            );

            configurationMock.Setup(config => config["Jwt:Key"]).Returns("StockAppAva2");
            configurationMock.Setup(config => config["Jwt:Issuer"]).Returns("StockAppAva");
            configurationMock.Setup(config => config["JWT: Audience"]).Returns("https://localhost:7189/swagger/index.html");

            var result = await authService.AuthenticateAsync("TestUser", "TestPassword");

            Assert.NotNull(result);
            Assert.IsType<TokenResponseDto>(result);
        }
    }
}