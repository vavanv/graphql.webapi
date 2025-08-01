namespace GraphQL.WebApi.Mvc.Services
{
    public class GraphQLError
    {
        public string? Message { get; set; }
        public List<GraphQLLocation>? Locations { get; set; }
    }
}