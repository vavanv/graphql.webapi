# GraphQL Web API with ASP.NET Core 8

A modern GraphQL API built with **ASP.NET Core 8** and **HotChocolate GraphQL** that demonstrates GraphQL implementation with Entity Framework Core and SQL Server. The solution includes both a GraphQL API and an MVC web application with full CRUD capabilities and **Role-Based Access Control (RBAC)**.

## 🚀 Features

- **.NET 8**: Latest framework with minimal hosting model
- **HotChocolate GraphQL**: Modern, high-performance GraphQL implementation
- **Entity Framework Core 8**: Latest ORM with improved performance
- **SQL Server LocalDB**: Local database for development
- **Banana Cake Pop**: Built-in GraphQL IDE for testing
- **MVC Web Application**: User-friendly web interface for data visualization
- **Full CRUD Operations**: Create, Read, Update, Delete customers
- **User Authentication & Authorization**: Cookie-based authentication with protected routes
- **Role-Based Access Control (RBAC)**: Advanced authorization with role-based permissions
- **Clean Architecture**: Separated service layer with dedicated interfaces per entity
- **Common GraphQL Client**: Reusable HTTP client for GraphQL communication
- **Single Responsibility Principle**: Each service handles one specific domain
- **Nullable Reference Types**: Better null safety throughout
- **Auto Database Seeding**: Sample data automatically populated on startup
- **Enhanced Logging**: Comprehensive logging throughout the application
- **Organized Service Layer**: Services organized by domain (Auth, Customer, User, GraphQL)
- **Resolved Namespace Conflicts**: Clean service organization with proper type resolution

## 🔐 Role-Based Access Control (RBAC)

The application implements a comprehensive RBAC system with the following roles and permissions:

### Roles
- **Admin**: Full system access and user management
- **Manager**: Customer management and limited user viewing
- **User**: Basic customer operations (view and create)
- **Guest**: Read-only access to customers

### Permissions by Role

| Permission | Admin | Manager | User | Guest |
|------------|-------|---------|------|-------|
| View Customers | ✅ | ✅ | ✅ | ✅ |
| Create Customer | ✅ | ✅ | ✅ | ❌ |
| Edit Customer | ✅ | ✅ | ❌ | ❌ |
| Delete Customer | ✅ | ❌ | ❌ | ❌ |
| View Users | ✅ | ✅ | ❌ | ❌ |
| Create User | ✅ | ❌ | ❌ | ❌ |
| Edit User Role | ✅ | ❌ | ❌ | ❌ |
| Manage Roles | ✅ | ❌ | ❌ | ❌ |

### Demo Users
The system comes with pre-configured demo users:

| Username | Password | Role | Access Level |
|----------|----------|------|-------------|
| `admin` | `admin123` | Admin | Full system access |
| `manager` | `manager123` | Manager | Customer management |
| `user` | `user123` | User | Basic operations |
| `guest` | `guest123` | Guest | Read-only access |

## 📋 Prerequisites

- **.NET 8 SDK** installed
- **SQL Server LocalDB** (usually comes with Visual Studio)
- **Visual Studio 2022** or **VS Code** (recommended)

## 🛠️ Technology Stack

- **.NET 8**
- **Entity Framework Core 8**
- **HotChocolate GraphQL 13.5.0**
- **SQL Server LocalDB**
- **Banana Cake Pop IDE**
- **ASP.NET Core MVC**
- **Role-Based Authorization**

## 🏗️ Project Structure

### GraphQL API (`GraphQL.WebApi/`)

```
GraphQL.WebApi/
├── Data/
│   ├── ApplicationDbContext.cs
│   └── DbInitializer.cs
├── GraphQL/
│   ├── Query.cs
│   └── Mutation.cs
├── Model/
│   ├── Customer.cs
│   ├── User.cs
│   └── AppRoles.cs
├── Migrations/
├── Program.cs
└── appsettings.json
```

### MVC Application (`GraphQL.WebApi.Mvc/`)

```
GraphQL.WebApi.Mvc/
├── Controllers/
│   ├── AccountController.cs
│   ├── CustomersController.cs
│   ├── HomeController.cs
│   └── UsersController.cs
├── Models/
│   ├── Customer.cs
│   └── User.cs (includes LoginViewModel, RegisterViewModel)
├── Services/
│   ├── Auth/
│   │   ├── IAuthService.cs
│   │   └── AuthService.cs
│   ├── Customer/
│   │   ├── ICustomerService.cs
│   │   └── CustomerService.cs
│   ├── GraphQL/
│   │   ├── IGraphQLClient.cs
│   │   ├── GraphQLClient.cs
│   │   ├── GraphQLResponse.cs
│   │   ├── GraphQLData.cs
│   │   ├── GraphQLError.cs
│   │   └── GraphQLLocation.cs
│   └── User/
│       ├── IUserService.cs
│       └── UserService.cs
├── Views/
├── Program.cs
└── appsettings.json
```

## 🚀 Getting Started

### 1. Clone and Navigate

```bash
cd GraphQL.WebApi/GraphQL.WebApi
```

### 2. Database Setup

```bash
# Create and apply migrations
dotnet ef database update

# The database will be automatically seeded with:
# - 10 sample customers
# - 4 demo users with different roles
```

### 3. Run the GraphQL API

```bash
dotnet run
```

The API will be available at:
- **GraphQL Endpoint**: `https://localhost:5001/graphql`
- **Banana Cake Pop IDE**: `https://localhost:5001/graphql/`

### 4. Run the MVC Application

In a new terminal:

```bash
cd GraphQL.WebApi.Mvc
dotnet run
```

