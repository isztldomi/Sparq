using Sparq.Shared.Models.AnswerDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.QuestionDto
{
    public class QuestionCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string? MediaUrl { get; set; }

        public int Point { get; set; }

        public List<AnswerCreateDto> Answers { get; set; } = new();
    }
}
