using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface IParticipantService
    {
        Task<Participant> CreateAsync(Participant participant);
        Task<Participant?> GetByIdAsync(int id);
        Task<IReadOnlyCollection<Participant>> GetAllAsync();
        Task<Participant?> UpdateAsync(int id, Participant updatedParticipant);
        Task<bool> DeleteAsync(int id);

        Task<IReadOnlyCollection<Participant>> GetBySessionIdAsync(int sessionId);
    }
}
