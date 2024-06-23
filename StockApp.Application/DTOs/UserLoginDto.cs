using System.ComponentModel.DataAnnotations;

namespace StockApp.Application.DTOs
{
    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
