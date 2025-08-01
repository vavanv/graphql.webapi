using GraphQL.WebApi.Mvc.Models;

namespace GraphQL.WebApi.Mvc.Services
{
    public class GraphQLResponse
    {
        public GraphQLData? Data { get; set; }
        public List<GraphQLError>? Errors { get; set; }
    }
}