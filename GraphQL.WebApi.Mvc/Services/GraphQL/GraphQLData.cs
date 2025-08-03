using GraphQL.WebApi.Mvc.Models;

namespace GraphQL.WebApi.Mvc.Services
{
    public class GraphQLData
    {
        public List<Customer>? Customers { get; set; }
        public Customer? Customer { get; set; }
        public Customer? AddCustomer { get; set; }
        public Customer? UpdateCustomer { get; set; }
        public bool? DeleteCustomer { get; set; }
        public List<User>? Users { get; set; }
        public User? User { get; set; }
        public User? AddUser { get; set; }
        public User? UpdateUserLastLogin { get; set; }
        public User? UpdateUserRole { get; set; }
        public User? UserById { get; set; }
    }
}