using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface ISessionService
    {
        Task<Session?> CreateAsync(int snapshotId);
        Task<Session?> GetByIdAsync(int id);
        Task<IReadOnlyCollection<Session>> GetAllAsync();
        Task<Session?> UpdateAsync(int id, Session updatedSession);
        Task<bool> DeleteAsync(int id);
    }
}
