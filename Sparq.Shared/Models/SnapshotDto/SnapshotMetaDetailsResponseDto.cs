using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.SnapshotDto
{
    public class SnapshotMetaDetailsResponseDto
    {
        public int Id { get; set; }
        public int QuizId { get; set; }
        public int SnapshotNumber { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? CreatedAt { get; set; }
    }
}
