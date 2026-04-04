using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class Snapshot
    {
        [Key]
        public int Id { get; set; }
        public int? QuizId { get; set; }
        [ForeignKey("QuizId")]
        public virtual Quiz? Quiz { get; set; }
        public int SnapshotNumber { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int TimeLimit { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Session> Sessions { get; set; } = new List<Session>();
        public virtual ICollection<Question> Questions { get; set; } = new List<Question>();
    }
}
