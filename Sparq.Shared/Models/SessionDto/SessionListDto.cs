using Sparq.Shared.Models.SnapshotDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparq.Shared.Models.SessionDto
{
    public class SessionListDto
    {
        public string Id { get; set; } = string.Empty;
        public string SnapshotId { get; set; } = string.Empty;
        public SnapshotMetaDetailsResponseDto? Snapshot { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public string? CurrentQuestionId { get; set; }
        public string PinCode { get; set; } = string.Empty;
        public SessionStatus Status { get; set; }
    }
}
