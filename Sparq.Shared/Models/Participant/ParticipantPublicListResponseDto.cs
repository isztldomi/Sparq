using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.Participant
{
    public class ParticipantPublicListResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public int Score { get; set; }
        public int Rank { get; set; }
    }
}
