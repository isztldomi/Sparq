using Microsoft.AspNetCore.Identity;
using Sparq.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sparq.DataAccess.Services
{
    public interface IUsersService
    {
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task<(string? authToken, string? refreshToken, string? userId, string? error)> LoginAsync(string email, string password);
        Task<(string? authToken, string? refreshToken, string? userId, string? error)> RedeemRefreshTokenAsync(string refreshToken);
        Task LogoutAsync();
        Task<User?> GetCurrentUserAsync();
        string? GetCurrentUserId();
        Task<User> GetUserByIdAsync(string id);
        Task<User> UpdateNickNameAsync(string id, string nickname);
    }
}
