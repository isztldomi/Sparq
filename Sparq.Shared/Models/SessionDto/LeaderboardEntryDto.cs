using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.SessionDto
{
    public class LeaderboardEntryDto
    {
        public string ParticipantId { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string? UserId { get; set; }
        public string? ExtUserId { get; set; }
        public int TotalPoints { get; set; }
        public int CorrectAnswers { get; set; }
    }
}
