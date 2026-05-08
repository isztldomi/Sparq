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
        Task<(List<Quiz> Items, int TotalCount)> GetByUserPagedAsync(string userId, int page, int pageSize);
        Task DeactivateAsync(int quizId, string userId);
        Task<(List<Session> Items, int TotalCount)> GetQuizSessionsPagedAsync(int quizId, int page, int pageSize);
    }
}
