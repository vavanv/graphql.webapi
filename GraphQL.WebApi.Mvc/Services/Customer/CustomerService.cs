using GraphQL.WebApi.Mvc.Models;

namespace GraphQL.WebApi.Mvc.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IGraphQLClient _graphQLClient;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IGraphQLClient graphQLClient, ILogger<CustomerService> logger)
        {
            _graphQLClient = graphQLClient;
            _logger = logger;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all customers");

                var query = @"
                    query {
                        customers {
                            id
                            firstName
                            lastName
                            contact
                            email
                            dateOfBirth
                        }
                    }";

                var response = await _graphQLClient.ExecuteQueryAsync(query);
                var customers = response?.Data?.Customers ?? new List<Customer>();

                _logger.LogInformation("Successfully fetched {Count} customers", customers.Count);
                return customers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customers");
                throw;
            }
        }

        public async Task<Customer?> GetCustomerByIdAsync(int id)
        {
            try
            {
                _logger.LogInformation("Fetching customer with ID: {Id}", id);

                var query = @"
                    query($id: Int!) {
                        customer(id: $id) {
                            id
                            firstName
                            lastName
                            contact
                            email
                            dateOfBirth
                        }
                    }";

                var variables = new { id };
                var response = await _graphQLClient.ExecuteQueryAsync(query, variables);
                var customer = response?.Data?.Customer;

                if (customer != null)
                {
                    _logger.LogInformation("Successfully fetched customer: {CustomerName}", $"{customer.FirstName} {customer.LastName}");
                }
                else
                {
                    _logger.LogWarning("Customer with ID {Id} not found", id);
                }
                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customer with ID: {Id}", id);
                throw;
            }
        }

        public async Task<Customer?> CreateCustomerAsync(Customer customer)
        {
            try
            {
                _logger.LogInformation("Creating new customer: {CustomerName}", $"{customer.FirstName} {customer.LastName}");

                var mutation = @"
                    mutation($firstName: String!, $lastName: String!, $contact: String!, $email: String!, $dateOfBirth: DateTime!) {
                        addCustomer(firstName: $firstName, lastName: $lastName, contact: $contact, email: $email, dateOfBirth: $dateOfBirth) {
                            id
                            firstName
                            lastName
                            contact
                            email
                            dateOfBirth
                        }
                    }";

                var variables = new
                {
                    firstName = customer.FirstName,
                    lastName = customer.LastName,
                    contact = customer.Contact,
                    email = customer.Email,
                    dateOfBirth = customer.DateOfBirth
                };

                var response = await _graphQLClient.ExecuteQueryAsync(mutation, variables);
                var createdCustomer = response?.Data?.AddCustomer;

                if (createdCustomer != null)
                {
                    _logger.LogInformation("Successfully created customer with ID: {Id}", createdCustomer.Id);
                }
                else
                {
                    _logger.LogWarning("Failed to create customer: GraphQL service returned null");
                }
                return createdCustomer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating customer: {CustomerName}", $"{customer.FirstName} {customer.LastName}");
                throw;
            }
        }

        public async Task<Customer?> UpdateCustomerAsync(Customer customer)
        {
            try
            {
                _logger.LogInformation("Updating customer with ID: {Id}", customer.Id);

                var mutation = @"
                    mutation($id: Int!, $firstName: String!, $lastName: String!, $contact: String!, $email: String!, $dateOfBirth: DateTime!) {
                        updateCustomer(id: $id, firstName: $firstName, lastName: $lastName, contact: $contact, email: $email, dateOfBirth: $dateOfBirth) {
                            id
                            firstName
                            lastName
                            contact
                            email
                            dateOfBirth
                        }
                    }";

                var variables = new
                {
                    id = customer.Id,
                    firstName = customer.FirstName,
                    lastName = customer.LastName,
                    contact = customer.Contact,
                    email = customer.Email,
                    dateOfBirth = customer.DateOfBirth
                };

                var response = await _graphQLClient.ExecuteQueryAsync(mutation, variables);
                var updatedCustomer = response?.Data?.UpdateCustomer;

                if (updatedCustomer != null)
                {
                    _logger.LogInformation("Successfully updated customer with ID: {Id}", updatedCustomer.Id);
                }
                else
                {
                    _logger.LogWarning("Failed to update customer with ID {Id}: GraphQL service returned null", customer.Id);
                }
                return updatedCustomer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating customer with ID: {Id}", customer.Id);
                throw;
            }
        }

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            try
            {
                _logger.LogInformation("Deleting customer with ID: {Id}", id);

                var mutation = @"
                    mutation($id: Int!) {
                        deleteCustomer(id: $id)
                    }";

                var variables = new
                {
                    id = id
                };

                var response = await _graphQLClient.ExecuteQueryAsync(mutation, variables);
                var deleteResult = response?.Data?.DeleteCustomer;

                if (deleteResult == true)
                {
                    _logger.LogInformation("Successfully deleted customer with ID: {Id}", id);
                }
                else
                {
                    _logger.LogWarning("Failed to delete customer with ID {Id}: GraphQL service returned false", id);
                }
                return deleteResult ?? false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer with ID: {Id}", id);
                throw;
            }
        }
    }
}