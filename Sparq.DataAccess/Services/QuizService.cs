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
            quiz.IsActive = true;
            foreach (var snapshot in quiz.Snapshots)
            {
                snapshot.SnapshotNumber = 1;
                snapshot.CreatedAt = DateTime.UtcNow;
                
            }
            _context.Quizzes.Add(quiz);
            
            await _context.SaveChangesAsync();
            var snapshotID = quiz.Snapshots.FirstOrDefault()?.Id;
            quiz.LastSnapshotId = snapshotID;
            await _context.SaveChangesAsync();

            return quiz;
        }

        // READ by id
        public async Task<Quiz?> GetByIdAsync(int id)
        {
            return await _context.Quizzes
                .Include(q => q.Owner)
                .Include(q => q.Snapshots)
                .FirstOrDefaultAsync(q => q.Id == id && q.IsActive);
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

        public async Task<(List<Quiz> Items, int TotalCount)> GetByUserPagedAsync(string userId, int page, int pageSize)
        {
            var query = _context.Quizzes
                .Where(q => q.OwnerId == userId && q.IsActive);

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(q => q.UpdatedAt > q.CreatedAt ? q.UpdatedAt : q.CreatedAt)
                .ThenByDescending(q => q.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
        public async Task DeactivateAsync(int quizId, string userId)
        {
            var quiz = await _context.Quizzes
                .FirstOrDefaultAsync(q => q.Id == quizId);

            if (quiz == null)
                throw new Exception("Quiz not found");

            quiz.IsActive = false;
            quiz.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
