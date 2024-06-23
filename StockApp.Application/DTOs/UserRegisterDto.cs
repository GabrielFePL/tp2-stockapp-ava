using System.ComponentModel.DataAnnotations;

namespace StockApp.Application.DTOs
{
    public class UserRegisterDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
