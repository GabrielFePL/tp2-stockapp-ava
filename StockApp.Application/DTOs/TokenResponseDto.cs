using System.ComponentModel.DataAnnotations;

namespace StockApp.Application.DTOs
{
    public class TokenResponseDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
