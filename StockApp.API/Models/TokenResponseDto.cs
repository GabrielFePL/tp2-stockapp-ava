using System.ComponentModel.DataAnnotations;

namespace StockApp.API.Models
{
    public class TokenResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
