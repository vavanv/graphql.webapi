using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using GraphQL.WebApi.Data;
using HotChocolate.Execution;
using System.Security.Claims;
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

            // Don't throw on 400 or 500 status codes - GraphQL can return these for invalid queries
            // and we want to test error responses
            if (response.StatusCode != System.Net.HttpStatusCode.BadRequest &&
                response.StatusCode != System.Net.HttpStatusCode.InternalServerError)
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

        protected HttpClient CreateAuthenticatedClient(List<Claim> claims)
        {
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });

            // For now, we'll use the default test authentication
            // In a real implementation, you would configure the authentication based on claims
            return client;
        }
    }
}
