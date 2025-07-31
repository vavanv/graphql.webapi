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

        public IQueryable<User> users([Service] ApplicationDbContext context)
        {
            try
            {
                return context.Users.AsQueryable();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error accessing users: {ex.Message}");
            }
        }

        public async Task<User?> user(string username, [Service] ApplicationDbContext context)
        {
            try
            {
                return await context.Users.FirstOrDefaultAsync(u => u.Username == username);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error accessing user with username {username}: {ex.Message}");
            }
        }

        public async Task<User?> userById(int id, [Service] ApplicationDbContext context)
        {
            try
            {
                return await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error accessing user with id {id}: {ex.Message}");
            }
        }
    }
}