using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class Session
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? SnapshotId { get; set; }
        [ForeignKey("SnapshotId")]
        public virtual Snapshot? Snapshot { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
        public int? CurrentQuestionId { get; set; }
        public string? PinCode { get; set; }
        public bool IsWaiting { get; set; }
        public bool IsRunning { get; set; }
        public virtual ICollection<Participant> Participants { get; set; } = new List<Participant>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
