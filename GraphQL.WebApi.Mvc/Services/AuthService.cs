using GraphQL.WebApi.Mvc.Models;
using System.Security.Cryptography;
using System.Text;

namespace GraphQL.WebApi.Mvc.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGraphQLService _graphQLService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IGraphQLService graphQLService, ILogger<AuthService> logger)
        {
            _graphQLService = graphQLService;
            _logger = logger;
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            try
            {
                // For now, we'll use hardcoded users since we don't have GraphQL queries for users yet
                var validUsers = new Dictionary<string, string>
                {
                    { "admin", HashPassword("admin123") },
                    { "user", HashPassword("user123") }
                };

                if (validUsers.ContainsKey(username))
                {
                    return VerifyPassword(password, validUsers[username]);
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating user {Username}", username);
                return false;
            }
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            try
            {
                // For now, return hardcoded user data
                var users = new Dictionary<string, User>
                {
                    { "admin", new User { Id = 1, Username = "admin", Email = "admin@example.com", FirstName = "Admin", LastName = "User", IsActive = true } },
                    { "user", new User { Id = 2, Username = "user", Email = "user@example.com", FirstName = "Regular", LastName = "User", IsActive = true } }
                };

                return users.ContainsKey(username) ? users[username] : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user {Username}", username);
                return null;
            }
        }

        public async Task<bool> RegisterUserAsync(RegisterViewModel model)
        {
            try
            {
                // For now, just return true as we don't have user registration in GraphQL yet
                _logger.LogInformation("User registration attempted for {Username}", model.Username);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user {Username}", model.Username);
                return false;
            }
        }

        public string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        public bool VerifyPassword(string password, string hash)
        {
            var passwordHash = HashPassword(password);
            return passwordHash == hash;
        }
    }
} 