using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using GraphQL.WebApi.Tests.TestHelpers;
using Xunit;

namespace GraphQL.WebApi.Tests.GraphQL
{
    public class AuthTests : BaseIntegrationTest
    {
        public AuthTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GraphQL_AccessibleWithoutAuthentication()
        {
            // Act - Test that GraphQL endpoint is accessible without authentication
            var response = await _client.GetAsync("/graphql?query={customers{id,firstName}}");
            
            // Assert - Should return OK since GraphQL API doesn't require authentication
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GraphQL_PostRequest_WorksWithoutAuthentication()
        {
            // Arrange
            const string query = "{ customers { id firstName lastName } }";
            var content = new StringContent(
                JsonSerializer.Serialize(new { query }),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/graphql", content);
            
            // Assert
            response.EnsureSuccessStatusCode();
            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseContent);
            
            // Should have data, not errors
            Assert.True(json.RootElement.TryGetProperty("data", out _));
        }

        [Fact]
        public async Task GraphQL_InvalidQuery_ReturnsGraphQLError()
        {
            // Arrange
            const string invalidQuery = "{ nonExistentField { id } }";
            var content = new StringContent(
                JsonSerializer.Serialize(new { query = invalidQuery }),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/graphql", content);
            
            // Assert
            // GraphQL can return 400 for invalid queries, which is acceptable
            Assert.True(response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.BadRequest);
            var responseContent = await response.Content.ReadAsStringAsync();
            var json = JsonDocument.Parse(responseContent);
            
            // Should have errors
            Assert.True(json.RootElement.TryGetProperty("errors", out _));
        }
    }
}
