using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    internal class QuestionService : IQuestionService
    {
        private readonly SparqDbContext _context;

        public QuestionService(SparqDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<Question> CreateAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return question;
        }

        // READ by id
        public async Task<Question?> GetByIdAsync(string id)
        {
            return await _context.Questions
                .Include(q => q.Snapshot)
                .Include(q => q.Answers)
                .Include(q => q.ParticipantAnswers)
                .Include(q => q.Messages)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        // READ all
        public async Task<IReadOnlyCollection<Question>> GetAllAsync()
        {
            return await _context.Questions
                .Include(q => q.Snapshot)
                .ToListAsync();
        }

        // UPDATE
        public async Task<Question?> UpdateAsync(string id, Question updatedQuestion)
        {
            var existing = await _context.Questions
                .Include(q => q.Answers)
                .Include(q => q.ParticipantAnswers)
                .Include(q => q.Messages)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (existing == null)
                return null;

            // scalar mezők
            existing.SnapshotId = updatedQuestion.SnapshotId;
            existing.Title = updatedQuestion.Title;
            existing.Text = updatedQuestion.Text;
            existing.MediaId = updatedQuestion.MediaId;
            existing.TimeLimit = updatedQuestion.TimeLimit;
            existing.Point = updatedQuestion.Point;

            // navigation
            existing.Snapshot = updatedQuestion.Snapshot;

            existing.Answers = updatedQuestion.Answers;
            existing.ParticipantAnswers = updatedQuestion.ParticipantAnswers;
            existing.Messages = updatedQuestion.Messages;

            await _context.SaveChangesAsync();

            return existing;
        }

        // DELETE
        public async Task<bool> DeleteAsync(string id)
        {
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
                return false;

            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Question?> GetBySessionIdAndOrderAsync(string sessionId, int order)
        {
            return await _context.Questions
                .Include(q => q.Snapshot)
                .Include(q => q.Answers)
                .Include(q => q.ParticipantAnswers)
                .Include(q => q.Messages)
                .FirstOrDefaultAsync(q =>
                    q.Order == order &&
                    q.Snapshot != null &&
                    q.Snapshot.Sessions.Any(s => s.Id == sessionId));
        }
    }
}
