using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sparq.Shared.Models.SessionDto
{
    public class CreateSessionRequestDto
    {
        [Required]
        public string QuizId { get; set; } = string.Empty;
    }
}
