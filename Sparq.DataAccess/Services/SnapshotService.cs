using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    internal class SnapshotService : ISnapshotService
    {
        private readonly SparqDbContext _context;

        public SnapshotService(SparqDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<Snapshot> CreateAsync(Snapshot snapshot)
        {
            var quiz = await _context.Quizzes.FindAsync(snapshot.QuizId);
            if (quiz == null)
                throw new Exception("Quiz not found");

            var lastSnapshotNumber = await _context.Snapshots
                .Where(s => s.QuizId == snapshot.QuizId)
                .MaxAsync(s => (int?)s.SnapshotNumber) ?? 0;

            snapshot.SnapshotNumber = lastSnapshotNumber + 1;
            snapshot.CreatedAt = DateTime.UtcNow;

            _context.Snapshots.Add(snapshot);

            quiz.LastSnapshot = snapshot;
            quiz.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return snapshot;
        }

        public async Task<Snapshot?> GetByIdAsync(string id)
        {
            return await _context.Snapshots
                .Include(v => v.Quiz)
                .Include(v => v.Questions)
                .Include(v => v.Sessions)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        // READ all
        public async Task<IReadOnlyCollection<Snapshot>> GetAllAsync()
        {
            return await _context.Snapshots
                .Include(v => v.Quiz)
                .ToListAsync();
        }

        // UPDATE
        public async Task<Snapshot?> UpdateAsync(string id, Snapshot updatedSnapshot)
        {
            var existing = await _context.Snapshots
                .Include(v => v.Questions)
                .Include(v => v.Sessions)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (existing == null)
                return null;

            // egyszerű mezők
            existing.QuizId = updatedSnapshot.QuizId;
            existing.SnapshotNumber = updatedSnapshot.SnapshotNumber;
            existing.Title = updatedSnapshot.Title;
            existing.Description = updatedSnapshot.Description;
            existing.TimeLimit = updatedSnapshot.TimeLimit;

            // navigation property-k
            existing.Quiz = updatedSnapshot.Quiz;

            // egyszerű csere (nem mindig safe)
            existing.Questions = updatedSnapshot.Questions;
            existing.Sessions = updatedSnapshot.Sessions;

            await _context.SaveChangesAsync();

            return existing;
        }

        // DELETE
        public async Task<bool> DeleteAsync(string id)
        {
            var snapshot = await _context.Snapshots.FindAsync(id);

            if (snapshot == null)
                return false;

            _context.Snapshots.Remove(snapshot);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
