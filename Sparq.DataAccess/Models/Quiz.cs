using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class Quiz
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public virtual User? Owner { get; set; }
        public bool IsPublic { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
        public string? LastSnapshotId { get; set; }
        [ForeignKey("LastSnapshotId")]
        public virtual Snapshot? LastSnapshot { get; set; }
        public virtual ICollection<Snapshot> Snapshots { get; set; } = new List<Snapshot>();
    }
}
