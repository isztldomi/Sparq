using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.SessionDto
{
    public class JoinSessionRequestDto
    {
        public required string SessionId { get; set; }
        public required string PinCode { get; set; }
        public required string Nickname { get; set; }
    }
}
