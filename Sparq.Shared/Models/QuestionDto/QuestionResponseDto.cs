using Sparq.Shared.Models.AnswerDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.QuestionDto
{
    public class QuestionResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public int? MediaId{ get; set; } 
        public int Point { get; set; }
        public List<AnswerResponseDto> Answers { get; set; } = new();
    }
}
