using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public int SessionId { get; set; }
        public Session Session { get; set; }

        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }

        public int? QuestionId { get; set; }
        public Question Question { get; set; }

        public string Text { get; set; }

        public DateTime SentAt { get; set; }
    }
}
