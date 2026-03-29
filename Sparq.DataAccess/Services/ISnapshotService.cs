using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface ISnapshotService
    {
        Task<Snapshot> CreateAsync(Snapshot version);
        Task<Snapshot?> GetByIdAsync(int id);
        Task<IReadOnlyCollection<Snapshot>> GetAllAsync();
        Task<Snapshot?> UpdateAsync(int id, Snapshot updatedVersion);
        Task<bool> DeleteAsync(int id);
    }
}
