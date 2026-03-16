using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class ParticipantAnswer
    {
        public int Id { get; set; }

        public int ParticipantId { get; set; }
        public Participant Participant { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public int AnswerOptionId { get; set; }
        public AnswerOption AnswerOption { get; set; }

        public DateTime AnsweredAt { get; set; }

        public bool IsCorrect { get; set; }

        public int PointsAwarded { get; set; }
    }
}
