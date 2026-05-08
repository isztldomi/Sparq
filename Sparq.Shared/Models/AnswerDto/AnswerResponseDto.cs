using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.AnswerDto
{
    public class AnswerResponseDto
    {
        public required string Id { get; set; }
        public required string Text { get; set; }
        public required bool IsCorrect { get; set; }
        public int Order { get; set; }
    }
}
