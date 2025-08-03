using GraphQL.WebApi.Mvc.Models;
using GraphQL.WebApi.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace GraphQL.WebApi.Mvc.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager},{AppRoles.User},{AppRoles.Guest}")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var customers = await _customerService.GetCustomersAsync();
                ViewBag.UserRole = User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value ?? "Guest";
                return View(customers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching customers");
                ViewBag.ErrorMessage = "Unable to fetch customers. Please try again later.";
                return View(new List<Customer>());
            }
        }

        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager},{AppRoles.User},{AppRoles.Guest}")]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
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
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager},{AppRoles.User}")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Customers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager},{AppRoles.User}")]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Contact,Email,DateOfBirth")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createdCustomer = await _customerService.CreateCustomerAsync(customer);
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
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var customer = await _customerService.GetCustomerByIdAsync(id);
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
        [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager}")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Contact,Email,DateOfBirth")] Customer customer)
        {
            _logger.LogInformation("Edit request received for customer ID: {Id}, ModelState.IsValid: {IsValid}", id, ModelState.IsValid);

            if (id != customer.Id)
            {
                _logger.LogWarning("ID mismatch: URL ID {UrlId} != Customer ID {CustomerId}", id, customer.Id);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Invalid customer ID." });
                }
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("Attempting to update customer: {Customer}", JsonSerializer.Serialize(customer));
                    var updatedCustomer = await _customerService.UpdateCustomerAsync(customer);

                    if (updatedCustomer != null)
                    {
                        _logger.LogInformation("Customer updated successfully: {UpdatedCustomer}", JsonSerializer.Serialize(updatedCustomer));
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            // AJAX request - return JSON
                            return Json(new { success = true, message = "Customer updated successfully!" });
                        }
                        else
                        {
                            // Regular form submission
                            TempData["SuccessMessage"] = "Customer updated successfully!";
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    else
                    {
                        _logger.LogWarning("GraphQL service returned null for customer update");
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = false, message = "Failed to update customer. The GraphQL service returned null." });
                        }
                        else
                        {
                            ModelState.AddModelError("", "Failed to update customer. Please try again.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating customer with id {Id}", id);
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = $"An error occurred while updating the customer: {ex.Message}" });
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred while updating the customer. Please try again.");
                    }
                }
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                var errorMessage = string.Join(", ", errors);
                _logger.LogWarning("ModelState validation failed: {Errors}", errorMessage);

                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = $"Validation errors: {errorMessage}" });
                }
                else
                {
                    return View(customer);
                }
            }

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = "Invalid data. Please check your input." });
            }
            else
            {
                return View(customer);
            }
        }

        // POST: Customers/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation("Delete request received for customer ID: {Id}", id);

                var deleteResult = await _customerService.DeleteCustomerAsync(id);

                if (deleteResult)
                {
                    _logger.LogInformation("Customer deleted successfully with ID: {Id}", id);
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = true, message = "Customer deleted successfully!" });
                    }
                    else
                    {
                        TempData["SuccessMessage"] = "Customer deleted successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    _logger.LogWarning("Failed to delete customer with ID {Id}: GraphQL service returned false", id);
                    if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    {
                        return Json(new { success = false, message = "Failed to delete customer. Please try again." });
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Failed to delete customer. Please try again.";
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting customer with id {Id}", id);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = $"An error occurred while deleting the customer: {ex.Message}" });
                }
                else
                {
                    TempData["ErrorMessage"] = "An error occurred while deleting the customer. Please try again.";
                    return RedirectToAction(nameof(Index));
                }
            }
        }
    }
}