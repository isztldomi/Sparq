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
    }
}
