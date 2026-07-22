# Repository Abstraction Pattern for Mock Testing

## Overview

This guide shows how to implement testable repositories that support both production (SQL Server stored procedures) and testing (in-memory) without database dependencies.

## Completed Example: Product Repository

We've implemented the pattern for **Product** as a template. The pattern includes:

1. **IProductRepository** — Interface contract (already existed)
2. **SqlProductRepository** — Production implementation using stored procedures
3. **InMemoryProductRepository** — Test implementation with in-memory data

## Architecture

```
┌─────────────────────┐
│   Razor Pages       │
│   ProductsPage      │
└──────────┬──────────┘
           │
           ↓ depends on
┌─────────────────────────────────┐
│    IProductRepository           │
│         (interface)             │
└──────┬──────────────────┬───────┘
       │                  │
       ↓                  ↓
┌──────────────────┐  ┌────────────────────┐
│ SqlProduct       │  │ InMemoryProduct    │
│ Repository       │  │ Repository         │
│                  │  │                    │
│ (production)     │  │ (testing)          │
│ Uses:            │  │ Uses:              │
│ - DbContext      │  │ - List<Product>    │
│ - Stored procs   │  │ - In-memory data   │
└──────────────────┘  └────────────────────┘
```

## How to Extend to Other Services

Follow these steps for each of the remaining 9 services (Address, Customer, etc.):

### Step 1: Create SqlXxxRepository

Copy the pattern from `SqlProductRepository.cs`:

- **File name**: `Sql[ServiceName]Repository.cs`
- **Namespace**: `WhatPlantsCrave.Infrastructure.Repositories`
- **Inherits**: `I[ServiceName]Repository`
- **Constructor**: Takes `AdventureWorksContext`
- **Methods**: Copy stored procedure logic from the service

**Example for Address:**

```csharp
using Brawndo_Components.Data;
using Brawndo_Components.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    public class SqlAddressRepository : IAddressRepository
    {
        private readonly AdventureWorksContext _context;

        public SqlAddressRepository(AdventureWorksContext context)
        {
            _context = context;
        }

        public int Create(string addressLine1, string city, string stateProvince, string countryRegion, string postalCode, string? addressLine2 = null)
        {
            try
            {
                using var connection = new SqlConnection(_context.Database.GetConnectionString());
                connection.Open();
                using var command = new SqlCommand("SalesLT.usp_Address_Create", connection);
                // ... (copy the logic from AddressService.Create)
            }
            catch (Exception)
            {
                return -1;
            }
        }

        // ... (copy all other methods from AddressService)
    }
}
```

### Step 2: Create InMemoryXxxRepository

Copy the pattern from `InMemoryProductRepository.cs`:

- **File name**: `InMemory[ServiceName]Repository.cs`
- **Namespace**: `WhatPlantsCrave.Infrastructure.Repositories`
- **Inherits**: `I[ServiceName]Repository`
- **Data storage**: `List<T>` with in-memory LINQ operations

**Example for Address:**

```csharp
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    public class InMemoryAddressRepository : IAddressRepository
    {
        private readonly List<Address> _addresses = new();
        private int _nextId = 1;

        public int Create(string addressLine1, string city, string stateProvince, string countryRegion, string postalCode, string? addressLine2 = null)
        {
            var address = new Address
            {
                AddressId = _nextId++,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                City = city,
                StateProvince = stateProvince,
                CountryRegion = countryRegion,
                PostalCode = postalCode,
                Rowguid = Guid.NewGuid(),
                ModifiedDate = DateTime.UtcNow
            };
            _addresses.Add(address);
            return address.AddressId;
        }

        // ... (implement all other methods using LINQ over _addresses list)
    }
}
```

### Step 3: Update Program.cs

Register the Sql*Repository implementations:

```csharp
// For each service, add:
builder.Services.AddScoped<I[ServiceName]Repository, Sql[ServiceName]Repository>();

// Example:
builder.Services.AddScoped<IAddressRepository, SqlAddressRepository>();
builder.Services.AddScoped<ICustomerRepository, SqlCustomerRepository>();
// ... etc for all 9 services
```

