using GraphQL.WebApi.Data;
using GraphQL.WebApi.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

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

        public async Task<User?> addUser(
            string username,
            string email,
            string password,
            string firstName,
            string lastName,
            [Service] ApplicationDbContext context)
        {
            try
            {
                // Check if user already exists
                var existingUser = await context.Users.FirstOrDefaultAsync(u => u.Username == username || u.Email == email);
                if (existingUser != null)
                {
                    throw new Exception("User with this username or email already exists");
                }

                var user = new User
                {
                    Username = username,
                    Email = email,
                    PasswordHash = HashPassword(password),
                    FirstName = firstName,
                    LastName = lastName,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding user: {ex.Message}");
            }
        }

        public async Task<User?> updateUserLastLogin(int id, [Service] ApplicationDbContext context)
        {
            try
            {
                var user = await context.Users.FindAsync(id);
                if (user == null)
                {
                    throw new Exception($"User with id {id} not found");
                }

                user.LastLoginAt = DateTime.UtcNow;
                await context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating user last login: {ex.Message}");
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
} 