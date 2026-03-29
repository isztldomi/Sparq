using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    internal class QuizService : IQuizService
    {
        private readonly SparqDbContext _context;

        public QuizService(SparqDbContext context)
        {
            _context = context;
        }
        // CREATE
        public async Task<Quiz> CreateAsync(Quiz quiz)
        {
            quiz.CreatedAt = DateTime.UtcNow;
            quiz.UpdatedAt = DateTime.UtcNow;

            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();

            return quiz;
        }

        // READ by id
        public async Task<Quiz?> GetByIdAsync(int id)
        {
            return await _context.Quizzes
                .Include(q => q.Owner)
                .Include(q => q.Snapshots)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        // READ all
        public async Task<IReadOnlyCollection<Quiz>> GetAllAsync()
        {
            return await _context.Quizzes
                .Include(q => q.Owner)
                .ToListAsync();
        }

        // UPDATE (teljes objektum frissítés)
        public async Task<Quiz?> UpdateAsync(int id, Quiz updatedQuiz)
        {
            var existing = await _context.Quizzes
                .Include(q => q.Snapshots)
                .FirstOrDefaultAsync(q => q.Id == id);

            if (existing == null)
                return null;

            // Egyszerű mezők
            existing.OwnerId = updatedQuiz.OwnerId;
            existing.IsPublic = updatedQuiz.IsPublic;
            existing.IsActive = updatedQuiz.IsActive;
            existing.UpdatedAt = DateTime.UtcNow;

            // Owner (ha explicit akarod frissíteni)
            existing.Owner = updatedQuiz.Owner;

            // Snapshots kezelés (egyszerű csere)
            existing.Snapshots = updatedQuiz.Snapshots;

            await _context.SaveChangesAsync();

            return existing;
        }

        // DELETE (hard delete)
        public async Task<bool> DeleteAsync(int id)
        {
            var quiz = await _context.Quizzes.FindAsync(id);

            if (quiz == null)
                return false;

            _context.Quizzes.Remove(quiz);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
