using GraphQL.WebApi.Mvc.Models;

namespace GraphQL.WebApi.Mvc.Services
{
    public interface IGraphQLService
    {
        Task<List<Customer>> GetCustomersAsync();
        Task<Customer?> GetCustomerByIdAsync(int id);
        Task<Customer?> CreateCustomerAsync(Customer customer);
        Task<Customer?> UpdateCustomerAsync(Customer customer);

        // User-related methods
        Task<List<User>> GetUsersAsync();
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> CreateUserAsync(User user, string password);
        Task<User?> UpdateUserLastLoginAsync(int id);
    }
}