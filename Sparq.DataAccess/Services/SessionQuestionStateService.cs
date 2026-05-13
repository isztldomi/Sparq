using Microsoft.EntityFrameworkCore;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    internal class SessionQuestionStateService : ISessionQuestionStateService
    {
        private readonly SparqDbContext _context;

        public SessionQuestionStateService(SparqDbContext context)
        {
            _context = context;
        }

        public async Task<SessionQuestionState?> CreateAsync(string sessionId, string questionId)
        {
            var question = await _context.Questions.FirstOrDefaultAsync(q => q.Id == questionId);

            if (question == null)
                return null;

            var now = DateTime.UtcNow;

            var sessionQuestionState = new SessionQuestionState
            {
                SessionId = sessionId,
                QuestionId = questionId,
                Order = question.Order,
                StartedAt = now,
                EndsAt = question.TimeLimit.HasValue
                    ? now.AddSeconds(question.TimeLimit.Value)
                    : null,
                IsActive = true
            };

            _context.SessionQuestionStates.Add(sessionQuestionState);

            await _context.SaveChangesAsync();

            return sessionQuestionState;
        }
        public async Task<bool> DeactivateCurrentAsync(string sessionId)
        {
            var activeState = await _context.SessionQuestionStates
                .Where(sqs => sqs.SessionId == sessionId && sqs.IsActive)
                .FirstOrDefaultAsync();
            if (activeState == null)
                return false;
            activeState.IsActive = false;
            await _context.SaveChangesAsync();
            return true; 
        }
        public async Task<SessionQuestionState?> GetActiveBySessionIdAsync(string sessionId)
        {
            return await _context.SessionQuestionStates
                .Where(sqs => sqs.SessionId == sessionId && sqs.IsActive)
                .FirstOrDefaultAsync();
        }
    }
}
