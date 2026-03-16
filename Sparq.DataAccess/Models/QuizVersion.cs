using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class QuizVersion
    {
        public int Id { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public int VersionNumber { get; set; }

        public string QuizTitle { get; set; }
        public string Description { get; set; }

        public int TimeLimit { get; set; }
        public string PinCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
