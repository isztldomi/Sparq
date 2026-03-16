using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class AnswerOption
    {
        public int Id { get; set; }

        public int QuestionId { get; set; }
        public Question Question { get; set; }

        public string AnswerText { get; set; }

        public bool IsCorrect { get; set; }
    }
}
