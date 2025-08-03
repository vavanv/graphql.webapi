using System.Text.Json;
using System.Text;
using Xunit;
using GraphQL.WebApi.Tests.TestHelpers;
using GraphQL.WebApi.Model;
using System.Net.Http.Json;

namespace GraphQL.WebApi.Tests.GraphQL
{
    public class CustomerTests : BaseIntegrationTest
    {
        public CustomerTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetCustomers_ReturnsListOfCustomers()
        {
            // Arrange
            const string query = @"
                query {
                    customers {
                        id
                        firstName
                        lastName
                        email
                    }
                }";

            // Act
            var result = await ExecuteGraphQLQueryAsync(query);

            // Assert
            var customers = result.RootElement.GetProperty("data").GetProperty("customers");
            Assert.True(customers.GetArrayLength() > 0);
        }

        [Fact]
        public async Task GetCustomerById_ReturnsCorrectCustomer()
        {
            // Arrange
            var customer = _dbContext.Customers.First();
            var query = $@"
                query {{
                    customer(id: {customer.Id}) {{
                        id
                        firstName
                        lastName
                        email
                    }}
                }}";

            // Act
            var result = await ExecuteGraphQLQueryAsync(query);

            // Assert
            var resultCustomer = result.RootElement.GetProperty("data").GetProperty("customer");
            Assert.Equal(customer.Id, resultCustomer.GetProperty("id").GetInt32());
            Assert.Equal(customer.FirstName, resultCustomer.GetProperty("firstName").GetString());
        }

        [Fact]
        public async Task AddCustomer_ReturnsNewCustomer()
        {
            // Arrange
            const string mutation = @"
                mutation AddCustomer($firstName: String!, $lastName: String!, $contact: String!, $email: String!, $dateOfBirth: DateTime!) {
                    addCustomer(
                        firstName: $firstName,
                        lastName: $lastName,
                        contact: $contact,
                        email: $email,
                        dateOfBirth: $dateOfBirth
                    ) {
                        id
                        firstName
                        lastName
                        email
                    }
                }";

            var variables = new
            {
                firstName = "Test",
                lastName = "User",
                contact = "1234567890",
                email = "test.user@example.com",
                dateOfBirth = new DateTime(1990, 1, 1)
            };

            // Act
            var result = await ExecuteGraphQLMutationAsync(mutation, variables);

            // Assert
            var newCustomer = result.RootElement.GetProperty("data").GetProperty("addCustomer");
            Assert.True(newCustomer.GetProperty("id").GetInt32() > 0);
            Assert.Equal("Test", newCustomer.GetProperty("firstName").GetString());
            Assert.Equal("User", newCustomer.GetProperty("lastName").GetString());
        }

        [Fact]
        public async Task UpdateCustomer_UpdatesCustomerDetails()
        {
            // Arrange
            var customer = _dbContext.Customers.First();
            const string newEmail = "updated.email@example.com";

            var mutation = $@"
                mutation UpdateCustomer {{
                    updateCustomer(
                        id: {customer.Id},
                        firstName: ""{customer.FirstName}"",
                        lastName: ""{customer.LastName}"",
                        contact: ""{customer.Contact}"",
                        email: ""{newEmail}"",
                        dateOfBirth: ""{customer.DateOfBirth:yyyy-MM-dd}""
                    ) {{
                        id
                        email
                    }}
                }}";

            // Act
            var result = await ExecuteGraphQLMutationAsync(mutation);

            // Assert
            var updatedCustomer = result.RootElement.GetProperty("data").GetProperty("updateCustomer");
            Assert.Equal(customer.Id, updatedCustomer.GetProperty("id").GetInt32());
            Assert.Equal(newEmail, updatedCustomer.GetProperty("email").GetString());
        }
    }
}

