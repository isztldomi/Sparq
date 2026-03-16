using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class QuizSession
    {
        public int Id { get; set; }

        public int QuizVersionId { get; set; }
        public QuizVersion QuizVersion { get; set; }

        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public int? CurrentQuestionId { get; set; }

        public string PinCode { get; set; }

        public bool IsWaiting { get; set; }
        public bool IsRunning { get; set; }

        public ICollection<Participant> Participants { get; set; }
    }
}
