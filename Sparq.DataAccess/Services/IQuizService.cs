using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface IQuizService
    {
        Task<Quiz> CreateAsync(Quiz quiz);
        Task<Quiz?> GetByIdAsync(int id);
        Task<IReadOnlyCollection<Quiz>> GetAllAsync();
        Task<Quiz?> UpdateAsync(int id, Quiz updatedQuiz);
        Task<bool> DeleteAsync(int id);
    }
}
