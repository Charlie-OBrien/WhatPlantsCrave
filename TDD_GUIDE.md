# Test Driven Development (TDD) Guide for WhatPlantsCrave

## Overview

This guide shows how to use the WhatPlantsCraveTests project for Test Driven Development. The InMemory repository tests provide **instant feedback** (~70ms for all 50 tests) without database dependencies, making them perfect for the Red-Green-Refactor cycle.

---

## Why TDD Works Best Here

### ✅ Advantages of InMemory Tests for TDD

| Aspect | Benefit |
|--------|---------|
| **Speed** | 50 tests run in 70ms (vs. 1-2 seconds with database) |
| **No Setup** | No database, no fixtures, no cleanup |
| **Isolation** | Each test runs independently in memory |
| **Instant Feedback** | Write test → see result immediately |
| **Flow State** | Fast loop keeps you focused on logic |
| **Confidence** | Failures are logic errors, not DB issues |

### ❌ Why SQL-First Breaks TDD Flow

- Each test takes 1-2 seconds (database latency)
- Tests fail due to DB state, not logic
- Setup/teardown adds friction
- Breaks concentration during Red-Green-Refactor

---

## The TDD Cycle with This Project

```
┌─────────────────────────────────────────────────────────┐
│ 1. RED: Write Failing Test (InMemory)                   │
│    └─ test fails, method doesn't exist                  │
├─────────────────────────────────────────────────────────┤
│ 2. GREEN: Implement in InMemoryXxxRepository            │
│    └─ test passes, feature works in memory              │
├─────────────────────────────────────────────────────────┤
│ 3. REFACTOR: Clean up implementation (optional)         │
│    └─ test still passes, code is better                 │
├─────────────────────────────────────────────────────────┤
│ 4. INTEGRATE: Implement SqlXxxRepository (optional)     │
│    └─ create SQL version for production use             │
├─────────────────────────────────────────────────────────┤
│ 5. VERIFY: Run full test suite                          │
│    └─ all 58 tests pass, nothing broke                  │
└─────────────────────────────────────────────────────────┘
```

---

## Step-by-Step Example: Add Price Range Filter

### Scenario
You want to add a feature: **"Filter products by price range"**

### Step 1: RED — Write Failing Test

File: `WhatPlantsCraveTests/InMemory/ProductTest.cs`

```csharp
[Fact]
public void GetByPriceRange_FiltersProductsCorrectly()
{
    var repository = new InMemoryProductRepository();
    repository.Create("Budget Bike", "BUDGET-001", 50m, 100m, DateTime.Now);
    repository.Create("Premium Bike", "PREMIUM-001", 500m, 1000m, DateTime.Now);
    repository.Create("Mid-Range Bike", "MID-001", 200m, 400m, DateTime.Now);
    
    var results = repository.GetByPriceRange(minPrice: 200m, maxPrice: 900m);
    
    Assert.Equal(2, results.Count);
    Assert.Contains(results, p => p.Name == "Premium Bike");
    Assert.Contains(results, p => p.Name == "Mid-Range Bike");
    Assert.DoesNotContain(results, r => r.Name == "Budget Bike");
}
```

**Run the test:**
```bash
cd WhatPlantsCraveTests
dotnet test --filter ProductRepositoryTest.GetByPriceRange_FiltersProductsCorrectly
```

**Expected result:** ❌ **FAILS** with error: `'GetByPriceRange' does not exist`

---

### Step 2: GREEN — Implement to Pass

**Update the interface** (`WhatPlantsCrave/Infrastructure/Repositories/ProductRepository.cs`):

```csharp
public interface IProductRepository
{
    // ... existing methods ...
    List<Product> GetByPriceRange(decimal minPrice, decimal maxPrice);
}
```

**Implement in InMemory** (`WhatPlantsCraveTests/InMemory/ProductTest.cs`):

```csharp
public class InMemoryProductRepository : IProductRepository
{
    // ... existing code ...
    
    public List<Product> GetByPriceRange(decimal minPrice, decimal maxPrice) =>
        _products.Where(p => p.ListPrice >= minPrice && p.ListPrice <= maxPrice).ToList();
}
```

**Run the test again:**
```bash
dotnet test --filter ProductRepositoryTest.GetByPriceRange_FiltersProductsCorrectly
```

**Expected result:** ✅ **PASSES**

