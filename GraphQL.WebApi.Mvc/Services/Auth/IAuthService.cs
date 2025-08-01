using GraphQL.WebApi.Mvc.Models;

namespace GraphQL.WebApi.Mvc.Services
{
    public interface IAuthService
    {
        Task<bool> ValidateUserAsync(string username, string password);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<bool> RegisterUserAsync(RegisterViewModel model);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}