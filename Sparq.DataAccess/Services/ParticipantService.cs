using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    internal class ParticipantService : IParticipantService
    {
        private readonly SparqDbContext _context;

        public ParticipantService(SparqDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<Participant> CreateAsync(Participant participant)
        {
            participant.Score = 0;
            participant.Rank = 0;
            participant.IsFinished = false;

            _context.Participants.Add(participant);
            await _context.SaveChangesAsync();

            return participant;
        }

        // READ by id
        public async Task<Participant?> GetByIdAsync(string id)
        {
            return await _context.Participants
                .Include(p => p.User)
                .Include(p => p.Session)
                .Include(p => p.ParticipantAnswers)
                .Include(p => p.Messages)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        // READ all
        public async Task<IReadOnlyCollection<Participant>> GetAllAsync()
        {
            return await _context.Participants
                .Include(p => p.Session)
                .ToListAsync();
        }

        // UPDATE
        public async Task<Participant?> UpdateAsync(string id, Participant updatedParticipant)
        {
            var existing = await _context.Participants
                .Include(p => p.ParticipantAnswers)
                .Include(p => p.Messages)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (existing == null)
                return null;

            // identity / kapcsolat
            existing.UserId = updatedParticipant.UserId;
            existing.SessionId = updatedParticipant.SessionId;

            // basic adatok
            existing.DisplayName = updatedParticipant.DisplayName;
            existing.Score = updatedParticipant.Score;
            existing.Rank = updatedParticipant.Rank;
            existing.IsFinished = updatedParticipant.IsFinished;

            // navigation property
            existing.User = updatedParticipant.User;
            existing.Session = updatedParticipant.Session;

            // collections
            existing.ParticipantAnswers = updatedParticipant.ParticipantAnswers;
            existing.Messages = updatedParticipant.Messages;

            await _context.SaveChangesAsync();

            return existing;
        }

        // DELETE
        public async Task<bool> DeleteAsync(string id)
        {
            var participant = await _context.Participants.FindAsync(id);

            if (participant == null)
                return false;

            _context.Participants.Remove(participant);
            await _context.SaveChangesAsync();

            return true;
        }

        // SESSION alapú lekérés 
        public async Task<IReadOnlyCollection<Participant>> GetBySessionIdAsync(string sessionId)
        {
            return await _context.Participants
                .Where(p => p.SessionId == sessionId)
                .Include(p => p.User)
                .ToListAsync();
        }

        public async Task<bool> IsUserJoinedAsync(string userId, string sessionId)
        {
            return await _context.Participants.AnyAsync(p => p.UserId == userId && p.SessionId == sessionId);
        }

        public async Task<bool> IsExtUserJoinedAsync(string extUserId, string sessionId)
        {
            return await _context.Participants.AnyAsync(p => p.ExternalUserId == extUserId && p.SessionId == sessionId);
        }

        public async Task<Participant?> GetIdByUserIdAndSessionIdAsync(string userId, string sessionId)
        {
            return await _context.Participants.FirstOrDefaultAsync(p => p.UserId == userId && p.SessionId == sessionId);
        }

        public async Task<Participant?> GetIdByExtUserIdAndSessionIdAsync(string extUserId, string sessionId)
        {
            return await _context.Participants.FirstOrDefaultAsync(p => p.ExternalUserId == extUserId && p.SessionId == sessionId);
        }
    }
}
