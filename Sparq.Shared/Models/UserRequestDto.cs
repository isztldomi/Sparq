using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sparq.Shared.Models
{
    public class UserRequestDto
    {
        [StringLength(255, ErrorMessage = "Name is too long")]
        public required string FirstName { get; init; }
        [StringLength(255, ErrorMessage = "Name is too long")]
        public required string LastName { get; init; }
        [StringLength(255, ErrorMessage = "Name is too long")]
        public required string NickName { get; init; }
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public required string Email { get; init; }
        public required string Password { get; init; }
    }
}
