using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface IParticipantService
    {
        Task<Participant> CreateAsync(Participant participant);
        Task<Participant?> GetByIdAsync(string id);
        Task<IReadOnlyCollection<Participant>> GetAllAsync();
        Task<Participant?> UpdateAsync(string id, Participant updatedParticipant);
        Task<bool> DeleteAsync(string id);
        Task<IReadOnlyCollection<Participant>> GetBySessionIdAsync(string sessionId); 
        Task<bool> IsUserJoinedAsync(string userId, string sessionId);
        Task<bool> IsExtUserJoinedAsync(string extUserId, string sessionId);
        Task<Participant?> GetIdByUserIdAndSessionIdAsync(string userId, string sessionId);
        Task<Participant?> GetIdByExtUserIdAndSessionIdAsync(string extUserId, string sessionId);
        Task<IReadOnlyCollection<Participant>> GetAllParticipantsBySessionIdAsync(string sessionId);
    }
}
