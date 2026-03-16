using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class Question
    {
        public int Id { get; set; }

        public int QuizVersionId { get; set; }
        public QuizVersion QuizVersion { get; set; }

        public string QuestionTitle { get; set; }
        public string QuestionText { get; set; }

        public string? QuestionMediaUrl { get; set; }

        public int QuestionTimeLimit { get; set; }
        public int QuestionPoint { get; set; }

        public ICollection<AnswerOption> AnswerOptions { get; set; }
    }
}
