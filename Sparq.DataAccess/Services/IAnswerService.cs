using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface IAnswerService
    {
        Task<Answer> CreateAsync(Answer answer);
        Task<Answer?> GetByIdAsync(int id);
        Task<IReadOnlyCollection<Answer>> GetAllAsync();
        Task<Answer?> UpdateAsync(int id, Answer updatedAnswer);
        Task<bool> DeleteAsync(int id);
    }
}
