using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sparq.Shared.Models.LoginDto
{
    public class LoginRequestDto
    {
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}
