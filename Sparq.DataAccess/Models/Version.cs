using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class Version
    {
        public int Id { get; set; }

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

        public int VersionNumber { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public int TimeLimit { get; set; }
        public string PinCode { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<Question> Questions { get; set; }
        public ICollection<Session> Sessions { get; set; }
    }
}
