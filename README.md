# Getting Started with GraphQL in ASP.NET Core 8

GraphQL or Graph Query Language is an API Standard that was invented and open-sourced by Facebook. It basically is an alternative to REST APIs. GraphQL unlike the traditional REST API, gives the control to the Client so that the Client App / user gets to request for the specific data he wants.

This project demonstrates GraphQL implementation using **.NET 8** with modern features including:

- Minimal hosting model
- Nullable reference types
- Implicit usings
- HotChocolate GraphQL implementation
- Entity Framework Core 8

## Topics Covered

1. What is GraphQL?
2. The Problem GraphQL Solves
3. GraphQL vs REST API
4. Types in GraphQL
5. GraphQL Schema
6. About GraphQL Playground
7. Testing GraphQL
8. Getting all the customers.
9. Get a Customer by ID

## Technology Stack

- **.NET 8**
- **Entity Framework Core 8**
- **HotChocolate GraphQL 13.5.0**
- **SQL Server LocalDB**

## Getting Started

1. Ensure you have .NET 8 SDK installed
2. Run `dotnet restore` to restore packages
3. Run `dotnet ef migrations add InitialCreate` to create database migrations
4. Run `dotnet ef database update` to create the database
5. Run `dotnet run` to start the application
6. Navigate to `/graphql` to access the GraphQL endpoint
7. Navigate to `/graphql/` (with trailing slash) to access Banana Cake Pop (HotChocolate's GraphQL IDE)

## Available GraphQL Queries

1. **Get All Customers**:

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

2. **Get Customer by ID**:
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

## Features

- **Modern .NET 8**: Uses the latest .NET 8 features
- **HotChocolate GraphQL**: Modern, high-performance GraphQL implementation
- **Entity Framework Core 8**: Latest ORM with improved performance
- **Nullable Reference Types**: Better null safety
- **Minimal API Pattern**: Clean, modern hosting model

Read the entire blog here - https://www.codewithmukesh.com/blog/graphql-in-aspnet-core/
"# grapql.webapi" 
