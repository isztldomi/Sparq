using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.SessionDto
{
    public class QuitSessionRequestDto
    {
        public required string SessionId { get; set; }
        public string? ExternalUserId { get; set; }

    }
}
