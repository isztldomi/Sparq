using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    internal class MessageService : IMessageService
    {
        private readonly SparqDbContext _context;

        public MessageService(SparqDbContext context)
        {
            _context = context;
        }

        public async Task<Message> CreateAsync(Message message)
        {
            message.SentAt = DateTime.UtcNow;

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return message;
        }

        public async Task<Message?> GetByIdAsync(string id)
        {
            return await _context.Messages
                .Include(m => m.Session)
                .Include(m => m.Participant)
                .Include(m => m.Question)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IReadOnlyCollection<Message>> GetAllAsync()
        {
            return await _context.Messages
                .Include(m => m.Session)
                .Include(m => m.Participant)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var message = await _context.Messages.FindAsync(id);

            if (message == null)
                return false;

            _context.Messages.Remove(message);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IReadOnlyCollection<Message>> GetBySessionIdAsync(string sessionId)
        {
            return await _context.Messages
                .Where(m => m.SessionId == sessionId)
                .Include(m => m.Participant)
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<Message>> GetByParticipantIdAsync(string participantId)
        {
            return await _context.Messages
                .Where(m => m.ParticipantId == participantId)
                .Include(m => m.Session)
                .OrderByDescending(m => m.SentAt)
                .ToListAsync();
        }
    }
}
