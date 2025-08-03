using System.Text;
using System.Text.Json;
using Xunit;
using GraphQL.WebApi.Tests.TestHelpers;
using GraphQL.WebApi.Model;
using System.Net.Http.Json;

namespace GraphQL.WebApi.Tests.GraphQL
{
    public class UserTests : BaseIntegrationTest
    {
        public UserTests(TestWebApplicationFactory factory) : base(factory)
        {
        }

        [Fact]
        public async Task GetUsers_ReturnsListOfUsers()
        {
            // Arrange
            const string query = @"
                query {
                    users {
                        id
                        username
                        email
                        role
                        isActive
                    }
                }";

            // Act
            var result = await ExecuteGraphQLQueryAsync(query);

            // Assert
            var users = result.RootElement.GetProperty("data").GetProperty("users");
            Assert.True(users.GetArrayLength() > 0);
        }

        [Fact]
        public async Task AddUser_CreatesNewUser()
        {
            // Arrange
            const string mutation = @"
                mutation AddUser($username: String!, $email: String!, $password: String!, $firstName: String!, $lastName: String!, $role: String!) {
                    addUser(
                        username: $username,
                        email: $email,
                        password: $password,
                        firstName: $firstName,
                        lastName: $lastName,
                        role: $role
                    ) {
                        id
                        username
                        email
                        role
                    }
                }";

            var variables = new
            {
                username = "newtestuser", // Use unique username
                email = "newtestuser@example.com", // Use unique email
                password = "Test@1234",
                firstName = "NewTest",
                lastName = "User",
                role = "User"
            };

            // Act
            var result = await ExecuteGraphQLMutationAsync(mutation, variables);

            // Assert
            var newUser = result.RootElement.GetProperty("data").GetProperty("addUser");
            Assert.True(newUser.GetProperty("id").GetInt32() > 0);
            Assert.Equal("newtestuser", newUser.GetProperty("username").GetString());
            Assert.Equal("newtestuser@example.com", newUser.GetProperty("email").GetString());
            Assert.Equal("User", newUser.GetProperty("role").GetString());
        }

        [Fact]
        public async Task UpdateUserRole_UpdatesUserRole()
        {
            // Arrange
            var user = _dbContext.Users.First(u => u.Role == "User");
            const string newRole = "Manager";

            var mutation = $@"
                mutation UpdateUserRole {{
                    updateUserRole(
                        id: {user.Id},
                        role: ""{newRole}""
                    ) {{
                        id
                        role
                    }}
                }}";

            // Act
            var result = await ExecuteGraphQLMutationAsync(mutation);

            // Assert
            var updatedUser = result.RootElement.GetProperty("data").GetProperty("updateUserRole");
            Assert.Equal(user.Id, updatedUser.GetProperty("id").GetInt32());
            Assert.Equal(newRole, updatedUser.GetProperty("role").GetString());
        }

        [Fact]
        public async Task UpdateUserLastLogin_UpdatesLastLoginTimestamp()
        {
            // Arrange
            var user = _dbContext.Users.First();
            var initialLastLogin = user.LastLoginAt;

            var mutation = $@"
                mutation UpdateUserLastLogin {{
                    updateUserLastLogin(id: {user.Id}) {{
                        id
                        lastLoginAt
                    }}
                }}";

            // Act
            var result = await ExecuteGraphQLMutationAsync(mutation);

            // Assert
            var updatedUser = result.RootElement.GetProperty("data").GetProperty("updateUserLastLogin");
            Assert.Equal(user.Id, updatedUser.GetProperty("id").GetInt32());

            // The last login should be updated to a newer timestamp
            var updatedLastLogin = updatedUser.GetProperty("lastLoginAt").GetDateTime();
            Assert.True(updatedLastLogin > initialLastLogin || initialLastLogin == null);
        }
    }
}
