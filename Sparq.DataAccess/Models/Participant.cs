using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class Participant
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual User? User { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string? SessionId { get; set; } 
        [ForeignKey("SessionId")]
        public virtual Session? Session { get; set; }
        public int Score { get; set; }
        public int Rank { get; set; }
        public bool IsFinished { get; set; }
        public virtual ICollection<ParticipantAnswer> ParticipantAnswers { get; set; } = new List<ParticipantAnswer>();
        public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
    }
}
