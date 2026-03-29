using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface IParticipantAnswerService
    {
        Task<ParticipantAnswer> CreateAsync(ParticipantAnswer participantAnswer);
        Task<ParticipantAnswer?> GetByIdAsync(int id);
        Task<IReadOnlyCollection<ParticipantAnswer>> GetAllAsync();
        Task<bool> DeleteAsync(int id);

        Task<IReadOnlyCollection<ParticipantAnswer>> GetByParticipantIdAsync(int participantId);
        Task<IReadOnlyCollection<ParticipantAnswer>> GetByQuestionIdAsync(int questionId);
    }
}
