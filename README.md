# GraphQL Web API with ASP.NET Core 8

A modern GraphQL API built with ASP.NET Core 8, HotChocolate GraphQL, Entity Framework Core, and a comprehensive MVC frontend with Role-Based Access Control (RBAC).

## 🚀 Features

- **GraphQL API**: HotChocolate GraphQL server with queries and mutations
- **Entity Framework Core 8**: Modern ORM with SQL Server LocalDB
- **ASP.NET Core MVC**: Rich web interface with Bootstrap 5
- **Role-Based Access Control (RBAC)**: Advanced authorization system
- **Modal Dialogs**: Interactive customer and user management
- **AJAX Integration**: Seamless updates without page reloads
- **Professional Error Handling**: Modal-based permission denied dialogs
- **Comprehensive Logging**: Detailed error tracking and debugging

## 🏗️ Project Structure

```
GraphQL.WebApi/
├── GraphQL.WebApi/                 # GraphQL API Project
│   ├── Data/                       # Database context and seeding
│   ├── GraphQL/                    # GraphQL queries and mutations
│   ├── Model/                      # Entity models
│   └── Program.cs                  # API configuration
├── GraphQL.WebApi.Mvc/             # MVC Frontend Project
│   ├── Controllers/                # MVC controllers
│   ├── Models/                     # MVC-specific models
│   ├── Services/                   # Business logic services
│   │   ├── GraphQL/               # GraphQL client and response models
│   │   ├── Customer/              # Customer-specific services
│   │   ├── User/                  # User management services
│   │   └── Auth/                  # Authentication services
│   ├── Views/                     # Razor views with modals
│   └── Program.cs                 # MVC configuration
└── README.md                      # This documentation
```

## 🛠️ Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server LocalDB (included with Visual Studio)

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
- **Configurable Timeouts**: Flexible session duration
- **Sliding Expiration**: Extended sessions for active users

### Access Denied Handling

- **User-friendly Pages**: Clear access denied messages
- **Role Information**: Display current user permissions
- **Navigation Options**: Easy access to authorized areas

## 🧪 Testing

### PowerShell Test Scripts

- **`test-auth.ps1`**: Authentication testing
- **`verify-users.ps1`**: User verification scripts

### Manual Testing

1. **Login with demo users** using the provided credentials
2. **Test role-based access** by accessing different features
3. **Verify customer operations** with different user roles
4. **Test user management** as an admin user
5. **Check permission denied modals** by logging in as User/Guest and clicking Edit buttons

## 📚 Code Examples

### JavaScript Role Detection

```javascript
// Get user role from server-side
const userRole =
  '@User.Claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value' ||
  "Guest";

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
- **MVC Frontend**: Complete web interface with Bootstrap 5
- **Role-Based Access Control**: Comprehensive permission system
- **Modal Dialogs**: Professional user interface components
- **AJAX Integration**: Seamless user experience
- **Error Handling**: Comprehensive logging and user feedback
- **Permission Denied Modals**: Professional error dialogs
- **Debug Logging**: Role detection and permission checking

### 🎯 Key Improvements

- **Service Layer Architecture**: Clean separation of concerns
- **GraphQL Response Models**: Organized and maintainable
- **Enhanced Error Handling**: Detailed error tracking
- **Professional UI**: Modal-based permission denied dialogs
- **Comprehensive Logging**: Debug information for development

### 🔧 Technical Highlights

- **ASP.NET Core 8**: Latest framework features
- **HotChocolate GraphQL**: Modern GraphQL implementation
- **Entity Framework Core 8**: Advanced ORM capabilities
- **Bootstrap 5**: Modern responsive design
- **Role-Based Security**: Advanced authorization system
- **Professional UX**: Modal dialogs and error handling

---

**Note**: This project demonstrates modern web development practices with GraphQL, ASP.NET Core, and comprehensive role-based access control. The architecture supports scalability and maintainability while providing an excellent user experience.
