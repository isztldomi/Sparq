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
        public int Id { get; set; }
        public required int ParticipantId { get; set; }
        [ForeignKey("ParticipantId")]
        public virtual required Participant Participant { get; set; }
        public required int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual required Question Question { get; set; }
        public required int AnswerId { get; set; }
        [ForeignKey("AnswerId")]
        public virtual required Answer Answer { get; set; }
        public DateTime AnsweredAt { get; set; }
        public bool IsCorrect { get; set; }
        public int PointsEarned { get; set; }
    }
}
