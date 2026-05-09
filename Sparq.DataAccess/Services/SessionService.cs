using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public class SessionService : ISessionService
    {
        private readonly SparqDbContext _context;

        public SessionService(SparqDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<Session?> CreateAsync(string snapshotId)
        {
            var snapshot = await _context.Snapshots
                .FirstOrDefaultAsync(s => s.Id == snapshotId);

            if (snapshot == null)
            {
                return null;
            }

            var session = new Session
            {
                SnapshotId = snapshotId,
                CreatedAt = DateTime.UtcNow,
                IsWaiting = false,
                IsRunning = false,
                PinCode = snapshot.PinCode,
            };

            _context.Sessions.Add(session);
            await _context.SaveChangesAsync();

            return session;
        }

        // READ by id
        public async Task<Session?> GetByIdAsync(string id)
        {
            return await _context.Sessions
                .Include(s => s.Snapshot)
                .Include(s => s.Participants)
                .Include(s => s.Messages)
                .FirstOrDefaultAsync(s => s.Id == id);
        }

        // READ all
        public async Task<IReadOnlyCollection<Session>> GetAllAsync()
        {
            return await _context.Sessions
                .Include(s => s.Snapshot)
                .ToListAsync();
        }

        // UPDATE
        public async Task<Session?> UpdateAsync(string id, Session updatedSession)
        {
            var existing = await _context.Sessions
                .Include(s => s.Participants)
                .Include(s => s.Messages)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (existing == null)
                return null;

            // Snapshot kapcsolat
            existing.SnapshotId = updatedSession.SnapshotId;

            // runtime state mezők
            existing.StartedAt = updatedSession.StartedAt;
            existing.EndedAt = updatedSession.EndedAt;
            existing.CurrentQuestionId = updatedSession.CurrentQuestionId;
            existing.PinCode = updatedSession.PinCode;
            existing.IsWaiting = updatedSession.IsWaiting;
            existing.IsRunning = updatedSession.IsRunning;

            // navigation property
            existing.Snapshot = updatedSession.Snapshot;

            // collections
            existing.Participants = updatedSession.Participants;
            existing.Messages = updatedSession.Messages;

            await _context.SaveChangesAsync();

            return existing;
        }

        // DELETE
        public async Task<bool> DeleteAsync(string id)
        {
            var session = await _context.Sessions.FindAsync(id);

            if (session == null)
                return false;

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ActivateForWaitingByIdAsync(string id)
        {
            var session = await _context.Sessions
                .Include(s => s.Snapshot)
                .Include(s => s.Participants)
                .Include(s => s.Messages)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (session == null)
                return false;

            session.IsWaiting = true;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<(List<Session> Items, int TotalCount)> GetAllPublicWaitingSessionsPagedAsync(int page, int pageSize)
        {
            var query = _context.Sessions
                .Include(s => s.Snapshot)
                .ThenInclude(s => s.Quiz)
                .Where(s =>
                    s.IsWaiting &&
                    s.Snapshot != null &&
                    s.Snapshot.Quiz != null &&
                    s.Snapshot.Quiz.IsPublic &&
                    s.Snapshot.Quiz.IsActive);
            var totalCount = query.Count();
            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            return (items, totalCount);
        }
    }
}
