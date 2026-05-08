using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface IQuestionService
    {
        Task<Question> CreateAsync(Question question);
        Task<Question?> GetByIdAsync(string id);
        Task<IReadOnlyCollection<Question>> GetAllAsync();
        Task<Question?> UpdateAsync(string id, Question updatedQuestion);
        Task<bool> DeleteAsync(string id);
    }
}
