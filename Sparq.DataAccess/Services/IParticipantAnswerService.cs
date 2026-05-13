using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface IParticipantAnswerService
    {
        Task<ParticipantAnswer> CreateAsync(ParticipantAnswer participantAnswer);
        Task<ParticipantAnswer?> GetByIdAsync(string id);
        Task<IReadOnlyCollection<ParticipantAnswer>> GetAllAsync();
        Task<bool> DeleteAsync(string id);

        Task<IReadOnlyCollection<ParticipantAnswer>> GetByParticipantIdAsync(string participantId);
        Task<IReadOnlyCollection<ParticipantAnswer>> GetByQuestionIdAsync(string questionId);
        Task<ParticipantAnswer?> GetParticipantAnswerAsync(string sessionId, string questionId, string? userId, string? extUserId);
        Task<IReadOnlyCollection<ParticipantAnswer>> GetBySessionAndQuestionAsync(string sessionId, string questionId);
        Task<IReadOnlyCollection<ParticipantAnswer>> GetBySessionAsync(string sessionId);
    }
}
