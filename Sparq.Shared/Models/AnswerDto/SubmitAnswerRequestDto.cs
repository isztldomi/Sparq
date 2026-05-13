using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.AnswerDto
{
    public class SubmitAnswerRequestDto
    {
        public string SessionId { get; set; } = string.Empty;
        public string QuestionId { get; set; } = string.Empty;
        public string AnswerId { get; set; } = string.Empty;
        public string? ExtUserId { get; set; }
    }
}
