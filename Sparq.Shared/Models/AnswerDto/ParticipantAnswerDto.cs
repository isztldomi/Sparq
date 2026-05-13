using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.AnswerDto
{
    public class ParticipantAnswerDto
    {
        public string ParticipantId { get; set; } = null!;
        public string? ExtUserId { get; set; }
        public string? UserId { get; set; }

        public string AnswerId { get; set; } = null!;
        public string AnswerText { get; set; } = null!;

        public bool IsCorrect { get; set; }
        public int PointsEarned { get; set; }

        public DateTime AnsweredAt { get; set; }
    }
}
