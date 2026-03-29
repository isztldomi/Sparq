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

        // CREATE (válasz leadás)
        public async Task<ParticipantAnswer> CreateAsync(ParticipantAnswer participantAnswer)
        {
            participantAnswer.AnsweredAt = DateTime.UtcNow;

            // pontszám számítás logika helye
            if (participantAnswer.Answer != null)
            {
                participantAnswer.IsCorrect = participantAnswer.Answer.IsCorrect;

                participantAnswer.PointsEarned = participantAnswer.IsCorrect
                    ? 1 // vagy Question.Point stb.
                    : 0;
            }

            _context.ParticipantAnswers.Add(participantAnswer);
            await _context.SaveChangesAsync();

            return participantAnswer;
        }

        // READ by id
        public async Task<ParticipantAnswer?> GetByIdAsync(int id)
        {
            return await _context.ParticipantAnswers
                .Include(pa => pa.Participant)
                .Include(pa => pa.Question)
                .Include(pa => pa.Answer)
                .FirstOrDefaultAsync(pa => pa.Id == id);
        }

        // READ all
        public async Task<IReadOnlyCollection<ParticipantAnswer>> GetAllAsync()
        {
            return await _context.ParticipantAnswers
                .Include(pa => pa.Participant)
                .Include(pa => pa.Question)
                .Include(pa => pa.Answer)
                .ToListAsync();
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.ParticipantAnswers.FindAsync(id);

            if (entity == null)
                return false;

            _context.ParticipantAnswers.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }

        // Participant válaszai
        public async Task<IReadOnlyCollection<ParticipantAnswer>> GetByParticipantIdAsync(int participantId)
        {
            return await _context.ParticipantAnswers
                .Where(pa => pa.ParticipantId == participantId)
                .Include(pa => pa.Question)
                .Include(pa => pa.Answer)
                .ToListAsync();
        }

        // Kérdésre adott válaszok
        public async Task<IReadOnlyCollection<ParticipantAnswer>> GetByQuestionIdAsync(int questionId)
        {
            return await _context.ParticipantAnswers
                .Where(pa => pa.QuestionId == questionId)
                .Include(pa => pa.Participant)
                .Include(pa => pa.Answer)
                .ToListAsync();
        }
    }
}
