using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface ISessionQuestionStateService
    {
        Task<SessionQuestionState?> CreateAsync(string sessionId, string questionId);
        Task<bool> DeactivateCurrentAsync(string sessionId);
        Task<SessionQuestionState?> GetActiveBySessionIdAsync(string sessionId);
    }
}
