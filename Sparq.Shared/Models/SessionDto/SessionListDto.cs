using Sparq.Shared.Models.SnapshotDto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparq.Shared.Models.SessionDto
{
    public class SessionListDto
    {

        public int Id { get; set; }
        public int SnapshotId { get; set; }
        public SnapshotMetaDetailsResponseDto? Snapshot { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public int? CurrentQuestionId { get; set; }
        public string PinCode { get; set; } = string.Empty;
        public bool IsWaiting { get; set; }
        public bool IsRunning { get; set; }
    }
}
