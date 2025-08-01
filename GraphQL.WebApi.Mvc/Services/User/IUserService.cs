using GraphQL.WebApi.Mvc.Models;

namespace GraphQL.WebApi.Mvc.Services
{
    public interface IUserService
    {
        Task<List<User>> GetUsersAsync();
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> CreateUserAsync(User user, string password);
        Task<User?> UpdateUserRoleAsync(int id, string role);
        Task<User?> UpdateUserLastLoginAsync(int id);
    }
}