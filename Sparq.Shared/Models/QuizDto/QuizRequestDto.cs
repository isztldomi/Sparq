using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sparq.Shared.Models.QuizDto
{
    public class QuizRequestDto
    {
        [Required]
        public bool IsPublic { get; set; }
    }
}
