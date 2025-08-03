# GraphQL Web API with ASP.NET Core 8

A modern GraphQL API built with ASP.NET Core 8, HotChocolate GraphQL, Entity Framework Core, and a comprehensive MVC frontend with Role-Based Access Control (RBAC). This project demonstrates a complete full-stack solution with testing infrastructure and development utilities.

## 🚀 Features

- **GraphQL API**: HotChocolate GraphQL server with queries and mutations
- **Entity Framework Core 8**: Modern ORM with SQL Server LocalDB
- **ASP.NET Core MVC**: Rich web interface with Bootstrap 5
- **Role-Based Access Control (RBAC)**: Advanced authorization system with 4 user roles
- **Customer Deletion**: Secure deletion with Admin-only access and comprehensive testing
- **Modal Dialogs**: Interactive customer and user management with confirmation dialogs
- **AJAX Integration**: Seamless updates without page reloads
- **Professional Error Handling**: Modal-based permission denied dialogs
- **Comprehensive Logging**: Detailed error tracking and debugging
- **Testing Infrastructure**: Unit tests, integration tests, and comprehensive test coverage
- **Development Tools**: PowerShell scripts for testing and database management
- **Clean Architecture**: Separation of concerns with service layer pattern

## 🏗️ Project Structure

```
GraphQL.WebApi/
├── GraphQL.WebApi/                 # GraphQL API Project
│   ├── Data/                       # Database context and seeding
│   │   ├── ApplicationDbContext.cs # EF Core DbContext
│   │   └── DbInitializer.cs       # Database seeding logic
│   ├── GraphQL/                    # GraphQL queries and mutations
│   │   ├── Query.cs               # GraphQL queries
│   │   └── Mutation.cs            # GraphQL mutations
│   ├── Model/                      # Entity models
│   │   ├── AppRoles.cs            # Role definitions
│   │   ├── Customer.cs            # Customer entity
│   │   └── User.cs                # User entity
│   ├── Migrations/                 # EF Core migrations
│   ├── appsettings.json           # Configuration
│   └── Program.cs                  # API configuration
├── GraphQL.WebApi.Mvc/             # MVC Frontend Project
│   ├── Controllers/                # MVC controllers
│   │   ├── AccountController.cs   # Authentication controller
│   │   ├── CustomersController.cs # Customer management
│   │   ├── HomeController.cs      # Home page
│   │   └── UsersController.cs     # User management
│   ├── Models/                     # MVC-specific models
│   ├── Services/                   # Business logic services
│   │   ├── GraphQL/               # GraphQL client and response models
│   │   │   ├── GraphQLClient.cs   # HTTP client for GraphQL
│   │   │   ├── GraphQLResponse.cs # Response wrapper
│   │   │   ├── GraphQLData.cs     # Data container
│   │   │   ├── GraphQLError.cs    # Error information
│   │   │   └── GraphQLLocation.cs # Error location
│   │   ├── Customer/              # Customer-specific services
│   │   ├── User/                  # User management services
│   │   └── Auth/                  # Authentication services
│   ├── Views/                     # Razor views with modals
│   │   ├── Account/               # Authentication views
│   │   ├── Customers/             # Customer management views
│   │   ├── Users/                 # User management views
│   │   └── Shared/                # Layout and shared components
│   ├── wwwroot/                   # Static files (CSS, JS, libs)
│   └── Program.cs                 # MVC configuration
├── GraphQL.WebApi.Tests/           # Test Project
│   ├── GraphQL/                   # GraphQL API tests
│   │   ├── AuthTests.cs           # Authentication tests
│   │   ├── CustomerTests.cs       # Customer operation tests
│   │   ├── UserTests.cs           # User management tests
│   │   └── ErrorHandlingTests.cs  # Error handling tests
│   ├── TestData/                  # Test data builders
│   ├── TestHelpers/               # Test utilities
│   └── Unit/                      # Unit tests

├── PowerShell Scripts/             # Development and testing scripts
│   ├── test-auth.ps1              # Authentication testing
│   ├── verify-users.ps1           # User verification
│   ├── reset-db.ps1               # Database reset
│   ├── test-graphql.ps1           # GraphQL API testing
│   ├── test-mutation.ps1          # Mutation testing
│   └── test-*.ps1                 # Various test scripts
└── README.md                      # This documentation
```

