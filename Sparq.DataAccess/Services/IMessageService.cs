using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface IMessageService
    {
        Task<Message> CreateAsync(Message message);
        Task<Message?> GetByIdAsync(string id);
        Task<IReadOnlyCollection<Message>> GetAllAsync();
        Task<bool> DeleteAsync(string id);

        Task<IReadOnlyCollection<Message>> GetBySessionIdAsync(string sessionId);
        Task<IReadOnlyCollection<Message>> GetByParticipantIdAsync(string participantId);
    }
}
