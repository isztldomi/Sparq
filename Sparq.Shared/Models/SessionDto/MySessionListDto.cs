using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.SessionDto
{
    public class MySessionListDto
    {
        public string SnapshotTitle { get; set; } = string.Empty;
        public string SessionId { get; set; } = string.Empty;
        public DateTime StartedAt { get; set; }
    }
}
