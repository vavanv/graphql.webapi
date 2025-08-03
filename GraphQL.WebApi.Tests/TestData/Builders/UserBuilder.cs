using GraphQL.WebApi.Model;
using System;

namespace GraphQL.WebApi.Tests.TestData.Builders
{
    public class UserBuilder
    {
        private readonly User _user = new()
        {
            Username = "testuser",
            Email = "test.user@example.com",
            PasswordHash = "hashedpassword123",
            FirstName = "Test",
            LastName = "User",
            Role = AppRoles.User,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        public UserBuilder WithId(int id)
        {
            _user.Id = id;
            return this;
        }

        public UserBuilder WithUsername(string username)
        {
            _user.Username = username;
            return this;
        }

        public UserBuilder WithEmail(string email)
        {
            _user.Email = email;
            return this;
        }

        public UserBuilder WithPasswordHash(string passwordHash)
        {
            _user.PasswordHash = passwordHash;
            return this;
        }

        public UserBuilder WithRole(string role)
        {
            _user.Role = role;
            return this;
        }

        public UserBuilder WithActiveStatus(bool isActive)
        {
            _user.IsActive = isActive;
            return this;
        }

        public UserBuilder WithCreatedAt(DateTime createdAt)
        {
            _user.CreatedAt = createdAt;
            return this;
        }

        public User Build()
        {
            return _user;
        }
    }
}
