using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface ISnapshotService
    {
        Task<Snapshot> CreateAsync(Snapshot version);
        Task<Snapshot?> GetByIdAsync(string id);
        Task<IReadOnlyCollection<Snapshot>> GetAllAsync();
        Task<Snapshot?> UpdateAsync(string id, Snapshot updatedVersion);
        Task<bool> DeleteAsync(string id);
    }
}