The MVC app will be available at:
- **Web Interface**: `http://localhost:5231`

## 🔐 Authentication & Authorization

### Login with Demo Users

1. Navigate to `http://localhost:5231`
2. Click "Login" in the navigation
3. Use any of the demo credentials:

| Username | Password | Role | Features Available |
|----------|----------|------|-------------------|
| `admin` | `admin123` | Admin | All features + User management |
| `manager` | `manager123` | Manager | Customer management |
| `user` | `user123` | User | View and create customers |
| `guest` | `guest123` | Guest | View customers only |

### Role-Based Features

#### Admin Role
- ✅ View all customers
- ✅ Create new customers
- ✅ Edit existing customers
- ✅ Delete customers
- ✅ Manage users (view, create, edit roles)
- ✅ Access to "Admin" dropdown in navigation

#### Manager Role
- ✅ View all customers
- ✅ Create new customers
- ✅ Edit existing customers
- ❌ Delete customers
- ✅ View users (read-only)
- ❌ Manage users

#### User Role
- ✅ View all customers
- ✅ Create new customers
- ❌ Edit customers
- ❌ Delete customers
- ❌ Access user management

#### Guest Role
- ✅ View all customers
- ❌ Create customers
- ❌ Edit customers
- ❌ Delete customers
- ❌ Access user management

## 📊 GraphQL Queries & Mutations

### Queries

```graphql
# Get all customers
query {
  customers {
    id
    firstName
    lastName
    contact
    email
    dateOfBirth
  }
}

# Get customer by ID
query {
  customer(id: 1) {
    id
    firstName
    lastName
    contact
    email
    dateOfBirth
  }
}

# Get all users (Admin only)
query {
  users {
    id
    username
    email
    firstName
    lastName
    role
    isActive
    createdAt
    lastLoginAt
  }
}

# Get user by username
query {
  user(username: "admin") {
    id
    username
    email
    firstName
    lastName
    role
    isActive
    createdAt
    lastLoginAt
  }
}

# Get available roles
query {
  roles
}
```

### Mutations

```graphql
# Create customer
mutation {
  addCustomer(
    firstName: "John"
    lastName: "Doe"
    contact: "+1-555-0123"
    email: "john.doe@example.com"
    dateOfBirth: "1990-01-01"
  ) {
    id
    firstName
    lastName
    contact
    email
    dateOfBirth
  }
}

# Update customer
mutation {
  updateCustomer(
    id: 1
    firstName: "John"
    lastName: "Smith"
    contact: "+1-555-0123"
    email: "john.smith@example.com"
    dateOfBirth: "1990-01-01"
  ) {
    id
    firstName
    lastName
    contact
    email
    dateOfBirth
  }
}

# Create user (Admin only)
mutation {
  addUser(
    username: "newuser"
    email: "newuser@example.com"
    password: "password123"
    firstName: "New"
    lastName: "User"
    role: "User"
  ) {
    id
    username
    email
    firstName
    lastName
    role
    isActive
    createdAt
  }
}

# Update user role (Admin only)
mutation {
  updateUserRole(id: 1, role: "Manager") {
    id
    username
    email
    firstName
    lastName
    role
    isActive
    createdAt
  }
}
```

## 🛡️ Security Features

### Authentication
- **Cookie-based authentication** with secure settings
- **Password hashing** using SHA256
- **Session management** with configurable expiration
- **Remember me** functionality

### Authorization
- **Role-based access control** with granular permissions
- **Controller-level authorization** using `[Authorize(Roles = "...")]`
- **Action-level authorization** for fine-grained control
- **UI-based role filtering** showing/hiding features based on user role

### Data Protection
- **Model binding protection** using `[Bind]` attributes
- **CSRF protection** with anti-forgery tokens
- **Input validation** with data annotations
- **SQL injection prevention** through Entity Framework Core

## 🔧 Configuration

### Connection Strings

The application uses SQL Server LocalDB by default:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=jqueryDb;Trusted_Connection=true;TrustServerCertificate=true"
  }
}
```

### GraphQL Settings

```json
{
  "GraphQL": {
    "ExposeExceptions": true
  }
}
```

## 🧪 Testing

### Manual Testing

1. **Start both applications** (GraphQL API and MVC)
2. **Login with different roles** to test authorization
3. **Navigate through the application** to verify role-based access
4. **Test GraphQL queries** in Banana Cake Pop IDE
5. **Verify database seeding** with demo data

### Automated Testing

```bash
# Build the solution
dotnet build

# Run tests (if any)
dotnet test
```

## 📝 Logging

The application includes comprehensive logging:

- **Authentication events** (login, logout, failed attempts)
- **Authorization decisions** (access granted/denied)
- **GraphQL operations** (queries, mutations, errors)
- **Database operations** (seeding, migrations)
- **User management** (creation, role updates)

## 🚀 Deployment

### Development
```bash
# GraphQL API
cd GraphQL.WebApi
dotnet run

# MVC Application
cd GraphQL.WebApi.Mvc
dotnet run
```

### Production
```bash
# Build for production
dotnet publish -c Release

# Deploy to your preferred hosting platform
```

## 🔄 Database Migrations

```bash
# Create new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Remove last migration
dotnet ef migrations remove
```

## 📚 Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [HotChocolate GraphQL Documentation](https://chillicream.com/docs/hotchocolate)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [GraphQL Specification](https://graphql.org/learn/)

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add tests if applicable
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License.

---

**Note**: This is a demonstration project showcasing GraphQL implementation with ASP.NET Core 8 and role-based access control. For production use, consider implementing additional security measures such as HTTPS enforcement, rate limiting, and more robust password hashing algorithms.
