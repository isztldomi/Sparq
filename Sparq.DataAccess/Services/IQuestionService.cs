using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface IQuestionService
    {
        Task<Question> CreateAsync(Question question);
        Task<Question?> GetByIdAsync(int id);
        Task<IReadOnlyCollection<Question>> GetAllAsync();
        Task<Question?> UpdateAsync(int id, Question updatedQuestion);
        Task<bool> DeleteAsync(int id);
    }
}
