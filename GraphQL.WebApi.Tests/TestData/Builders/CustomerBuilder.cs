using GraphQL.WebApi.Model;

namespace GraphQL.WebApi.Tests.TestData.Builders
{
    public class CustomerBuilder
    {
        private readonly Customer _customer = new()
        {
            FirstName = "Test",
            LastName = "User",
            Contact = "1234567890",
            Email = "test.user@example.com",
            DateOfBirth = new DateTime(1990, 1, 1)
        };

        public CustomerBuilder WithId(int id)
        {
            _customer.Id = id;
            return this;
        }

        public CustomerBuilder WithFirstName(string firstName)
        {
            _customer.FirstName = firstName;
            return this;
        }

        public CustomerBuilder WithLastName(string lastName)
        {
            _customer.LastName = lastName;
            return this;
        }

        public CustomerBuilder WithEmail(string email)
        {
            _customer.Email = email;
            return this;
        }

        public CustomerBuilder WithContact(string contact)
        {
            _customer.Contact = contact;
            return this;
        }

        public CustomerBuilder WithDateOfBirth(DateTime dateOfBirth)
        {
            _customer.DateOfBirth = dateOfBirth;
            return this;
        }

        public Customer Build()
        {
            return _customer;
        }
    }
}
