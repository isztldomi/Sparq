using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.AnswerDto
{
    public class AnswerResponseDto
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsCorrect { get; set; }
    }
}
