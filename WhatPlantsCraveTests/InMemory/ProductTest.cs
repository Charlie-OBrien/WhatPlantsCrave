using Brawndo_Components.Models;
using WhatPlantsCrave.Infrastructure.Repositories;
using WhatPlantsCrave.Infrastructure.Repositories.InMemory;

namespace WhatPlantsCraveTests.InMemory
{
    /// <summary>
    /// In Memory Product Repository Tests
    /// Model: Brawndo_Components/Models/Product.cs
    /// Service: Brawndo_Components/Product.Service.cs
    /// Repository: WhatPlantsCrave.Infrastructure.Repositories.InMemory/InMemoryProductRepository.cs
    /// </summary>
    public class ProductRepositoryTest
    {
        [Fact]
        public void CreateProduct_AddsProductToRepository()
        {
            var repository = new InMemoryProductRepository();
            int productId = repository.Create(
                "Test Widget", "TST-001", 10m, 20m, DateTime.Now);
            Assert.True(productId > 0);
            var product = repository.GetByID(productId);
            Assert.NotNull(product);
            Assert.Equal("Test Widget", product.Name);
        }

        [Fact]
        public void GetAll_ReturnsMultipleProducts()
        {
            var repository = new InMemoryProductRepository();
            repository.Create("Widget", "WDG-001", 10m, 20m, DateTime.Now);
            repository.Create("Gadget", "GDG-001", 15m, 30m, DateTime.Now);
            var products = repository.GetAll();
            Assert.Equal(2, products.Count);
        }

        [Fact]
        public void Update_ModifiesProductData()
        {
            var repository = new InMemoryProductRepository();
            int id = repository.Create("Original", "ORG-001", 10m, 20m, DateTime.Now);
            bool updated = repository.Update(id, "Updated", listPrice: 25m);
            Assert.True(updated);
            var product = repository.GetByID(id);
            Assert.Equal("Updated", product.Name);
        }

        [Fact]
        public void Delete_RemovesProduct()
        {
            var repository = new InMemoryProductRepository();
            int id = repository.Create("ToDelete", "DEL-001", 10m, 20m, DateTime.Now);
            bool deleted = repository.Delete(id);
            Assert.True(deleted);
            Assert.Null(repository.GetByID(id));
        }

        [Fact]
        public void GetByPriceRange_FiltersProductsCorrectly()
        {
            var repository = new InMemoryProductRepository();
            repository.Create("Budget Bike", "BUDGET-001", 50m, 100m, DateTime.Now);    // Below minPrice
            repository.Create("Mid-Range Bike", "MID-001", 200m, 400m, DateTime.Now);   // Within range
            repository.Create("Premium Bike", "PREMIUM-001", 500m, 1000m, DateTime.Now); // To high on the maxPrice side

            var results = repository.GetByPriceRange(minPrice: 200m, maxPrice: 900m);

            Assert.Equal(2, results.Count);
            Assert.Contains(results, p => p.Name == "Premium Bike");
            Assert.Contains(results, p => p.Name == "Mid-Range Bike");
            Assert.DoesNotContain(results, r => r.Name == "Budget Bike");
        }



    }//END Class    
}//END Namespace