## 🔒 Customer Deletion Functionality

### Overview

The customer deletion feature provides secure, role-based deletion with comprehensive testing coverage:

### Security Features

- **Admin-Only Access**: Only users with "Admin" role can delete customers
- **Server-Side Authorization**: `[Authorize(Roles = AppRoles.Admin)]` attribute protection
- **Client-Side Validation**: JavaScript permission checks with user-friendly error messages
- **Confirmation Dialogs**: Professional modal dialogs for delete confirmation
- **Permission Denied Modals**: Clear feedback for unauthorized users

### GraphQL Implementation

```csharp
// GraphQL Mutation
public async Task<bool> deleteCustomer(int id, [Service] ApplicationDbContext context)
{
    try
    {
        var customer = await context.Customers.FindAsync(id);
        if (customer == null)
        {
            throw new Exception($"Customer with id {id} not found");
        }

        context.Customers.Remove(customer);
        await context.SaveChangesAsync();
        return true;
    }
    catch (Exception ex)
    {
        throw new Exception($"Error deleting customer: {ex.Message}");
    }
}
```

### MVC Controller Implementation

```csharp
[HttpPost]
[ValidateAntiForgeryToken]
[Authorize(Roles = AppRoles.Admin)]
public async Task<IActionResult> Delete(int id)
{
    try
    {
        var deleteResult = await _customerService.DeleteCustomerAsync(id);

        if (deleteResult)
        {
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
        // ... error handling
    }
    catch (Exception ex)
    {
        // ... exception handling
    }
}
```

### Test Coverage

The deletion functionality includes comprehensive test coverage:

- **Success Scenarios**: Valid customer deletion with database verification
- **Error Scenarios**: Invalid IDs, non-existent customers, edge cases
- **Authorization Tests**: Role-based access control validation
- **Integration Tests**: End-to-end testing with in-memory database
- **Edge Cases**: Zero IDs, negative IDs, large invalid IDs

### User Experience

- **Delete Button**: Only visible to Admin users
- **Confirmation Modal**: Professional Bootstrap modal with customer details
- **Permission Denied Modal**: Clear feedback for non-Admin users
- **AJAX Support**: Seamless updates without page reload
- **Error Handling**: User-friendly error messages

## 🛠️ Getting Started

### Prerequisites

- **.NET 8 SDK** (Latest version)
- **SQL Server LocalDB** (included with Visual Studio)
- **PowerShell** (for running test scripts)
- **Visual Studio 2022** or **VS Code** (recommended)

### Installation

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd GraphQL.WebApi
   ```

2. **Setup Database**

   ```bash
   cd GraphQL.WebApi
   dotnet ef database update
   ```

3. **Run Applications**

   ```bash
   # Terminal 1: Start GraphQL API
   cd GraphQL.WebApi
   dotnet run

   # Terminal 2: Start MVC Frontend
   cd GraphQL.WebApi.Mvc
   dotnet run
   ```

4. **Access Applications**
   - **GraphQL API**: https://localhost:5001/graphql
   - **MVC Frontend**: http://localhost:5231

### Development Tools

The project includes several PowerShell scripts for development and testing:

```powershell
# Test authentication
.\test-auth.ps1

# Verify user data
.\verify-users.ps1

# Reset database
.\GraphQL.WebApi\reset-db.ps1

