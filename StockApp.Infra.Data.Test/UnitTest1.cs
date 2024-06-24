using Microsoft.Extensions.Configuration;
using Moq;
using StockApp.Domain.Interfaces;
using StockApp.Infra.Data.Identity;
using StockApp.Domain.Entities;
using StockApp.Application.DTOs;
using StockApp.API.Controllers;
using Microsoft.AspNetCore.Mvc;

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

        [Fact]
        public async Task Login_ValidCredebtials_ReturnsToken()
        {
            var authServiceMock = new Mock<IAuthService>();
            var tokenController = new TokenController(authServiceMock.Object);

            authServiceMock.Setup(auth => auth.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new TokenResponseDto
                {
                    Token = "token",
                    Expiration = DateTime.UtcNow.AddMinutes(60)
                }
            );

            var userLoginDto = new UserLoginDto
            {
                Username = "TestUser",
                Password = "TestPassword"
            };

            var result = await tokenController.Login(userLoginDto) as OkObjectResult;

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Value);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsOk()
        {
            var userRepositoryMock = new Mock<IUserRepository>();
            var userController = new UserController(userRepositoryMock.Object);

            var userRegisterDto = new UserRegisterDto
            {
                Username = "TestUser",
                Password = "TestPassword",
                Role = "User"
            };

            var result = await userController.Register(userRegisterDto) as OkResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }
    }
}