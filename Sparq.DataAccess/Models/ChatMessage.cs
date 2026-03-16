using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class ChatMessage
    {
        public int Id { get; set; }

        public int QuizSessionId { get; set; }
        public QuizSession QuizSession { get; set; }

        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }

        public int? QuestionId { get; set; }
        public Question Question { get; set; }

        public string MessageText { get; set; }

        public DateTime SentAt { get; set; }
    }
}
