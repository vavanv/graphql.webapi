using System.Text;
using System.Text.Json;
using GraphQL.WebApi.Mvc.Models;

namespace GraphQL.WebApi.Mvc.Services
{
    public class GraphQLClient : IGraphQLClient
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<GraphQLClient> _logger;

        public GraphQLClient(HttpClient httpClient, ILogger<GraphQLClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<GraphQLResponse?> ExecuteQueryAsync(string query, object? variables = null)
        {
            try
            {
                var request = new
                {
                    query,
                    variables
                };

                var json = JsonSerializer.Serialize(request);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogDebug("Executing GraphQL query: {Query}", query);

                var response = await _httpClient.PostAsync("", content);
                response.EnsureSuccessStatusCode();

                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<GraphQLResponse>(responseContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (result?.Errors?.Any() == true)
                {
                    _logger.LogError("GraphQL errors: {Errors}", JsonSerializer.Serialize(result.Errors));
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing GraphQL query");
                return null;
            }
        }
    }
}