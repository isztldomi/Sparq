using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.SnapshotDto
{
    public class SnapshotResponseDto
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int SnapshotNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TimeLimit { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
