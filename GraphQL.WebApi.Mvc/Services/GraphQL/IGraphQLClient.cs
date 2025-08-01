using GraphQL.WebApi.Mvc.Models;

namespace GraphQL.WebApi.Mvc.Services
{
    public interface IGraphQLClient
    {
        Task<GraphQLResponse?> ExecuteQueryAsync(string query, object? variables = null);
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