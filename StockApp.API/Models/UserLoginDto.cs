using System.ComponentModel.DataAnnotations;

namespace StockApp.API.Models
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Invalid Format Email Address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [StringLength(20, ErrorMessage = "Password must be between 10 and 20 characters", MinimumLength = 10)]
        public string Password { get; set; }
    }
}
