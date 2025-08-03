using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using GraphQL.WebApi.Data;
using HotChocolate.Execution;
using System.Text;
using System.Text.Json;

namespace GraphQL.WebApi.Tests.TestHelpers
{
    public abstract class BaseIntegrationTest : IClassFixture<TestWebApplicationFactory>
    {
        protected readonly TestWebApplicationFactory _factory;
        protected readonly HttpClient _client;
        protected readonly ApplicationDbContext _dbContext;

        protected BaseIntegrationTest(TestWebApplicationFactory factory)
        {
            _factory = factory;
            _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            var scope = factory.Services.CreateScope();
            _dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        protected async Task<JsonDocument> ExecuteGraphQLQueryAsync(string query, object? variables = null)
        {
            var content = new StringContent(
                JsonSerializer.Serialize(new { query, variables }),
                Encoding.UTF8,
                "application/json");

            var response = await _client.PostAsync("/graphql", content);
            
            // Don't throw on 400 status codes - GraphQL can return 400 for invalid queries
            // and we want to test error responses
            if (response.StatusCode != System.Net.HttpStatusCode.BadRequest)
            {
                response.EnsureSuccessStatusCode();
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonDocument.Parse(responseContent);
        }

        protected async Task<JsonDocument> ExecuteGraphQLMutationAsync(string mutation, object? variables = null)
        {
            return await ExecuteGraphQLQueryAsync(mutation, variables);
        }
    }
}
