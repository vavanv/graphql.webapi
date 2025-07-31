# GraphQL Web API with ASP.NET Core 8

A modern GraphQL API built with **ASP.NET Core 8** and **HotChocolate GraphQL** that demonstrates GraphQL implementation with Entity Framework Core and SQL Server.

## 🚀 Features

- **.NET 8**: Latest framework with minimal hosting model
- **HotChocolate GraphQL**: Modern, high-performance GraphQL implementation
- **Entity Framework Core 8**: Latest ORM with improved performance
- **SQL Server LocalDB**: Local database for development
- **Banana Cake Pop**: Built-in GraphQL IDE for testing
- **Nullable Reference Types**: Better null safety throughout
- **Auto Database Seeding**: Sample data automatically populated on startup

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

### 4. Run the Application

```bash
dotnet run
```

The application will start on:

- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`

## 🎯 GraphQL Endpoints

### **GraphQL Endpoint**

- **URL**: `http://localhost:5000/graphql`
- **Purpose**: Raw GraphQL queries

### **Banana Cake Pop IDE**

- **URL**: `http://localhost:5000/graphql/` (with trailing slash)
- **Purpose**: Interactive GraphQL testing interface

## 📊 Available Queries

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

## 🗄️ Database

### **Connection Details**

- **Server**: `(localdb)\mssqllocaldb`
- **Database**: `jqueryDb`
- **Connection String**: Configured in `appsettings.json`

### **Sample Data**

The application automatically seeds the database with 10 sample customers on startup:

- John Doe, Jane Smith, Michael Johnson, Sarah Williams, David Brown
- Emily Davis, Robert Wilson, Lisa Anderson, James Taylor, Amanda Martinez

### **Database Schema**

```sql
CREATE TABLE [Customers] (
    [Id] int NOT NULL IDENTITY,
    [FirstName] nvarchar(max) NOT NULL,
    [LastName] nvarchar(max) NOT NULL,
    [Contact] nvarchar(max) NOT NULL,
    [Email] nvarchar(max) NOT NULL,
    [DateOfBirth] datetime2 NOT NULL,
    CONSTRAINT [PK_Customers] PRIMARY KEY ([Id])
);
```

## 🏗️ Project Structure

```
GraphQL.WebApi/
├── Data/
│   ├── ApplicationDbContext.cs    # EF Core DbContext
│   └── DbInitializer.cs          # Database seeding
├── GraphQL/
│   └── Query.cs                  # GraphQL queries
├── Model/
│   └── Customer.cs               # Customer entity
├── Program.cs                    # Application entry point
├── appsettings.json              # Configuration
└── seed-data.sql                 # Manual SQL seeding script
```

## 🔧 Configuration

### **Database Connection**

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
    .ModifyRequestOptions(opt => opt.IncludeExceptionDetails = true);
```

## 🧪 Testing

### **Using Banana Cake Pop**

1. Navigate to `http://localhost:5000/graphql/`
2. Use the interactive interface to test queries
3. Explore the schema documentation

### **Using curl**

```bash
curl -X POST http://localhost:5000/graphql \
  -H "Content-Type: application/json" \
  -d '{"query":"query { customers { id firstName lastName } }"}'
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

4. **Port conflicts**
   - Change ports in `Properties/launchSettings.json`
   - Or kill processes using ports 5000/5001

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

## 📄 License

This project is for educational purposes. Feel free to use and modify as needed.

---

**Happy GraphQLing! 🚀**
