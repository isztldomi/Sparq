using Microsoft.AspNetCore.Mvc;
using Sparq.DataAccess.Models;
using Sparq.DataAccess.Services;
using Sparq.Shared.Models.AnswerDto;
using Sparq.Shared.Models.SessionDto;

namespace Sparq.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswerController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly IParticipantService _participantService;
        private readonly ISessionQuestionStateService _sessionQuestionStateService;
        private readonly IQuestionService _questionService;
        private readonly IAnswerService _answerService;
        private readonly IParticipantAnswerService _participantAnswerService;

        public AnswerController(
            IUsersService usersService,
            IParticipantService participantService,
            ISessionQuestionStateService sessionQuestionStateService,
            IQuestionService questionService,
            IAnswerService answerService,
            IParticipantAnswerService participantAnswerService)
        {
            _usersService = usersService;
            _participantService = participantService;
            _sessionQuestionStateService = sessionQuestionStateService;
            _questionService = questionService;
            _answerService = answerService;
            _participantAnswerService = participantAnswerService;
        }
        [HttpPost("submit")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(bool))]
        public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerRequestDto dto)
        {
            var user = await _usersService.GetCurrentUserAsync();

            var state = await _sessionQuestionStateService
                .GetActiveBySessionIdAsync(dto.SessionId);

            if (state == null)
                return NotFound();

            if (state.EndsAt != null && state.EndsAt < DateTime.UtcNow)
                return Forbid("Question expired");

            Participant participant;

            if (user == null)
            {
                if (string.IsNullOrWhiteSpace(dto.ExtUserId))
                    return Forbid();

                participant = (await _participantService.GetIdByExtUserIdAndSessionIdAsync(dto.ExtUserId, dto.SessionId))!;
            }
            else
            {
                participant = (await _participantService.GetIdByUserIdAndSessionIdAsync(user.Id, dto.SessionId))!;
            }

            if (participant == null)
                return Forbid();

            var answer = await _answerService.GetByIdAsync(dto.AnswerId);

            if (answer == null || answer.QuestionId != dto.QuestionId)
                return BadRequest("Invalid answer");

            var existing = await _participantAnswerService.GetParticipantAnswerAsync(
                dto.SessionId,
                dto.QuestionId,
                user?.Id,
                dto.ExtUserId);

            if (existing != null)
                return Conflict("Already answered");

            var isCorrect = answer.IsCorrect;

            var question = await _questionService.GetByIdAsync(dto.QuestionId);
            var points = isCorrect ? question?.Point ?? 0 : 0;

            var participantAnswer = new ParticipantAnswer
            {
                Id = Guid.NewGuid().ToString(),
                SessionId = dto.SessionId,
                ParticipantId = participant.Id,
                QuestionId = dto.QuestionId,
                AnswerId = dto.AnswerId,
                AnsweredAt = DateTime.UtcNow,
                IsCorrect = isCorrect,
                PointsEarned = points
            };

            await _participantAnswerService.CreateAsync(participantAnswer);

            return Ok(true);
        }

        [HttpGet("session/{sessionId}/question/{questionId}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SessionQuestionAnswersResponseDto))]
        public async Task<IActionResult> GetSessionQuestionAnswers(string sessionId, string questionId, string? extUserId)
        {
            var user = await _usersService.GetCurrentUserAsync();

            var state = await _sessionQuestionStateService
                .GetActiveBySessionIdAsync(sessionId);

            if (state == null)
                return NotFound();

            Participant? participant;

            if (user == null)
            {
                if (string.IsNullOrWhiteSpace(extUserId))
                    return Forbid();

                participant = await _participantService
                    .GetIdByExtUserIdAndSessionIdAsync(extUserId, sessionId);
            }
            else
            {
                participant = await _participantService
                    .GetIdByUserIdAndSessionIdAsync(user.Id, sessionId);
            }

            if (participant == null)
                return Forbid();

            var participantAnswers =
                await _participantAnswerService
                    .GetBySessionAndQuestionAsync(sessionId, questionId);

            participantAnswers = participantAnswers
                .Where(pa => pa.ParticipantId == participant.Id)
                .ToList();

            var answerIds = participantAnswers
                .Select(x => x.AnswerId)
                .Distinct()
                .ToList();

            var answers = await _answerService
                .GetByIdsAsync(answerIds);

            var result = participantAnswers.Select(pa =>
            {
                var answer = answers.FirstOrDefault(a => a.Id == pa.AnswerId);

                return new ParticipantAnswerDto
                {
                    ParticipantId = pa.ParticipantId,
                    UserId = pa.Participant?.UserId,
                    ExtUserId = pa.Participant?.ExternalUserId,

                    AnswerId = pa.AnswerId,
                    AnswerText = answer?.Text ?? string.Empty,

                    IsCorrect = pa.IsCorrect,
                    PointsEarned = pa.PointsEarned,
                    AnsweredAt = pa.AnsweredAt
                };
            }).ToList();

            return Ok(new SessionQuestionAnswersResponseDto
            {
                SessionId = sessionId,
                QuestionId = questionId,
                Answers = result
            });
        }

        
    }
}
