using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    internal class ParticipantAnswerService : IParticipantAnswerService
    {
        private readonly SparqDbContext _context;

        public ParticipantAnswerService(SparqDbContext context)
        {
            _context = context;
        }

        public async Task<ParticipantAnswer> CreateAsync(ParticipantAnswer participantAnswer)
        {
            participantAnswer.AnsweredAt = DateTime.UtcNow;

            if (participantAnswer.Answer != null)
            {
                participantAnswer.IsCorrect = participantAnswer.Answer.IsCorrect;

                participantAnswer.PointsEarned = participantAnswer.IsCorrect ? 1 : 0;
            }

            _context.ParticipantAnswers.Add(participantAnswer);
            await _context.SaveChangesAsync();

            return participantAnswer;
        }

        public async Task<ParticipantAnswer?> GetByIdAsync(string id)
        {
            return await _context.ParticipantAnswers
                .Include(pa => pa.Participant)
                .Include(pa => pa.Question)
                .Include(pa => pa.Answer)
                .FirstOrDefaultAsync(pa => pa.Id == id);
        }

        public async Task<IReadOnlyCollection<ParticipantAnswer>> GetAllAsync()
        {
            return await _context.ParticipantAnswers
                .Include(pa => pa.Participant)
                .Include(pa => pa.Question)
                .Include(pa => pa.Answer)
                .ToListAsync();
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var entity = await _context.ParticipantAnswers.FindAsync(id);

            if (entity == null)
                return false;

            _context.ParticipantAnswers.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IReadOnlyCollection<ParticipantAnswer>> GetByParticipantIdAsync(string participantId)
        {
            return await _context.ParticipantAnswers
                .Where(pa => pa.ParticipantId == participantId)
                .Include(pa => pa.Question)
                .Include(pa => pa.Answer)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<ParticipantAnswer>> GetByQuestionIdAsync(string questionId)
        {
            return await _context.ParticipantAnswers
                .Where(pa => pa.QuestionId == questionId)
                .Include(pa => pa.Participant)
                .Include(pa => pa.Answer)
                .ToListAsync();
        }

        public async Task<ParticipantAnswer?> GetParticipantAnswerAsync(string sessionId, string questionId, string? userId, string? extUserId)
        {
            var query = _context.ParticipantAnswers
                .AsQueryable()
                .Where(pa =>
                    pa.SessionId == sessionId &&
                    pa.QuestionId == questionId);

            if (!string.IsNullOrWhiteSpace(userId))
            {
                query = query.Where(pa => pa.Participant!.UserId == userId);
            }
            else if (!string.IsNullOrWhiteSpace(extUserId))
            {
                query = query.Where(pa => pa.Participant!.ExternalUserId == extUserId);
            }
            else
            {
                return null;
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<ParticipantAnswer>> GetBySessionAndQuestionAsync(string sessionId, string questionId)
        {
            return await _context.ParticipantAnswers
                .Where(pa =>
                    pa.SessionId == sessionId &&
                    pa.QuestionId == questionId)
                .Include(pa => pa.Participant)
                .Include(pa => pa.Answer)
                .Include(pa => pa.Question)
                .ToListAsync();
        }

        public async Task<IReadOnlyCollection<ParticipantAnswer>> GetBySessionAsync(string sessionId)
        {
            return await _context.ParticipantAnswers
                .Include(x => x.Participant)
                .Where(x => x.SessionId == sessionId)
                .ToListAsync();
        }
    }
}
