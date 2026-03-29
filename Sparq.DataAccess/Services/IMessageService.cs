using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface IMessageService
    {
        Task<Message> CreateAsync(Message message);
        Task<Message?> GetByIdAsync(int id);
        Task<IReadOnlyCollection<Message>> GetAllAsync();
        Task<bool> DeleteAsync(int id);

        Task<IReadOnlyCollection<Message>> GetBySessionIdAsync(int sessionId);
        Task<IReadOnlyCollection<Message>> GetByParticipantIdAsync(int participantId);
    }
}
