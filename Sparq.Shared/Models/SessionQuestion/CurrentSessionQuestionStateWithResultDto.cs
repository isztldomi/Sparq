using Sparq.Shared.Models.QuestionDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.SessionQuestion
{
    public class CurrentSessionQuestionStateWithResultDto
    {
        public string Id { get; set; } = string.Empty;
        public CurrentQuestionWithResultDto? Question { get; set; }
        public int Order { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public bool IsActive { get; set; }
    }
}
