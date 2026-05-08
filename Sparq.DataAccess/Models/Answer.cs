using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class Answer
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Question? Question { get; set; }
        public int Order { get; set; }
        public string? Text { get; set; }
        public bool IsCorrect { get; set; }
        public virtual ICollection<ParticipantAnswer> ParticipantAnswers { get; set; } = new List<ParticipantAnswer>();
    }
}
