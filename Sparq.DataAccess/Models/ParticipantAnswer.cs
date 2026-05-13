using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class ParticipantAnswer
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? SessionId { get; set; }
        [ForeignKey("SessionId")]
        public virtual Session? Session { get; set; }
        public string? ParticipantId { get; set; }
        [ForeignKey("ParticipantId")]
        public virtual Participant? Participant { get; set; }
        public string? QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; }
        public string? AnswerId { get; set; }
        [ForeignKey("AnswerId")]
        public virtual Answer? Answer { get; set; }
        public DateTime AnsweredAt { get; set; }
        public bool IsCorrect { get; set; }
        public int PointsEarned { get; set; }
    }
}