# Test GraphQL operations
.\GraphQL.WebApi\test-graphql.ps1
.\GraphQL.WebApi\test-mutation.ps1
```

## 🔐 Authentication & Authorization

### Demo Users

| Username  | Password     | Role    | Permissions                      |
| --------- | ------------ | ------- | -------------------------------- |
| `admin`   | `admin123`   | Admin   | Full access                      |
| `manager` | `manager123` | Manager | Customer management              |
| `user`    | `user123`    | User    | View customers, create customers |
| `guest`   | `guest123`   | Guest   | View customers only              |

### Role Permissions

| Permission      | Admin | Manager | User | Guest |
| --------------- | ----- | ------- | ---- | ----- |
| View Customers  | ✅    | ✅      | ✅   | ✅    |
| Create Customer | ✅    | ✅      | ✅   | ❌    |
| Edit Customer   | ✅    | ✅      | ❌   | ❌    |
| Delete Customer | ✅    | ❌      | ❌   | ❌    |
| View Users      | ✅    | ✅      | ❌   | ❌    |
| Create User     | ✅    | ❌      | ❌   | ❌    |
| Edit User Role  | ✅    | ❌      | ❌   | ❌    |
| Manage Roles    | ✅    | ❌      | ❌   | ❌    |

### Permission-Based Access Control

- **Edit Buttons Visible**: All users can see edit buttons
- **Client-Side Validation**: JavaScript checks permissions before opening modals
- **Professional Error Dialogs**: Modal-based permission denied messages
- **Role Display**: Shows user's current role in error messages
- **Server-Side Security**: Controller authorization prevents unauthorized access

## 🎨 UI Features

### Modal Popup Interface

- **Customer Details**: View customer information in modal dialogs
- **Customer Edit**: Edit customer data without page navigation (Admin/Manager only)
- **User Details**: View user information and permissions
- **Role Management**: Edit user roles with AJAX updates
- **Permission-Based Access**: Edit buttons visible to all users with modal error messages for unauthorized access
- **Access Denied Modal**: Professional error dialog showing user role and permission requirements

### AJAX Integration

- **Seamless Updates**: Form submissions without page reloads
- **Real-time Feedback**: Immediate success/error messages
- **Enhanced UX**: Smooth user interactions

### Enhanced Error Handling

- **Comprehensive Logging**: Detailed error tracking
- **User-friendly Messages**: Clear error communication
- **Graceful Degradation**: Fallback for failed operations
- **Debug Console Logs**: Role detection and permission checking

## 🔒 Security Features

### Password Security

- **SHA256 Hashing**: Secure password storage
- **Salt Implementation**: Enhanced security
- **Password Verification**: Secure authentication

### Session Management

- **Cookie-based Authentication**: Secure session handling
- **Configurable Timeouts**: Flexible session duration (8 hours)
- **Sliding Expiration**: Extended sessions for active users

### Access Denied Handling

- **User-friendly Pages**: Clear access denied messages
- **Role Information**: Display current user permissions
- **Navigation Options**: Easy access to authorized areas

## 🧪 Testing

### Test Projects

The solution includes comprehensive testing infrastructure:

- **GraphQL.WebApi.Tests**: Main test project with integration and unit tests

### Test Categories

- **GraphQL API Tests**: Authentication, CRUD operations, error handling
- **Customer Deletion Tests**: Comprehensive coverage of delete functionality
- **Integration Tests**: End-to-end testing with TestWebApplicationFactory
- **Unit Tests**: Individual component testing
- **Test Data Builders**: Fluent API for creating test data

### Running Tests

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test GraphQL.WebApi.Tests

# Run customer deletion tests
dotnet test GraphQL.WebApi.Tests --filter "CustomerTests.DeleteCustomer"

# Run all customer tests
dotnet test GraphQL.WebApi.Tests --filter "CustomerTests"

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

### PowerShell Test Scripts

- **`test-auth.ps1`**: Authentication testing
- **`verify-users.ps1`**: User verification scripts
- **`test-graphql.ps1`**: GraphQL API testing
- **`test-mutation.ps1`**: Mutation testing
- **`reset-db.ps1`**: Database reset utility

### Manual Testing

1. **Login with demo users** using the provided credentials
2. **Test role-based access** by accessing different features
3. **Verify customer operations** with different user roles
4. **Test customer deletion** as Admin user (only Admin can delete)
5. **Test permission denied** by logging in as User/Guest and clicking Delete buttons
6. **Test user management** as an admin user
7. **Check permission denied modals** by logging in as User/Guest and clicking Edit buttons

## 📚 Code Examples

### Customer Deletion Tests

```csharp
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
    Assert.True(errorMessage.Contains("Error") || errorMessage.Contains("Unexpected Execution Error"));
}
```

### JavaScript Role Detection

```javascript
// Get user role from server-side
const userRole = document.getElementById("userRole").value || "Guest";

