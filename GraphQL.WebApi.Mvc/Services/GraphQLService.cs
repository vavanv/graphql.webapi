using GraphQL.WebApi.Mvc.Models;
using System.Text;
using System.Text.Json;

namespace GraphQL.WebApi.Mvc.Services
{
    public class GraphQLService : IGraphQLService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GraphQLService> _logger;

        public GraphQLService(HttpClient httpClient, ILogger<GraphQLService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            try
            {
                var query = @"
                    query {
                        customers {
                            id
                            firstName
                            lastName
                            contact
                            email
                            dateOfBirth
                        }
                    }";

                var response = await ExecuteGraphQLQueryAsync(query);
                return response?.Data?.Customers ?? new List<Customer>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customers from GraphQL API");
                return new List<Customer>();
            }
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            try
            {
                var query = @"
                    query($id: Int!) {
                        customer(id: $id) {
                            id
                            firstName
                            lastName
                            contact
                            email
                            dateOfBirth
                        }
                    }";

                var variables = new { id };
                var response = await ExecuteGraphQLQueryAsync(query, variables);
                return response?.Data?.Customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customer with id {Id} from GraphQL API", id);
                return null;
            }
        }

        public async Task<Customer?> CreateCustomerAsync(Customer customer)
        {
            try
            {
                var mutation = @"
                    mutation($firstName: String!, $lastName: String!, $contact: String!, $email: String!, $dateOfBirth: DateTime!) {
                        addCustomer(firstName: $firstName, lastName: $lastName, contact: $contact, email: $email, dateOfBirth: $dateOfBirth) {
                            id
                            firstName
                            lastName
                            contact
                            email
                            dateOfBirth
                        }
                    }";

                var variables = new
                {
                    firstName = customer.FirstName,
                    lastName = customer.LastName,
                    contact = customer.Contact,
                    email = customer.Email,
                    dateOfBirth = customer.DateOfBirth
                };

                var response = await ExecuteGraphQLQueryAsync(mutation, variables);
                return response?.Data?.AddCustomer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer via GraphQL API");
                return null;
            }
        }

        public async Task<Customer?> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                var mutation = @"
                    mutation($id: Int!, $firstName: String!, $lastName: String!, $contact: String!, $email: String!, $dateOfBirth: DateTime!) {
                        updateCustomer(id: $id, firstName: $firstName, lastName: $lastName, contact: $contact, email: $email, dateOfBirth: $dateOfBirth) {
                            id
                            firstName
                            lastName
                            contact
                            email
                            dateOfBirth
                        }
                    }";

                var variables = new
                {
                    id = customer.Id,
                    firstName = customer.FirstName,
                    lastName = customer.LastName,
                    contact = customer.Contact,
                    email = customer.Email,
                    dateOfBirth = customer.DateOfBirth
                };

                var response = await ExecuteGraphQLQueryAsync(mutation, variables);
                return response?.Data?.UpdateCustomer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer with id {Id} via GraphQL API", customer.Id);
                return null;
            }
        }

        // User-related methods
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
                            firstName
                            lastName
                            isActive
                            createdAt
                            lastLoginAt
                        }
                    }";

                var response = await ExecuteGraphQLQueryAsync(query);
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
                var response = await ExecuteGraphQLQueryAsync(query, variables);
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
                var response = await ExecuteGraphQLQueryAsync(query, variables);
                return response?.Data?.UserById;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user with id {Id} from GraphQL API", id);
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

                var response = await ExecuteGraphQLQueryAsync(mutation, variables);
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
                var response = await ExecuteGraphQLQueryAsync(mutation, variables);
                return response?.Data?.UpdateUserLastLogin;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user last login for id {Id} via GraphQL API", id);
                return null;
            }
        }

        private async Task<GraphQLResponse?> ExecuteGraphQLQueryAsync(string query, object? variables = null)
        {
            var request = new
            {
                query,
                variables
            };

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("", content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<GraphQLResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }

    public class GraphQLResponse
    {
        public GraphQLData? Data { get; set; }
        public List<GraphQLError>? Errors { get; set; }
    }

    public class GraphQLData
    {
        public List<Customer>? Customers { get; set; }
        public Customer? Customer { get; set; }
        public Customer? AddCustomer { get; set; }
        public Customer? UpdateCustomer { get; set; }
        public List<User>? Users { get; set; }
        public User? User { get; set; }
        public User? AddUser { get; set; }
        public User? UpdateUserLastLogin { get; set; }
        public User? UserById { get; set; }
    }

    public class GraphQLError
    {
        public string? Message { get; set; }
        public List<GraphQLLocation>? Locations { get; set; }
    }

    public class GraphQLLocation
    {
        public int Line { get; set; }
        public int Column { get; set; }
    }
}