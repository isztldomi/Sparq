using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sparq.Shared.Models.UserDto
{
    public class NickNameUpdateRequestDto
    {
        [Required]
        [StringLength(10, MinimumLength = 3, ErrorMessage = "Nickname must be between 3 and 10 characters")]
        public required string NickName { get; set; } = string.Empty;
    }
}
