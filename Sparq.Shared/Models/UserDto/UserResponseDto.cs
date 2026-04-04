using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sparq.Shared.Models.UserDto
{
    public class UserResponseDto
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string NickName { get; init; }
        public required string Email { get; init; }
    }
}
