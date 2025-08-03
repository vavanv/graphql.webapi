using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using GraphQL.WebApi.Data;
using GraphQL.WebApi.TestUtils.TestData;
using Microsoft.Extensions.Logging;

namespace GraphQL.WebApi.Tests.TestHelpers
{
    public class TestWebApplicationFactory : WebApplicationFactory<Program>
    {
        private static int _databaseCounter = 0;

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Set environment to "Test" to prevent DbInitializer from running
            builder.UseEnvironment("Test");

            builder.ConfigureServices(services =>
            {
                // Remove the existing database context registration
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                // Add ApplicationDbContext using an in-memory database for testing
                // Use unique database name for each test instance
                var databaseName = $"InMemoryDbForTesting_{Interlocked.Increment(ref _databaseCounter)}";
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase(databaseName);
                });

                // Build the service provider
                var sp = services.BuildServiceProvider();

                // Create a scope to obtain a reference to the database context
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<TestWebApplicationFactory>>();

                    // Ensure the database is created
                    db.Database.EnsureCreated();

                    try
                    {
                        // Clear existing data and ensure database is created
                        db.Database.EnsureDeleted();
                        db.Database.EnsureCreated();
                        
                        // Seed the database with test data only (skip main DbInitializer)
                        TestDataSeeder.SeedTestData(db);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "An error occurred seeding the database with test data. Error: {Message}", ex.Message);
                        throw; // Rethrow to fail the test
                    }
                }
            });
        }
    }
}
