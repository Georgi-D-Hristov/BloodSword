using System;
using System.Collections.Generic;
using System.Text;

namespace BloodSword.Application.DTOs
{
    public class RegisterModel
    {
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}
