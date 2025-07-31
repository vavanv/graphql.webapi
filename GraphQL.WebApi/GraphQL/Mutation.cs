using GraphQL.WebApi.Data;
using GraphQL.WebApi.Model;

namespace GraphQL.WebApi.GraphQL
{
    public class Mutation
    {
        public async Task<Customer> addCustomer(
            string firstName,
            string lastName,
            string contact,
            string email,
            DateTime dateOfBirth,
            [Service] ApplicationDbContext context)
        {
            try
            {
                var customer = new Customer
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Contact = contact,
                    Email = email,
                    DateOfBirth = dateOfBirth
                };

                context.Customers.Add(customer);
                await context.SaveChangesAsync();

                return customer;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding customer: {ex.Message}");
            }
        }
    }
} 