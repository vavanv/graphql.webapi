using GraphQL.WebApi.Mvc.Models;
using GraphQL.WebApi.Mvc.Services;
using Microsoft.AspNetCore.Mvc;

namespace GraphQL.WebApi.Mvc.Controllers
{
    public class CustomersController : Controller
    {
        private readonly IGraphQLService _graphQLService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(IGraphQLService graphQLService, ILogger<CustomersController> logger)
        {
            _graphQLService = graphQLService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var customers = await _graphQLService.GetCustomersAsync();
                return View(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customers");
                ViewBag.ErrorMessage = "Unable to fetch customers. Please try again later.";
                return View(new List<Customer>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var customer = await _graphQLService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return View(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customer with id {Id}", id);
                ViewBag.ErrorMessage = "Unable to fetch customer details. Please try again later.";
                return View();
            }
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Contact,Email,DateOfBirth")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createdCustomer = await _graphQLService.CreateCustomerAsync(customer);
                    if (createdCustomer != null)
                    {
                        TempData["SuccessMessage"] = "Customer created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to create customer. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating customer");
                    ModelState.AddModelError("", "An error occurred while creating the customer. Please try again.");
                }
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var customer = await _graphQLService.GetCustomerByIdAsync(id);
                if (customer == null)
                {
                    return NotFound();
                }
                return View(customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customer with id {Id} for editing", id);
                ViewBag.ErrorMessage = "Unable to fetch customer for editing. Please try again later.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Customers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Contact,Email,DateOfBirth")] Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var updatedCustomer = await _graphQLService.UpdateCustomerAsync(customer);
                    if (updatedCustomer != null)
                    {
                        TempData["SuccessMessage"] = "Customer updated successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update customer. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating customer with id {Id}", id);
                    ModelState.AddModelError("", "An error occurred while updating the customer. Please try again.");
                }
            }

            return View(customer);
        }
    }
} 