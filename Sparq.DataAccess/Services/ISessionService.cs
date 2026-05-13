using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface ISessionService
    {
        Task<Session?> CreateAsync(string snapshotId);
        Task<Session?> GetByIdAsync(string id);
        Task<IReadOnlyCollection<Session>> GetAllAsync();
        Task<Session?> UpdateAsync(string id, Session updatedSession);
        Task<bool> DeleteAsync(string id); 
        Task<bool> ActivateForWaitingByIdAsync(string id); 
        Task<(List<Session> Items, int TotalCount)> GetAllPublicWaitingSessionsPagedAsync(int page, int pageSize);
        Task<bool> DeactivateSessionAsync(string id);
        Task<bool> StartSessionAsync(string id);
        Task<bool> EndSessionAsync(string id);
    }
}
