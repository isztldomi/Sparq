using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    internal class AnswerService : IAnswerService
    {
        private readonly SparqDbContext _context;

        public AnswerService(SparqDbContext context)
        {
            _context = context;
        }

        public async Task<Answer> CreateAsync(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();

            return answer;
        }

        public async Task<Answer?> GetByIdAsync(string id)
        {
            return await _context.Answers
                .Include(a => a.Question)
                .Include(a => a.ParticipantAnswers)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IReadOnlyCollection<Answer>> GetAllAsync()
        {
            return await _context.Answers
                .Include(a => a.Question)
                .ToListAsync();
        }

        public async Task<Answer?> UpdateAsync(string id, Answer updatedAnswer)
        {
            var existing = await _context.Answers
                .Include(a => a.ParticipantAnswers)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (existing == null)
                return null;

            existing.QuestionId = updatedAnswer.QuestionId;
            existing.Text = updatedAnswer.Text;
            existing.IsCorrect = updatedAnswer.IsCorrect;

            existing.Question = updatedAnswer.Question;

            existing.ParticipantAnswers = updatedAnswer.ParticipantAnswers;

            await _context.SaveChangesAsync();

            return existing;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var answer = await _context.Answers.FindAsync(id);

            if (answer == null)
                return false;

            _context.Answers.Remove(answer);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IReadOnlyCollection<Answer>> GetByIdsAsync(IReadOnlyCollection<string> answerIds)
        {
            if (answerIds == null || answerIds.Count == 0)
                return new List<Answer>();

            return await _context.Answers
                .AsNoTracking()
                .Where(a => answerIds.Contains(a.Id))
                .ToListAsync();
        }
    }
}
