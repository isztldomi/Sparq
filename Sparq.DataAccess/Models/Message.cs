using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public int? SessionId { get; set; }
        [ForeignKey("SessionId")]
        public virtual Session? Session { get; set; }
        public int? ParticipantId { get; set; }
        [ForeignKey("ParticipantId")]
        public virtual Participant? Participant { get; set; }
        public int? QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; }
        public string? Text { get; set; }
        public DateTime SentAt { get; set; }
    }
}
