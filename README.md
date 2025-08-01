# GraphQL Web API with ASP.NET Core 8

A modern GraphQL API built with **ASP.NET Core 8** and **HotChocolate GraphQL** that demonstrates GraphQL implementation with Entity Framework Core and SQL Server. The solution includes both a GraphQL API and an MVC web application with full CRUD capabilities.

## 🚀 Features

- **.NET 8**: Latest framework with minimal hosting model
- **HotChocolate GraphQL**: Modern, high-performance GraphQL implementation
- **Entity Framework Core 8**: Latest ORM with improved performance
- **SQL Server LocalDB**: Local database for development
- **Banana Cake Pop**: Built-in GraphQL IDE for testing
- **MVC Web Application**: User-friendly web interface for data visualization
- **Full CRUD Operations**: Create, Read, Update, Delete customers
- **User Authentication & Authorization**: Cookie-based authentication with protected routes
- **Service Layer Architecture**: Clean separation of concerns with dedicated service interfaces
- **Nullable Reference Types**: Better null safety throughout
- **Auto Database Seeding**: Sample data automatically populated on startup
- **Enhanced Logging**: Comprehensive logging throughout the application

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

## 🚀 Getting Started

### 1. Clone and Navigate

```bash
cd GraphQL.WebApi/GraphQL.WebApi
```

### 2. Restore Packages

```bash
dotnet restore
```

### 3. Create Database

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 4. Run the GraphQL API

```bash
dotnet run
```

The GraphQL API will start on:

- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`

### 5. Run the MVC Application

In a new terminal:

```bash
cd GraphQL.WebApi.Mvc
dotnet run
```

The MVC application will start on:

- **HTTP**: `http://localhost:5002`
- **HTTPS**: `https://localhost:5231`

## 🎯 Available Applications

### **GraphQL API**

- **URL**: `https://localhost:5001/graphql`
- **Purpose**: Raw GraphQL queries and mutations
- **Banana Cake Pop IDE**: `https://localhost:5001/graphql/` (with trailing slash)

### **MVC Web Application**

- **URL**: `https://localhost:5231`
- **Purpose**: User-friendly web interface
- **Features**: Customer list, customer details, create new customers, edit customers

## 📊 Available Queries and Mutations

### 1. **Get All Customers**

```graphql
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
```

### 2. **Get Customer by ID**

```graphql
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
```

### 3. **Create New Customer**

```graphql
mutation (
  $firstName: String!
  $lastName: String!
  $contact: String!
  $email: String!
  $dateOfBirth: DateTime!
) {
  addCustomer(
    firstName: $firstName
    lastName: $lastName
    contact: $contact
    email: $email
    dateOfBirth: $dateOfBirth
  ) {
    id
    firstName
    lastName
    contact
    email
    dateOfBirth
  }
}
```

**Variables:**

```json
{
  "firstName": "John",
  "lastName": "Doe",
  "contact": "+1-555-0101",
  "email": "john.doe@email.com",
  "dateOfBirth": "1990-01-01T00:00:00.000Z"
}
```

### 4. **Update Customer**

```graphql
mutation (
  $id: Int!
  $firstName: String!
  $lastName: String!
  $contact: String!
  $email: String!
  $dateOfBirth: DateTime!
) {
  updateCustomer(
    id: $id
    firstName: $firstName
    lastName: $lastName
    contact: $contact
    email: $email
    dateOfBirth: $dateOfBirth
  ) {
    id
    firstName
    lastName
    contact
    email
    dateOfBirth
  }
}
```

**Variables:**

```json
{
  "id": 1,
  "firstName": "John Updated",
  "lastName": "Doe",
  "contact": "+1-555-0101",
  "email": "updated.john.doe@email.com",
  "dateOfBirth": "1990-01-01T00:00:00.000Z"
}
```

### 5. **User Management Queries**

```graphql
# Get all users
query {
  users {
    id
    username
    email
    firstName
    lastName
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
    isActive
    createdAt
    lastLoginAt
  }
}

# Get user by ID
query {
  userById(id: 1) {
    id
    username
    email
    firstName
    lastName
    isActive
    createdAt
    lastLoginAt
  }
}
```

### 6. **User Management Mutations**

```graphql
# Create new user
mutation (
  $username: String!
  $email: String!
  $password: String!
  $firstName: String!
  $lastName: String!
) {
  addUser(
    username: $username
    email: $email
    password: $password
    firstName: $firstName
    lastName: $lastName
  ) {
    id
    username
    email
    firstName
    lastName
    isActive
    createdAt
  }
}

# Update user last login
mutation {
  updateUserLastLogin(id: 1) {
    id
    username
    lastLoginAt
  }
}
```

## 🗄️ Database

### **Connection Details**

- **Server**: `(localdb)\mssqllocaldb`
- **Database**: `jqueryDb`
- **Connection String**: Configured in `appsettings.json`

