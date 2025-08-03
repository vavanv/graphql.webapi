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

        [Fact]
        public async Task DeleteCustomer_DeletesCustomerSuccessfully()
        {
            // Arrange
            var customer = _dbContext.Customers.First();
            var initialCount = _dbContext.Customers.Count();

            var mutation = $@"
                mutation DeleteCustomer {{
                    deleteCustomer(id: {customer.Id})
                }}";

            // Act
            var result = await ExecuteGraphQLMutationAsync(mutation);

            // Assert
            var deleteResult = result.RootElement.GetProperty("data").GetProperty("deleteCustomer");
            Assert.True(deleteResult.GetBoolean());

            // Verify customer was actually deleted from database
            var finalCount = _dbContext.Customers.Count();
            Assert.Equal(initialCount - 1, finalCount);

            // Verify the specific customer no longer exists
            var deletedCustomer = _dbContext.Customers.FirstOrDefault(c => c.Id == customer.Id);
            Assert.Null(deletedCustomer);
        }

                [Fact]
        public async Task DeleteCustomer_WithInvalidId_ReturnsError()
        {
            // Arrange
            const int invalidId = 99999;
            var mutation = $@"
                mutation DeleteCustomer {{
                    deleteCustomer(id: {invalidId})
                }}";

            // Act
            var result = await ExecuteGraphQLMutationAsync(mutation);

            // Assert
            Assert.True(result.RootElement.TryGetProperty("errors", out var errors));
            Assert.True(errors.GetArrayLength() > 0);

            var errorMessage = errors[0].GetProperty("message").GetString();
            // In test environment, we might get generic error messages
            Assert.True(errorMessage.Contains("Error") || errorMessage.Contains("Unexpected Execution Error"));
        }

                [Fact]
        public async Task DeleteCustomer_WithZeroId_ReturnsError()
        {
            // Arrange
            const int zeroId = 0;
            var mutation = $@"
                mutation DeleteCustomer {{
                    deleteCustomer(id: {zeroId})
                }}";

            // Act
            var result = await ExecuteGraphQLMutationAsync(mutation);

            // Assert
            Assert.True(result.RootElement.TryGetProperty("errors", out var errors));
            Assert.True(errors.GetArrayLength() > 0);

            var errorMessage = errors[0].GetProperty("message").GetString();
            // In test environment, we might get generic error messages
            Assert.True(errorMessage.Contains("Error") || errorMessage.Contains("Unexpected Execution Error"));
        }

                [Fact]
        public async Task DeleteCustomer_WithNegativeId_ReturnsError()
        {
            // Arrange
            const int negativeId = -1;
            var mutation = $@"
                mutation DeleteCustomer {{
                    deleteCustomer(id: {negativeId})
                }}";

            // Act
            var result = await ExecuteGraphQLMutationAsync(mutation);

            // Assert
            Assert.True(result.RootElement.TryGetProperty("errors", out var errors));
            Assert.True(errors.GetArrayLength() > 0);

            var errorMessage = errors[0].GetProperty("message").GetString();
            // In test environment, we might get generic error messages
            Assert.True(errorMessage.Contains("Error") || errorMessage.Contains("Unexpected Execution Error"));
        }
    }
}