function showDeleteCustomer(id, firstName, lastName) {
  // Check if user has permission to delete customers (Admin only)
  const hasDeletePermission = userRole === "Admin";

  console.log("=== DELETE CUSTOMER DEBUG ===");
  console.log("User Role:", userRole);
  console.log("Has Delete Permission:", hasDeletePermission);

  if (!hasDeletePermission) {
    // Show error message for unauthorized users
    document.getElementById("deleteUserRoleDisplay").textContent = userRole;
    const modal = new bootstrap.Modal(
      document.getElementById("deletePermissionDeniedModal")
    );
    modal.show();
    return;
  }

  // User has permission, proceed with delete confirmation
  document.getElementById("delete-customer-id").value = id;
  document.getElementById(
    "delete-customer-name"
  ).textContent = `${firstName} ${lastName}`;

  const modal = new bootstrap.Modal(
    document.getElementById("deleteCustomerModal")
  );
  modal.show();
}

function showEditCustomer(
  id,
  firstName,
  lastName,
  contact,
  email,
  dateOfBirth
) {
  // Check if user has permission to edit customers
  const hasEditPermission = userRole === "Admin" || userRole === "Manager";

  console.log("User Role:", userRole);
  console.log("Has Edit Permission:", hasEditPermission);

  if (!hasEditPermission) {
    // Show error message for unauthorized users
    document.getElementById("userRoleDisplay").textContent = userRole;
    const modal = new bootstrap.Modal(
      document.getElementById("permissionDeniedModal")
    );
    modal.show();
    return;
  }
  // ... proceed with edit modal
}
```

### Permission Denied Modal

```html
<!-- Permission Denied Modal -->
<div class="modal fade" id="permissionDeniedModal" tabindex="-1">
  <div class="modal-dialog">
    <div class="modal-content">
      <div class="modal-header bg-danger text-white">
        <h5 class="modal-title">
          <i class="bi bi-shield-exclamation"></i> Access Denied
        </h5>
      </div>
      <div class="modal-body text-center">
        <i class="bi bi-lock display-1 text-danger mb-3"></i>
        <h4 class="text-danger mb-3">Permission Required</h4>
        <p class="text-muted mb-4">
          You do not have permission to edit customers. Only
          <strong>Administrators</strong> and <strong>Managers</strong> can edit
          customer information.
        </p>
        <div class="alert alert-info">
          <i class="bi bi-info-circle"></i>
          <strong>Your Role:</strong> <span id="userRoleDisplay"></span>
        </div>
      </div>
    </div>
  </div>
