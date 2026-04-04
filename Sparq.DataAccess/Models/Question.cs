using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class Question
    {
        [Key]
        public int Id { get; set; }
        public required int SnapshotId { get; set; }
        [ForeignKey("SnapshotId")]
        public virtual required Snapshot Snapshot { get; set; }
        public required string Title { get; set; }
        public required string Text { get; set; }
        public string? MediaUrl { get; set; }
        public int? TimeLimit { get; set; }
        public int Point { get; set; }
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public virtual ICollection<ParticipantAnswer> ParticipantAnswers { get; set; } = new List<ParticipantAnswer>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
