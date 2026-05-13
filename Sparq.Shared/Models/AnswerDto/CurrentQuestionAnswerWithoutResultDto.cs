using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.AnswerDto
{
    public class CurrentQuestionAnswerWithoutResultDto
    {
        public required string Id { get; set; }
        public required string Text { get; set; }
        public int Order { get; set; }
    }
}
