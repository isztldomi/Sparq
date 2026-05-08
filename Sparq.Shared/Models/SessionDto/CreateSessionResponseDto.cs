using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.SessionDto
{
    public class CreateSessionResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string SnapshotId { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
        public string? PinCode { get; set; }
        public bool IsWaiting { get; set; }
        public bool IsRunning { get; set; }
    }
}
