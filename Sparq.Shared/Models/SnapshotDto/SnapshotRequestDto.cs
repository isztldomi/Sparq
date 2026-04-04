using Sparq.Shared.Models.QuestionDto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.SnapshotDto
{
    public class SnapshotCreateRequestDto
    {
        public int QuizId { get; set; }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int TimeLimit { get; set; }

        public List<QuestionCreateDto> Questions { get; set; } = new();
    }
}
