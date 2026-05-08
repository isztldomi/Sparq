using Sparq.Shared.Models.AnswerDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.QuestionDto
{
    public class QuestionResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int Order { get; set; }
        public string? MediaId { get; set; }
        public int TimeLimit { get; set; }
        public int Point { get; set; }
        public List<AnswerResponseDto> Answers { get; set; } = new();
    }
}