### **Sample Data**

The application automatically seeds the database with sample data on startup:

**Customers (12 sample records):**

- John Doe, Jane Smith, Michael Johnson, Sarah Williams, David Brown
- Emily Davis, Robert Wilson, Lisa Anderson, James Taylor, Amanda Martinez
- Plus additional customers for testing

**Users (2 demo accounts):**

- **Admin**: `admin` / `admin123`
- **User**: `user` / `user123`

### **Database Schema**

```sql
-- Customers Table
CREATE TABLE [Customers] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Contact] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);

-- Users Table
CREATE TABLE [Users] (
    [Id] int NOT NULL IDENTITY,
    [Username] nvarchar(50) NOT NULL,
    [Email] nvarchar(100) NOT NULL,
    [PasswordHash] nvarchar(100) NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [IsActive] bit NOT NULL DEFAULT 1,
    [CreatedAt] datetime2 NOT NULL DEFAULT GETDATE(),
    [LastLoginAt] datetime2 NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY ([Id])
);
```

## 🏗️ Project Structure

```
GraphQL.WebApi/
├── GraphQL.WebApi/              # GraphQL API Project
│   ├── Data/
│   │   ├── ApplicationDbContext.cs    # EF Core DbContext
│   │   └── DbInitializer.cs          # Database seeding
│   ├── GraphQL/
│   │   ├── Query.cs                  # GraphQL queries
│   │   └── Mutation.cs               # GraphQL mutations
│   ├── Model/
│   │   ├── Customer.cs               # Customer entity
│   │   └── User.cs                   # User entity
│   ├── Program.cs                    # Application entry point
│   ├── appsettings.json              # Configuration
│   └── seed-data.sql                 # Manual SQL seeding script
├── GraphQL.WebApi.Mvc/           # MVC Web Application
│   ├── Controllers/
│   │   ├── HomeController.cs         # Home page controller
│   │   ├── CustomersController.cs    # Customer operations (CRUD)
│   │   └── AccountController.cs      # Authentication controller
│   ├── Models/
│   │   ├── Customer.cs               # Customer model
│   │   └── User.cs                   # User and auth models
│   ├── Services/
│   │   ├── IGraphQLService.cs        # GraphQL service interface
│   │   ├── GraphQLService.cs         # GraphQL client implementation
│   │   ├── ICustomerService.cs       # Customer service interface
│   │   ├── CustomerService.cs        # Customer service implementation
│   │   ├── IAuthService.cs           # Authentication service interface
│   │   └── AuthService.cs            # Authentication service implementation
│   ├── Views/
│   │   ├── Home/
│   │   │   └── Index.cshtml          # Home page
│   │   ├── Account/
│   │   │   ├── Login.cshtml          # Login form
│   │   │   └── Register.cshtml       # Registration form
│   │   └── Customers/
│   │       ├── Index.cshtml          # Customer list with modals
│   │       └── Create.cshtml         # Create customer form
│   └── Program.cs                    # MVC application entry point
└── GraphQL.WebApi.sln             # Solution file
```

## 🏗️ Architecture

### **Service Layer Pattern**

The MVC application follows a clean service layer architecture:

```
Controllers → Service Interfaces → Service Implementations → GraphQL API
```

**Key Components:**

- **`ICustomerService`**: Business logic for customer operations
- **`CustomerService`**: Implementation that wraps `IGraphQLService` calls
- **`IAuthService`**: Authentication and user management
- **`AuthService`**: Implementation with database-backed user validation
- **`IGraphQLService`**: Low-level GraphQL client interface
- **`GraphQLService`**: HTTP client implementation for GraphQL API communication

**Benefits:**

- ✅ **Separation of Concerns**: Controllers focus on HTTP, services handle business logic
- ✅ **Testability**: Easy to mock service interfaces for unit testing
- ✅ **Maintainability**: Clear boundaries between layers
- ✅ **Logging**: Enhanced logging at service layer for better debugging
- ✅ **Flexibility**: Can easily swap implementations without changing controllers

## 🔧 Configuration

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=jqueryDb;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  }
}
```

### **GraphQL Configuration**

```csharp
builder.Services
    .AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
```

### **MVC Service Layer Configuration**

```csharp
// GraphQL Client Configuration
builder.Services.AddHttpClient<IGraphQLService, GraphQLService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/graphql");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
});

// Service Layer Registration
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();

// Authentication Configuration
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorization();
```

## 🧪 Testing

### **Using Banana Cake Pop**

1. Navigate to `https://localhost:5001/graphql/`
2. Use the interactive interface to test queries and mutations
3. Explore the schema documentation

### **Using MVC Web Application**

1. Navigate to `https://localhost:5231`
2. **Login** with demo credentials:
   - Username: `admin` / Password: `admin123`
   - Or Username: `user` / Password: `user123`
