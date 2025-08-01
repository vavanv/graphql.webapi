using GraphQL.WebApi.Mvc.Models;

namespace GraphQL.WebApi.Mvc.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IGraphQLService _graphQLService;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IGraphQLService graphQLService, ILogger<CustomerService> logger)
        {
            _graphQLService = graphQLService;
            _logger = logger;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all customers");
                var customers = await _graphQLService.GetCustomersAsync();
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
                var customer = await _graphQLService.GetCustomerByIdAsync(id);
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
                var createdCustomer = await _graphQLService.CreateCustomerAsync(customer);
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
                var updatedCustomer = await _graphQLService.UpdateCustomerAsync(customer);
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
    }
} 