## Using in Production

```csharp
// Program.cs automatically wires up SQL repositories
builder.Services.AddScoped<IProductRepository, SqlProductRepository>();

// Razorpages/controllers automatically get the SQL implementation
public class ProductsPage : PageModel
{
    private readonly IProductRepository _repository;
    
    public ProductsPage(IProductRepository repository)
    {
        _repository = repository;  // Gets SqlProductRepository
    }
}
```

## Using in Tests

```csharp
[TestClass]
public class ProductServiceTests
{
    [TestMethod]
    public void GetAll_ReturnsAllProducts()
    {
        // Arrange - use in-memory repository (no database!)
        var repository = new InMemoryProductRepository();
        repository.Create("Widget", "WDG-001", 10m, 20m, DateTime.Now);
        repository.Create("Gadget", "GDG-001", 15m, 30m, DateTime.Now);

        // Act
        var products = repository.GetAll();

        // Assert
        Assert.AreEqual(2, products.Count);
        Assert.AreEqual("Widget", products[0].Name);
    }

    [TestMethod]
    public void Create_AddsProductAndReturnsId()
    {
        // Arrange
        var repository = new InMemoryProductRepository();

        // Act
        var id = repository.Create("Test", "TST-001", 5m, 10m, DateTime.Now);

        // Assert
        Assert.IsTrue(id > 0);
        var product = repository.GetByID(id);
        Assert.IsNotNull(product);
        Assert.AreEqual("Test", product.Name);
    }
}
```

## Switching Between Implementations

### For Production (uses real database):
```csharp
builder.Services.AddScoped<IProductRepository, SqlProductRepository>();
```

### For Testing (no database):
```csharp
// In test setup, create the in-memory version directly
var repository = new InMemoryProductRepository();
```

## Services Remaining to Implement

Apply the pattern above to these 9 services:

1. **Address** — IAddressRepository, SqlAddressRepository, InMemoryAddressRepository
2. **Customer** — ICustomerRepository, SqlCustomerRepository, InMemoryCustomerRepository
3. **CustomerAddress** — ICustomerAddressRepository, SqlCustomerAddressRepository, InMemoryCustomerAddressRepository
4. **ProductCategory** — IProductCategoryRepository, SqlProductCategoryRepository, InMemoryProductCategoryRepository
5. **ProductDescription** — IProductDescriptionRepository, SqlProductDescriptionRepository, InMemoryProductDescriptionRepository
6. **ProductModel** — IProductModelRepository, SqlProductModelRepository, InMemoryProductModelRepository
7. **ProductModelProductDescription** — IProductModelProductDescriptionRepository, SqlProductModelProductDescriptionRepository, InMemoryProductModelProductDescriptionRepository
8. **SalesOrderDetail** — ISalesOrderDetailRepository, SqlSalesOrderDetailRepository, InMemorySalesOrderDetailRepository
9. **SalesOrderHeader** — ISalesOrderHeaderRepository, SqlSalesOrderHeaderRepository, InMemorySalesOrderHeaderRepository

## Benefits

| Aspect | Benefit |
|--------|---------|
| **Testing** | No database required; use InMemory*Repository in unit tests |
| **Stored Procedures** | Fully preserved in Sql*Repository; no migration to LINQ needed |
| **EF Core** | Stays installed and ready for future LINQ migration if desired |
| **Production** | Uses Sql*Repository (stored procedures) automatically |
| **Flexibility** | Swap implementations at any time without touching page/controller code |
| **Future Migration** | Can add EF*Repository implementations using LINQ whenever you want |

## Current State (Commit: WithEF branch)

✅ **Product** — Fully implemented (SqlProductRepository + InMemoryProductRepository)
⏳ **Other 9 services** — Ready to follow the same pattern

## Next Steps

1. Create SqlAddressRepository and InMemoryAddressRepository
2. Create SqlCustomerRepository and InMemoryCustomerRepository
3. ... repeat for remaining 7 services
4. Update Program.cs to register all Sql*Repository implementations
5. Create unit tests using InMemory*Repository implementations
6. Commit the complete refactoring
