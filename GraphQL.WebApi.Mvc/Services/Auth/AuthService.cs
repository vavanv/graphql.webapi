using GraphQL.WebApi.Mvc.Models;
using System.Security.Cryptography;
using System.Text;

namespace GraphQL.WebApi.Mvc.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IUserService userService, ILogger<AuthService> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<bool> ValidateUserAsync(string username, string password)
        {
            try
            {
                // Get user from database via GraphQL
                var user = await _userService.GetUserByUsernameAsync(username);

                if (user == null || !user.IsActive)
                {
                    _logger.LogWarning("User {Username} not found or inactive", username);
                    return false;
                }

                // Verify password
                var isValid = VerifyPassword(password, user.PasswordHash);

                if (isValid)
                {
                    _logger.LogInformation("User {Username} validated successfully", username);
                    // Update last login time
                    await _userService.UpdateUserLastLoginAsync(user.Id);
                }
                else
                {
                    _logger.LogWarning("Invalid password for user {Username}", username);
                }

                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating user {Username}", username);
                return false;
            }
        }

        public async Task<GraphQL.WebApi.Mvc.Models.User?> GetUserByUsernameAsync(string username)
        {
            try
            {
                // Get user from database via GraphQL
                var user = await _userService.GetUserByUsernameAsync(username);

                if (user != null)
                {
                    _logger.LogInformation("Retrieved user {Username} from database", username);
                }
                else
                {
                    _logger.LogWarning("User {Username} not found in database", username);
                }

                return user;
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
                // Check if user already exists
                var existingUser = await _userService.GetUserByUsernameAsync(model.Username);
                if (existingUser != null)
                {
                    _logger.LogWarning("User registration failed: Username {Username} already exists", model.Username);
                    return false;
                }

                // Create new user with role
                var newUser = new GraphQL.WebApi.Mvc.Models.User
                {
                    Username = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Role = model.Role ?? "User", // Default to User role if not specified
                    IsActive = true
                };

                var createdUser = await _userService.CreateUserAsync(newUser, model.Password);

                if (createdUser != null)
                {
                    _logger.LogInformation("User {Username} registered successfully with role {Role}", model.Username, newUser.Role);
                    return true;
                }
                else
                {
                    _logger.LogError("Failed to create user {Username}", model.Username);
                    return false;
                }
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