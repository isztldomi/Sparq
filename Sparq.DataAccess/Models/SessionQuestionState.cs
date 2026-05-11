using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Sparq.DataAccess.Models
{
    public class SessionQuestionState
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? SessionId { get; set; }
        [ForeignKey("SessionId")]
        public Session? Session { get; set; }
        public string? QuestionId { get; set; }
        [ForeignKey(nameof(QuestionId))]
        public Question? Question { get; set; }
        public int Order { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndsAt { get; set; }
        public bool IsActive { get; set; }
    }
}
