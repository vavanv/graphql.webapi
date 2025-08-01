using GraphQL.WebApi.Mvc.Models;

namespace GraphQL.WebApi.Mvc.Services
{
    public interface IGraphQLClient
    {
        Task<GraphQLResponse?> ExecuteQueryAsync(string query, object? variables = null);
    }
}