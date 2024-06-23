using Microsoft.AspNetCore.Mvc;
using StockApp.Application.DTOs;
using StockApp.Infra.Data.Identity;

namespace StockApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly IAuthService _authService;

        public TokenController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
        {
            var token = await _authService.AuthenticateAsync(userLoginDto.Username, userLoginDto.Password);
            if (token == null)
            {
                return Unauthorized();
            }
            return Ok(token);
        }

    }
}
