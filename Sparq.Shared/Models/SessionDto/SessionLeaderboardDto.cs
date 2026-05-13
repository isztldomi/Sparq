using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.SessionDto
{
    public class SessionLeaderboardDto
    {
        public string SessionId { get; set; } = default!;
        public List<LeaderboardEntryDto> Entries { get; set; } = new();
    }
}
