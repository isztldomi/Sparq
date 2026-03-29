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
        public required int SessionId { get; set; }
        [ForeignKey("SessionId")]
        public required Session Session { get; set; }
        public required int ParticipantId { get; set; }
        [ForeignKey("ParticipantId")]
        public required Participant Participant { get; set; }
        public int? QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public Question? Question { get; set; }
        public required string Text { get; set; }
        public DateTime SentAt { get; set; }
    }
}
