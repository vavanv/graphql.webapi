using GraphQL.WebApi.Model;
using Microsoft.EntityFrameworkCore;

namespace GraphQL.WebApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
    }
}