</div>
```

### ExecuteQueryAsync Method

The `ExecuteQueryAsync` method in `GraphQLClient` handles HTTP communication with the GraphQL API:

```csharp
public async Task<GraphQLResponse<T>> ExecuteQueryAsync<T>(string query, object? variables = null)
{
    try
    {
        var request = new
        {
            query,
            variables
        };

        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("", content);
        var responseContent = await response.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var graphQLResponse = JsonSerializer.Deserialize<GraphQLResponse<T>>(responseContent, options);

        if (graphQLResponse?.Errors?.Any() == true)
        {
            var errorMessages = string.Join(", ", graphQLResponse.Errors.Select(e => e.Message));
            _logger.LogError("GraphQL errors: {Errors}", errorMessages);
        }

        return graphQLResponse ?? new GraphQLResponse<T>();
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error executing GraphQL query");
        throw;
    }
}
```

## 🔧 Service Layer Architecture

### GraphQL Client

- **`IGraphQLClient`**: Common interface for GraphQL communication
- **`GraphQLClient`**: HTTP client implementation with error handling
- **Response Models**: Separate classes for `GraphQLResponse`, `GraphQLData`, `GraphQLError`, `GraphQLLocation`

### Business Services

- **`ICustomerService`**: Customer-specific business logic
- **`IUserService`**: User management operations
- **`IAuthService`**: Authentication and authorization

### Separation of Concerns

- **Entity-Specific Services**: Each entity has its own service interface
- **Common GraphQL Client**: Shared HTTP client for all services
- **Clean Architecture**: Clear separation between data access and business logic

## 📊 Logging

### Comprehensive Logging Strategy

- **HTTP Client Logging**: Track GraphQL API communication
- **Service Layer Logging**: Business operation tracking
- **Controller Logging**: User action monitoring
- **Error Logging**: Detailed exception information
- **Debug Logging**: Role detection and permission checking

### Log Levels

- **Information**: Normal operations and user actions
- **Warning**: Validation failures and business rule violations
- **Error**: Exceptions and system failures
- **Debug**: Development-time debugging information

## 🚀 Deployment

### Production Considerations

- **Environment Configuration**: Use `appsettings.Production.json`
- **Database Migration**: Run `dotnet ef database update` on production
- **HTTPS Configuration**: Configure SSL certificates
- **Logging Configuration**: Set appropriate log levels
- **Security Headers**: Configure security middleware

### Docker Support

- **Multi-stage Builds**: Optimize container size
- **Health Checks**: Monitor application health
- **Environment Variables**: Configure via environment

## 📦 Dependencies

### GraphQL API Project

- **HotChocolate.AspNetCore**: 13.5.0 - GraphQL server
- **HotChocolate.Data.EntityFramework**: 13.5.0 - EF Core integration
- **Microsoft.EntityFrameworkCore**: 8.0.0 - ORM framework
- **Microsoft.EntityFrameworkCore.SqlServer**: 8.0.0 - SQL Server provider

### MVC Project

- **Microsoft.Extensions.Http**: 9.0.7 - HTTP client factory
- **Microsoft.AspNetCore.Authentication.Cookies**: 2.2.0 - Cookie authentication

### Test Projects

- **Microsoft.AspNetCore.Mvc.Testing**: 8.0.0 - Integration testing
- **Microsoft.EntityFrameworkCore.InMemory**: 8.0.0 - In-memory database
- **Moq**: 4.20.72 - Mocking framework
- **xunit**: 2.9.2 - Testing framework

## 📖 Additional Resources

- **GraphQL Documentation**: https://graphql.org/
- **HotChocolate**: https://chillicream.com/docs/hotchocolate
- **Entity Framework Core**: https://docs.microsoft.com/en-us/ef/core/
- **ASP.NET Core**: https://docs.microsoft.com/en-us/aspnet/core/

## 🔄 Recent Architecture Refactoring

### Service Organization Improvements

**Before**: All services in single namespace with project reference conflicts

```csharp
// Old structure - namespace conflicts
using GraphQL.WebApi.Model; // Caused circular dependency
```

**After**: Organized services with local role definitions

```csharp
// New structure - clean separation
namespace GraphQL.WebApi.Mvc.Services
{
    public interface ICustomerService { /* ... */ }
    public interface IUserService { /* ... */ }
    public interface IAuthService { /* ... */ }
}
```

### GraphQL Response Class Separation

**Before**: All response classes in single file

```csharp
// Old approach - everything in one file
public interface IGraphQLClient { /* ... */ }
public class GraphQLResponse { /* ... */ }
public class GraphQLData { /* ... */ }
public class GraphQLError { /* ... */ }
```

**After**: Separate files for each response class

```csharp
// New approach - single responsibility
// GraphQLResponse.cs
public class GraphQLResponse<T> { /* ... */ }

// GraphQLData.cs
public class GraphQLData { /* ... */ }

