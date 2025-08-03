using GraphQL.WebApi.Mvc.Models;
using GraphQL.WebApi.Mvc.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GraphQL.WebApi.Mvc.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var users = await _userService.GetUsersAsync();
                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching users");
                ViewBag.ErrorMessage = "Unable to fetch users. Please try again later.";
                return View(new List<User>());
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }
                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user with id {Id}", id);
                ViewBag.ErrorMessage = "Unable to fetch user details. Please try again later.";
                return View();
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Roles = AppRoles.AllRoles.ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Username,Email,FirstName,LastName,Role")] User user, string password)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var createdUser = await _userService.CreateUserAsync(user, password);
                    if (createdUser != null)
                    {
                        TempData["SuccessMessage"] = "User created successfully!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to create user. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating user");
                    ModelState.AddModelError("", "An error occurred while creating the user. Please try again.");
                }
            }

            ViewBag.Roles = AppRoles.AllRoles.ToList();
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(int id)
        {
            try
            {
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                ViewBag.Roles = AppRoles.AllRoles.ToList();
                return View(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching user with id {Id} for role editing", id);
                ViewBag.ErrorMessage = "Unable to fetch user for role editing. Please try again later.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(int id, string role)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserRoleAsync(id, role);
                if (updatedUser != null)
                {
                    TempData["SuccessMessage"] = "User role updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Failed to update user role. Please try again.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user role for id {Id}", id);
                ModelState.AddModelError("", "An error occurred while updating the user role. Please try again.");
            }

            var user = await _userService.GetUserByIdAsync(id);
            ViewBag.Roles = AppRoles.AllRoles.ToList();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditRoleAjax(int id, string role)
        {
            try
            {
                var updatedUser = await _userService.UpdateUserRoleAsync(id, role);
                if (updatedUser != null)
                {
                    return Json(new { success = true, message = "User role updated successfully!" });
                }
                else
                {
                    return Json(new { success = false, message = "Failed to update user role. Please try again." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user role for id {Id}", id);
                return Json(new { success = false, message = "An error occurred while updating the user role. Please try again." });
            }
        }
    }
}