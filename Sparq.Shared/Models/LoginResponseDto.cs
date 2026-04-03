using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models
{
    public class LoginResponseDto
    {
        public required string UserId { get; init; }
        public required string AuthToken { get; init; }
        public required string RefreshToken { get; init; }
    }
}