---

### Step 3: REFACTOR — Improve (Optional)

```csharp
public List<Product> GetByPriceRange(decimal minPrice, decimal maxPrice)
{
    if (minPrice < 0 || maxPrice < 0)
        throw new ArgumentException("Prices must be non-negative");
    if (minPrice > maxPrice)
        throw new ArgumentException("MinPrice cannot exceed MaxPrice");
    
    return _products
        .Where(p => p.ListPrice >= minPrice && p.ListPrice <= maxPrice)
        .OrderBy(p => p.ListPrice)
        .ToList();
}
```

**Run test again:** ✅ Still passes, but implementation is more robust

---

### Step 4: INTEGRATE — Implement SQL Version (Optional)

When you're confident the logic is correct, implement the production version.

**Update SQL Repository** (`WhatPlantsCrave/Infrastructure/Repositories/Sql/SqlProductRepository.cs`):

```csharp
public List<Product> GetByPriceRange(decimal minPrice, decimal maxPrice)
{
    try
    {
        using var connection = new SqlConnection(_context.Database.GetConnectionString());
        connection.Open();
        using var command = new SqlCommand("SalesLT.usp_Product_GetByPriceRange", connection);
        command.CommandType = CommandType.StoredProcedure;
        command.Parameters.AddWithValue("@MinPrice", minPrice);
        command.Parameters.AddWithValue("@MaxPrice", maxPrice);
        using var reader = command.ExecuteReader();
        return ReadProducts(reader);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Failed to get products by price range {Min}-{Max}", minPrice, maxPrice);
        return new List<Product>();
    }
}
```

**Add SQL Integration Test** (`WhatPlantsCraveTests/Sql/SqlProductRepositoryTest.cs`):

```csharp
[Fact]
public void GetByPriceRange_ReturnsProductsInRange()
{
    var allProducts = _repository.GetAll();
    Assert.NotEmpty(allProducts);
    
    var minPrice = allProducts.First().ListPrice - 10m;
    var maxPrice = allProducts.First().ListPrice + 10m;
    var results = _repository.GetByPriceRange(minPrice, maxPrice);
    
    Assert.NotEmpty(results);
    foreach (var product in results)
    {
        Assert.True(product.ListPrice >= minPrice && product.ListPrice <= maxPrice);
    }
}
```

---

### Step 5: VERIFY — Run Full Suite

```bash
dotnet test
```

**Expected result:** ✅ All 58 tests pass (50 InMemory + 8 SQL)

---

## Command Reference

### Development Loop (Fast Feedback)

```bash
# Run just the service you're working on (e.g., Product)
dotnet test --filter FullyQualifiedName~InMemory.ProductRepositoryTest

# Run just InMemory tests (no database)
dotnet test --filter Namespace~InMemory

# Run tests continuously on file save (requires dotnet-watch)
dotnet watch test --filter Namespace~InMemory
```

### Integration Testing

```bash
# Run just SQL tests (requires database)
dotnet test --filter Namespace~Sql

# Run specific SQL test
dotnet test --filter FullyQualifiedName~Sql.SqlProductRepositoryTest
```

### Full Suite

```bash
# Run all 58 tests
dotnet test

# Run with verbose output
dotnet test --verbosity detailed

# Run with code coverage
dotnet test /p:CollectCoverage=true
```

---

## TDD Best Practices for This Project

### ✅ DO

- **Write test first** — before implementing any logic
- **Test one thing** — each test method tests one behavior
- **Use descriptive names** — test name should explain what it tests
- **Keep tests fast** — use InMemory, skip database until Step 4
- **Run tests frequently** — after every code change
- **Commit when green** — commit only when all tests pass
- **Add edge cases** — test boundary conditions (empty, null, zero, negative)

### ❌ DON'T

- **Test implementation details** — test behavior, not internal state
- **Create test interdependencies** — tests should run in any order
- **Skip InMemory tests** — they're your fast feedback loop
- **Hardcode IDs** — use repository results, not database IDs
- **Leave failing tests** — fix them immediately or revert the change
- **Test database directly** — SQL tests verify repository, not DB schema

---

## Example: Testing All 10 Services

Each service follows the same TDD pattern:

