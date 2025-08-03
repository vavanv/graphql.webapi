using GraphQL.WebApi.Data;
using GraphQL.WebApi.Model;
using System;
using System.Linq;

namespace GraphQL.WebApi.TestUtils.TestData
{
    public static class TestDataSeeder
    {
        public static void SeedTestData(ApplicationDbContext context)
        {
            if (!context.Users.Any())
            {
                // Add test users with different roles
                context.Users.AddRange(
                    new User 
                    { 
                        Username = "testadmin",
                        Email = "admin@test.com",
                        PasswordHash = "hashedpassword123",
                        FirstName = "Admin",
                        LastName = "User",
                        Role = AppRoles.Admin,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Username = "testmanager",
                        Email = "manager@test.com",
                        PasswordHash = "hashedpassword123",
                        FirstName = "Manager",
                        LastName = "User",
                        Role = AppRoles.Manager,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Username = "testuser",
                        Email = "user@test.com",
                        PasswordHash = "hashedpassword123",
                        FirstName = "Regular",
                        LastName = "User",
                        Role = AppRoles.User,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Username = "inactiveuser",
                        Email = "inactive@test.com",
                        PasswordHash = "hashedpassword123",
                        FirstName = "Inactive",
                        LastName = "User",
                        Role = AppRoles.User,
                        IsActive = false,
                        CreatedAt = DateTime.UtcNow
                    }
                );
            }

            if (!context.Customers.Any())
            {
                // Add test customers
                context.Customers.AddRange(
                    new Customer
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        Email = "john.doe@example.com",
                        Contact = "1234567890",
                        DateOfBirth = new DateTime(1980, 1, 1)
                    },
                    new Customer
                    {
                        FirstName = "Jane",
                        LastName = "Smith",
                        Email = "jane.smith@example.com",
                        Contact = "0987654321",
                        DateOfBirth = new DateTime(1990, 5, 15)
                    },
                    new Customer
                    {
                        FirstName = "Alice",
                        LastName = "Johnson",
                        Email = "alice.johnson@example.com",
                        Contact = "1122334455",
                        DateOfBirth = new DateTime(1985, 10, 25)
                    }
                );
            }

            context.SaveChanges();
        }
    }
}