3. Click on "Customers" in the navigation
4. View customer list with modal dialogs for details and editing
5. Click "Create New Customer" to add new customers
6. Click the "View Details" or "Edit" buttons to use modal dialogs
7. Use the "Logout" link to sign out

### **Using curl**

```bash
# Query all customers
curl -X POST https://localhost:5001/graphql \
  -H "Content-Type: application/json" \
  -d '{"query":"query { customers { id firstName lastName } }"}' \
  -k

# Create new customer
curl -X POST https://localhost:5001/graphql \
  -H "Content-Type: application/json" \
  -d '{"query":"mutation($firstName: String!, $lastName: String!, $contact: String!, $email: String!, $dateOfBirth: DateTime!) { addCustomer(firstName: $firstName, lastName: $lastName, contact: $contact, email: $email, dateOfBirth: $dateOfBirth) { id firstName lastName } }","variables":{"firstName":"Test","lastName":"Customer","contact":"+1-555-9999","email":"test@email.com","dateOfBirth":"1990-01-01T00:00:00.000Z"}}' \
  -k

# Update customer
curl -X POST https://localhost:5001/graphql \
  -H "Content-Type: application/json" \
  -d '{"query":"mutation($id: Int!, $firstName: String!, $lastName: String!, $contact: String!, $email: String!, $dateOfBirth: DateTime!) { updateCustomer(id: $id, firstName: $firstName, lastName: $lastName, contact: $contact, email: $email, dateOfBirth: $dateOfBirth) { id firstName lastName } }","variables":{"id":1,"firstName":"Updated","lastName":"Customer","contact":"+1-555-9999","email":"updated@email.com","dateOfBirth":"1990-01-01T00:00:00.000Z"}}' \
  -k
```

### **Using PowerShell Test Scripts**

```powershell
# Test queries
powershell -ExecutionPolicy Bypass -File test-graphql.ps1

# Test mutations
powershell -ExecutionPolicy Bypass -File test-mutation.ps1

# Test updates
powershell -ExecutionPolicy Bypass -File test-update.ps1
```

## 🔍 Troubleshooting

### **Common Issues**

1. **"Couldn't find a project to run"**

   - Ensure you're in the correct directory: `GraphQL.WebApi/GraphQL.WebApi`

2. **Database connection errors**

   - Verify LocalDB is running: `sqllocaldb start "MSSQLLocalDB"`
   - Check connection string in `appsettings.json`

3. **GraphQL errors**

   - Check application logs for detailed error messages
   - Verify database has been seeded with sample data

4. **MVC application can't connect to GraphQL API**

   - Ensure GraphQL API is running on port 5001 (HTTPS)
   - Check the base address in `Program.cs`
   - SSL certificate issues are handled automatically in development

5. **SSL Certificate Issues**

   - The MVC application is configured to ignore SSL certificate validation for development
   - Use HTTPS endpoints: `https://localhost:5001/graphql` and `https://localhost:5231`

6. **Port conflicts**

   - Change ports in `Properties/launchSettings.json`
   - Or kill processes using the required ports

7. **Mutation errors**

   - Ensure all required fields are provided
   - Check date format (ISO 8601 format required)
   - Verify GraphQL API includes mutation type in configuration

8. **Update errors**
   - Ensure the customer ID exists in the database
   - Check that all required fields are provided
   - Verify the customer exists before attempting to update

### **Database Management**

**View data in SSMS:**

- Connect to: `(localdb)\mssqllocaldb`
- Database: `jqueryDb`
- Table: `Customers`

**Manual seeding:**

- Run `seed-data.sql` in SSMS if auto-seeding fails

## 📚 Learning Resources

- **GraphQL**: [GraphQL Official Documentation](https://graphql.org/)
- **HotChocolate**: [HotChocolate Documentation](https://chillicream.com/docs/hotchocolate)
- **Entity Framework**: [EF Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- **ASP.NET Core**: [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)

## 🎯 Key Improvements from .NET Core 3.1

- ✅ **Upgraded to .NET 8**
- ✅ **Switched to HotChocolate GraphQL**
- ✅ **Implemented minimal hosting model**
- ✅ **Added nullable reference types**
- ✅ **Enhanced error handling**
- ✅ **Auto database seeding**
- ✅ **Modern GraphQL IDE integration**
- ✅ **Added MVC web application**
- ✅ **GraphQL client integration**
- ✅ **SSL certificate handling for development**
- ✅ **Added GraphQL mutations**
- ✅ **Full CRUD operations via MVC**
- ✅ **Update functionality for customers**
- ✅ **User authentication and authorization**
- ✅ **Service layer architecture with ICustomerService**
- ✅ **Enhanced logging throughout the application**
- ✅ **Modal dialogs for better UX**
- ✅ **Cookie-based authentication system**
- ✅ **Protected routes and authorization**

## 📄 License

This project is for educational purposes. Feel free to use and modify as needed.

---

**Happy GraphQLing! 🚀**
