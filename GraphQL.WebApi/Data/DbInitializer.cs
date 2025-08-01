using GraphQL.WebApi.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace GraphQL.WebApi.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            // Seed users if they don't exist
            if (!context.Users.Any())
            {
                var users = new User[]
                {
                    new User
                    {
                        Username = "admin",
                        Email = "admin@example.com",
                        PasswordHash = HashPassword("admin123"),
                        FirstName = "Admin",
                        LastName = "User",
                        Role = AppRoles.Admin,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Username = "manager",
                        Email = "manager@example.com",
                        PasswordHash = HashPassword("manager123"),
                        FirstName = "Manager",
                        LastName = "User",
                        Role = AppRoles.Manager,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Username = "user",
                        Email = "user@example.com",
                        PasswordHash = HashPassword("user123"),
                        FirstName = "Regular",
                        LastName = "User",
                        Role = AppRoles.User,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    },
                    new User
                    {
                        Username = "guest",
                        Email = "guest@example.com",
                        PasswordHash = HashPassword("guest123"),
                        FirstName = "Guest",
                        LastName = "User",
                        Role = AppRoles.Guest,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.Users.AddRange(users);
                context.SaveChanges();
            }

            // Check if customers already exist
            if (context.Customers.Any())
            {
                return; // Database has been seeded
            }

            var customers = new Customer[]
            {
                new Customer
                {
                    FirstName = "John",
                    LastName = "Doe",
                    Contact = "+1-555-0101",
                    Email = "john.doe@email.com",
                    DateOfBirth = new DateTime(1985, 3, 15)
                },
                new Customer
                {
                    FirstName = "Jane",
                    LastName = "Smith",
                    Contact = "+1-555-0102",
                    Email = "jane.smith@email.com",
                    DateOfBirth = new DateTime(1990, 7, 22)
                },
                new Customer
                {
                    FirstName = "Michael",
                    LastName = "Johnson",
                    Contact = "+1-555-0103",
                    Email = "michael.johnson@email.com",
                    DateOfBirth = new DateTime(1982, 11, 8)
                },
                new Customer
                {
                    FirstName = "Sarah",
                    LastName = "Williams",
                    Contact = "+1-555-0104",
                    Email = "sarah.williams@email.com",
                    DateOfBirth = new DateTime(1988, 4, 12)
                },
                new Customer
                {
                    FirstName = "David",
                    LastName = "Brown",
                    Contact = "+1-555-0105",
                    Email = "david.brown@email.com",
                    DateOfBirth = new DateTime(1995, 9, 30)
                },
                new Customer
                {
                    FirstName = "Emily",
                    LastName = "Davis",
                    Contact = "+1-555-0106",
                    Email = "emily.davis@email.com",
                    DateOfBirth = new DateTime(1992, 1, 18)
                },
                new Customer
                {
                    FirstName = "Robert",
                    LastName = "Wilson",
                    Contact = "+1-555-0107",
                    Email = "robert.wilson@email.com",
                    DateOfBirth = new DateTime(1987, 6, 25)
                },
                new Customer
                {
                    FirstName = "Lisa",
                    LastName = "Anderson",
                    Contact = "+1-555-0108",
                    Email = "lisa.anderson@email.com",
                    DateOfBirth = new DateTime(1993, 12, 3)
                },
                new Customer
                {
                    FirstName = "James",
                    LastName = "Taylor",
                    Contact = "+1-555-0109",
                    Email = "james.taylor@email.com",
                    DateOfBirth = new DateTime(1980, 8, 14)
                },
                new Customer
                {
                    FirstName = "Amanda",
                    LastName = "Martinez",
                    Contact = "+1-555-0110",
                    Email = "amanda.martinez@email.com",
                    DateOfBirth = new DateTime(1991, 2, 28)
                }
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }

        private static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
}