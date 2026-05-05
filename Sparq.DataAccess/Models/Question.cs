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
        public int SnapshotId { get; set; }
        [ForeignKey("SnapshotId")]
        public virtual Snapshot? Snapshot { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public int? MediaId { get; set; }
        [ForeignKey("MediaId")]
        public virtual Media? Media { get; set; }
        public int? TimeLimit { get; set; }
        public int Point { get; set; }
        public virtual ICollection<Answer> Answers { get; set; } = new List<Answer>();
        public virtual ICollection<ParticipantAnswer> ParticipantAnswers { get; set; } = new List<ParticipantAnswer>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
