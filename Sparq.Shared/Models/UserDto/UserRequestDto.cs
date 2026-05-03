using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sparq.Shared.Models.UserDto
{
    public class UserRequestDto
    {
        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "First name must be between 2 and 255 characters")]
        public required string FirstName { get; init; }

        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Last name must be between 2 and 255 characters")]
        public required string LastName { get; init; }

        [Required]
        [StringLength(255, MinimumLength = 3, ErrorMessage = "Nickname must be between 2 and 255 characters")]
        public required string NickName { get; init; }

        [Required]
        [EmailAddress(ErrorMessage = "Email is invalid")]
        public required string Email { get; init; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        public required string Password { get; init; }
    }
}