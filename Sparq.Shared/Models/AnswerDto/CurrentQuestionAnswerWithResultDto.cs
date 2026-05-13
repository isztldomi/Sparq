using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.AnswerDto
{
    public class CurrentQuestionAnswerWithResultDto
    {
        public required string Id { get; set; }
        public required string Text { get; set; }
        public bool IsCorrect { get; set; }
        public int Order { get; set; }
    }
}
