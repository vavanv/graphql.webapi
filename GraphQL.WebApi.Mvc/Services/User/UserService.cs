using GraphQL.WebApi.Mvc.Models;

namespace GraphQL.WebApi.Mvc.Services
{
    public class UserService : IUserService
    {
        private readonly IGraphQLClient _graphQLClient;
        private readonly ILogger<UserService> _logger;

        public UserService(IGraphQLClient graphQLClient, ILogger<UserService> logger)
        {
            _graphQLClient = graphQLClient;
            _logger = logger;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            try
            {
                var query = @"
                    query {
                        users {
                            id
                            username
                            email
                            passwordHash
                            firstName
                            lastName
                            isActive
                            createdAt
                            lastLoginAt
                        }
                    }";

                var response = await _graphQLClient.ExecuteQueryAsync(query);
                return response?.Data?.Users ?? new List<User>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users from GraphQL API");
                return new List<User>();
            }
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            try
            {
                var query = @"
                    query($username: String!) {
                        user(username: $username) {
                            id
                            username
                            email
                            passwordHash
                            firstName
                            lastName
                            isActive
                            createdAt
                            lastLoginAt
                        }
                    }";

                var variables = new { username };
                var response = await _graphQLClient.ExecuteQueryAsync(query, variables);
                return response?.Data?.User;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user with username {Username} from GraphQL API", username);
                return null;
            }
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            try
            {
                var query = @"
                    query($id: Int!) {
                        userById(id: $id) {
                            id
                            username
                            email
                            passwordHash
                            firstName
                            lastName
                            isActive
                            createdAt
                            lastLoginAt
                        }
                    }";

                var variables = new { id };
                var response = await _graphQLClient.ExecuteQueryAsync(query, variables);
                return response?.Data?.UserById;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user with ID {Id} from GraphQL API", id);
                return null;
            }
        }

        public async Task<User?> CreateUserAsync(User user, string password)
        {
            try
            {
                var mutation = @"
                    mutation($username: String!, $email: String!, $password: String!, $firstName: String!, $lastName: String!) {
                        addUser(username: $username, email: $email, password: $password, firstName: $firstName, lastName: $lastName) {
                            id
                            username
                            email
                            firstName
                            lastName
                            isActive
                            createdAt
                        }
                    }";

                var variables = new
                {
                    username = user.Username,
                    email = user.Email,
                    password = password,
                    firstName = user.FirstName,
                    lastName = user.LastName
                };

                var response = await _graphQLClient.ExecuteQueryAsync(mutation, variables);
                return response?.Data?.AddUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user with username {Username} via GraphQL API", user.Username);
                return null;
            }
        }

        public async Task<User?> UpdateUserLastLoginAsync(int id)
        {
            try
            {
                var mutation = @"
                    mutation($id: Int!) {
                        updateUserLastLogin(id: $id) {
                            id
                            username
                            email
                            firstName
                            lastName
                            isActive
                            lastLoginAt
                        }
                    }";

                var variables = new { id };
                var response = await _graphQLClient.ExecuteQueryAsync(mutation, variables);
                return response?.Data?.UpdateUserLastLogin;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user last login for id {Id} via GraphQL API", id);
                return null;
            }
        }
    }
}