﻿using System.ComponentModel.DataAnnotations;

namespace StockApp.API.Models
{
    public class UserLoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
