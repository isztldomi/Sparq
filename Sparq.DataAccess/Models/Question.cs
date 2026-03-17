using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class Question
    {
        public int Id { get; set; }

        public int VersionId { get; set; }
        public Version Version { get; set; }

        public string Title { get; set; }
        public string Text { get; set; }

        public string? MediaUrl { get; set; }

        public int TimeLimit { get; set; }
        public int Point { get; set; }

        public ICollection<Answer> Answers { get; set; }
    }
}