// GraphQLError.cs
public class GraphQLError { /* ... */ }

// GraphQLLocation.cs
public class GraphQLLocation { /* ... */ }
```

### Enhanced GraphQL Error Handling

**Before**: Basic error logging

```csharp
if (result?.Errors?.Any() == true)
{
    var errorMessages = string.Join(", ", result.Errors.Select(e => e.Message));
    _logger.LogError("GraphQL errors: {Errors}", errorMessages);
}
```

**After**: Detailed error location tracking

```csharp
if (result?.Errors?.Any() == true)
{
    var errorMessages = result.Errors.Select(e =>
    {
        var location = e.Locations?.FirstOrDefault();
        if (location != null)
        {
            return $"Error at line {location.Line}, column {location.Column}: {e.Message}";
        }
        return $"Error: {e.Message}";
    });
    _logger.LogError("GraphQL errors: {Errors}", string.Join("; ", errorMessages));
}
```

## 📈 Current Project Status

### ✅ Completed Features

- **GraphQL API**: Full CRUD operations for customers and users
- **Customer Deletion**: Secure deletion with Admin-only access control
- **MVC Frontend**: Complete web interface with Bootstrap 5
- **Role-Based Access Control**: Comprehensive permission system with 4 roles
- **Modal Dialogs**: Professional user interface components including delete confirmation
- **AJAX Integration**: Seamless user experience
- **Error Handling**: Comprehensive logging and user feedback
- **Permission Denied Modals**: Professional error dialogs for unauthorized actions
- **Debug Logging**: Role detection and permission checking
- **Testing Infrastructure**: Unit tests, integration tests, and comprehensive test coverage
- **Development Tools**: PowerShell scripts for testing and database management
- **Clean Architecture**: Service layer pattern with separation of concerns

### 🎯 Key Improvements

- **Service Layer Architecture**: Clean separation of concerns
- **GraphQL Response Models**: Organized and maintainable
- **Enhanced Error Handling**: Detailed error tracking with location information
- **Professional UI**: Modal-based permission denied dialogs and delete confirmation
- **Comprehensive Logging**: Debug information for development
- **Testing Coverage**: Integration and unit tests for all major components including customer deletion
- **Development Utilities**: PowerShell scripts for common development tasks
- **Customer Deletion Security**: Admin-only access with proper authorization checks

### 🔧 Technical Highlights

- **ASP.NET Core 8**: Latest framework features
- **HotChocolate GraphQL 13.5.0**: Modern GraphQL implementation
- **Entity Framework Core 8**: Advanced ORM capabilities
- **Bootstrap 5**: Modern responsive design
- **Role-Based Security**: Advanced authorization system with 4 distinct roles
- **Professional UX**: Modal dialogs and comprehensive error handling
- **Testing Infrastructure**: xUnit, Moq, and integration testing with comprehensive coverage
- **Development Tools**: PowerShell scripts for testing and database management
- **Customer Deletion**: Secure GraphQL mutation with proper error handling and database verification

### 🚧 Future Enhancements

- **API Documentation**: Swagger/OpenAPI integration
- **Performance Monitoring**: Application insights integration
- **Caching Layer**: Redis or in-memory caching
- **Background Jobs**: Hangfire or Quartz.NET integration
- **Real-time Updates**: SignalR integration for live updates
- **Advanced Security**: JWT tokens, refresh tokens, and API keys
- **Containerization**: Docker and Kubernetes support
- **CI/CD Pipeline**: GitHub Actions or Azure DevOps integration
- **Soft Delete**: Implement soft delete functionality for data recovery
- **Audit Logging**: Track all customer deletion operations
- **Bulk Operations**: Support for bulk customer deletion with confirmation

---

**Note**: This project demonstrates modern web development practices with GraphQL, ASP.NET Core, comprehensive role-based access control, and professional testing infrastructure. The architecture supports scalability and maintainability while providing an excellent user experience and robust development tooling. The customer deletion functionality showcases secure, role-based operations with comprehensive test coverage and professional user interface design.
