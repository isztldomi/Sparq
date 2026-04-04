using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.QuizDto
{
    public class QuizResponseDto
    {
        public int Id { get; set; }

        public string? OwnerId { get; set; }

        public bool IsPublic { get; set; }

        public bool IsActive { get; set; }

        public int? LastSnapshotId { get; set; }

        public int SnapshotCount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}
