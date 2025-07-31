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