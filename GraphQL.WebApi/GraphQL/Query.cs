using GraphQL.WebApi.Data;
using GraphQL.WebApi.Model;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.WebApi.GraphQL
{
    public class Query
    {
        [UseDbContext(typeof(ApplicationDbContext))]
        public IQueryable<Customer> GetCustomers([Service] ApplicationDbContext context)
        {
            return context.Customers;
        }

        [UseDbContext(typeof(ApplicationDbContext))]
        public async Task<Customer?> GetCustomer(int id, [Service] ApplicationDbContext context)
        {
            return await context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}