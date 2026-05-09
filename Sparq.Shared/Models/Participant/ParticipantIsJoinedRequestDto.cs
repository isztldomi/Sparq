using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.Participant
{
    public class ParticipantIsJoinedRequestDto
    {
        public required string ExternalUserId { get; set; }
    }
}
