using Sparq.Shared.Models.AnswerDto;
using System;
using System.Collections.Generic;
using System.Text;
using Sparq.Shared.Models.QuestionDto;

namespace Sparq.Shared.Models.SessionQuestion
{
    public class CurrentSessionQuestionStateWithoutResultDto
    {
        public string Id { get; set; } = string.Empty;
        public CurrentQuestionWithoutResultDto? Question { get; set; }
        public int Order { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public bool IsActive { get; set; }
    }
}
