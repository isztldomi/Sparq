using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.Shared.Models.AnswerDto
{
    public class SessionQuestionAnswersResponseDto
    {
        public string SessionId { get; set; } = null!;
        public string QuestionId { get; set; } = null!;
        public List<ParticipantAnswerDto> Answers { get; set; } = new();
    }
}