| Service | InMemory Test | SQL Test | Status |
|---------|---------------|----------|--------|
| Product | ProductTest.cs | SqlProductRepositoryTest.cs | ✅ Complete |
| Address | AddressTest.cs | — | ✅ Complete |
| Customer | CustomerTest.cs | — | ✅ Complete |
| CustomerAddress | CustomerAddressTest.cs | — | ✅ Complete |
| ProductCategory | ProductCategoryTest.cs | — | ✅ Complete |
| ProductDescription | ProductDescriptionTest.cs | — | ✅ Complete |
| ProductModel | ProductModelTest.cs | — | ✅ Complete |
| ProductModelProductDescription | ProductModelProductDescriptionTest.cs | — | ✅ Complete |
| SalesOrderDetail | SalesOrderDetailTest.cs | — | ✅ Complete |
| SalesOrderHeader | SalesOrderHeaderTest.cs | — | ✅ Complete |

### Adding a New Method to Address Service

```bash
# 1. Write failing test in AddressTest.cs
dotnet test --filter AddressRepositoryTest.YourNewMethodName_DoesX
# ❌ FAILS

# 2. Implement in InMemoryAddressRepository
# 3. Run test again
dotnet test --filter AddressRepositoryTest.YourNewMethodName_DoesX
# ✅ PASSES

# 4. (Optional) Implement SqlAddressRepository
# 5. Run full suite
dotnet test
# ✅ All 58 pass
```

---

## Workflow Timeline

### Single Development Session

```
0m:00s  - Write failing test (RED)
0m:15s  - Implement InMemory (GREEN)
0m:30s  - Refactor + run test (REFACTOR)
1m:00s  - Run full InMemory suite
2m:00s  - Implement SQL version
2m:30s  - Run SQL test
3m:00s  - Run full suite (all 58 tests)
3m:30s  - Commit and push
```

Compare to traditional approach:

```
0m:00s  - Design database schema
15m:00  - Run migrations
20m:00  - Write integration tests
25m:00  - Implement service
35m:00  - Debug database issues
45m:00  - Finally see tests pass
```

---

## Common TDD Patterns

### Pattern: Create and Retrieve

```csharp
[Fact]
public void Create_ReturnsIdAndGetByID_RetrievesIt()
{
    var repository = new InMemoryProductRepository();
    
    int id = repository.Create("Test Product", "TEST-001", 10m, 20m, DateTime.Now);
    
    Assert.True(id > 0);
    var retrieved = repository.GetByID(id);
    Assert.NotNull(retrieved);
    Assert.Equal("Test Product", retrieved.Name);
}
```

### Pattern: Update and Verify

```csharp
[Fact]
public void Update_ChangesPropertyAndGetByID_ReflectsChange()
{
    var repository = new InMemoryProductRepository();
    int id = repository.Create("Original", "ORG-001", 10m, 20m, DateTime.Now);
    
    bool updated = repository.Update(id, name: "Updated");
    
    Assert.True(updated);
    var retrieved = repository.GetByID(id);
    Assert.NotNull(retrieved);
    Assert.Equal("Updated", retrieved.Name);
}
```

### Pattern: Delete and Verify Gone

```csharp
[Fact]
public void Delete_RemovesItemAndGetByID_ReturnsNull()
{
    var repository = new InMemoryProductRepository();
    int id = repository.Create("To Delete", "DEL-001", 10m, 20m, DateTime.Now);
    
    bool deleted = repository.Delete(id);
    
    Assert.True(deleted);
    var retrieved = repository.GetByID(id);
    Assert.Null(retrieved);
}
```

---

## Summary

**TDD with WhatPlantsCrave is fast because:**

1. Write test → 1 second
2. Implement → 10 seconds  
3. Test passes → 2 seconds
4. Full suite → 70ms

**Total cycle time: <2 minutes per feature**

vs. traditional: 30+ minutes with database setup, migrations, debugging.

**Start with InMemory. Stay fast. Add SQL only when proven.**

---

## Next Steps

1. Pick a service (e.g., Customer, ProductCategory)
2. Write a failing test for a new method
3. Implement in InMemory repository
4. Watch test pass in <1 second
5. Refactor if needed
6. (Optional) Add SQL version
7. Commit when all tests pass

Happy TDD! 🚀

---

**Guide Version:** 1.0  
**Branch:** WithEF (Entity Framework + Testable Repositories)  
**Last Updated:** 2026-07-22
