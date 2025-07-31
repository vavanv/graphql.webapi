using GraphQL.WebApi.Data;
using GraphQL.WebApi.Model;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.WebApi.GraphQL
{
    public class Query
    {
        public IQueryable<Customer> customers([Service] ApplicationDbContext context)
        {
            try
            {
                return context.Customers.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error accessing customers: {ex.Message}");
            }
        }

        public async Task<Customer?> customer(int id, [Service] ApplicationDbContext context)
        {
            try
            {
                return await context.Customers.FirstOrDefaultAsync(c => c.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error accessing customer with id {id}: {ex.Message}");
            }
        }
    }
}