using Xunit;
using GraphQL.WebApi.Data;
using Microsoft.EntityFrameworkCore;
using GraphQL.WebApi.Model;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace GraphQL.WebApi.Tests.Unit
{
    public class DbInitializerTests
    {
        private readonly DbContextOptions<ApplicationDbContext> _dbContextOptions;

        public DbInitializerTests()
        {
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance for each test method.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            _dbContextOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDb")
                .UseInternalServiceProvider(serviceProvider)
                .Options;
        }

        [Fact]
        public void Initialize_WithEmptyDb_SeedsAdminUser()
        {
            // Arrange
            using var context = new ApplicationDbContext(_dbContextOptions);
            
            // Act
            DbInitializer.Initialize(context);
            
            // Assert
            var adminUser = context.Users.FirstOrDefault(u => u.Username == "admin");
            Assert.NotNull(adminUser);
            Assert.Equal("Admin", adminUser.Role);
            Assert.True(adminUser.IsActive);
        }

        [Fact]
        public void Initialize_WithEmptyDb_SeedsSampleCustomers()
        {
            // Arrange
            using var context = new ApplicationDbContext(_dbContextOptions);
            
            // Act
            DbInitializer.Initialize(context);
            
            // Assert
            var customers = context.Customers.ToList();
            Assert.True(customers.Count >= 4); // At least 4 sample customers should be seeded
            
            // Verify at least one customer has expected data
            var johnDoe = customers.FirstOrDefault(c => c.FirstName == "John" && c.LastName == "Doe");
            Assert.NotNull(johnDoe);
            Assert.Equal("john.doe@email.com", johnDoe.Email);
        }

        [Fact]
        public void Initialize_WithExistingData_DoesNotDuplicateData()
        {
            // Arrange
            using var context = new ApplicationDbContext(_dbContextOptions);
            
            // First initialization
            DbInitializer.Initialize(context);
            var initialUserCount = context.Users.Count();
            var initialCustomerCount = context.Customers.Count();
            
            // Act - Initialize again
            DbInitializer.Initialize(context);
            
            // Assert - Counts should remain the same
            Assert.Equal(initialUserCount, context.Users.Count());
            Assert.Equal(initialCustomerCount, context.Customers.Count());
        }
    }
}
