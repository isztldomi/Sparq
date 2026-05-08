using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface IQuizService
    {
        Task<Quiz> CreateAsync(Quiz quiz);
        Task<Quiz?> GetByIdAsync(string id);
        Task<IReadOnlyCollection<Quiz>> GetAllAsync();
        Task<Quiz?> UpdateAsync(string id, Quiz updatedQuiz);
        Task<bool> DeleteAsync(string id);
        Task<(List<Quiz> Items, int TotalCount)> GetByUserPagedAsync(string userId, int page, int pageSize);
        Task DeactivateAsync(string quizId, string userId);
        Task<(List<Session> Items, int TotalCount)> GetQuizSessionsPagedAsync(string quizId, int page, int pageSize);
    }
}
