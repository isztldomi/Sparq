using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class Participant
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string DisplayName { get; set; }

        public int QuizSessionId { get; set; }
        public QuizSession QuizSession { get; set; }

        public int Score { get; set; }

        public bool IsFinished { get; set; }

        public ICollection<ParticipantAnswer> Answers { get; set; }
    }
}
