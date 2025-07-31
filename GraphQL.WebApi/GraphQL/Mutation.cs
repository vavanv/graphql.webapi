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

        public async Task<Customer?> updateCustomer(
            int id,
            string firstName,
            string lastName,
            string contact,
            string email,
            DateTime dateOfBirth,
            [Service] ApplicationDbContext context)
        {
            try
            {
                var customer = await context.Customers.FindAsync(id);
                if (customer == null)
                {
                    throw new Exception($"Customer with id {id} not found");
                }

                customer.FirstName = firstName;
                customer.LastName = lastName;
                customer.Contact = contact;
                customer.Email = email;
                customer.DateOfBirth = dateOfBirth;

                await context.SaveChangesAsync();

                return customer;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating customer: {ex.Message}");
            }
        }
    }
} 