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
                name: "Test Widget", productNumber: "TST-001", standardCost: 10m, listPrice: 20m, sellStartDate: DateTime.Now);
            Assert.True(productId > 0);
            var product = repository.GetByID(productId);
            Assert.NotNull(product);
            Assert.Equal("Test Widget", product.Name);
        }

        [Fact]
        public void GetAll_ReturnsMultipleProducts()
        {
            var repository = new InMemoryProductRepository();
            repository.Create(name: "Widget", productNumber: "WDG-001", standardCost: 10m, listPrice: 20m, sellStartDate: DateTime.Now);
            repository.Create(name: "Gadget", productNumber: "GDG-001", standardCost: 15m, listPrice: 30m, sellStartDate: DateTime.Now);
            var products = repository.GetAll();
            Assert.Equal(2, products.Count);
        }

        [Fact]
        public void Update_ModifiesProductData()
        {
            var repository = new InMemoryProductRepository();
            int id = repository.Create(name: "Original", productNumber: "ORG-001", standardCost: 10m, listPrice: 20m, sellStartDate: DateTime.Now);
            bool updated = repository.Update(id, name: "Updated", listPrice: 25m);
            Assert.True(updated);
            var product = repository.GetByID(id);
            Assert.Equal("Updated", product.Name);
        }

        [Fact]
        public void Delete_RemovesProduct()
        {
            var repository = new InMemoryProductRepository();
            int id = repository.Create(name: "ToDelete", productNumber: "DEL-001", standardCost: 10m, listPrice: 20m, sellStartDate: DateTime.Now);
            bool deleted = repository.Delete(id);
            Assert.True(deleted);
            Assert.Null(repository.GetByID(id));
        }

        [Fact]
        public void GetByPriceRange_FiltersProductsCorrectly()
        {
            var repository = new InMemoryProductRepository();
            repository.Create(name: "Budget Bike", productNumber: "BUDGET-001", standardCost: 50m, listPrice: 100m, sellStartDate: DateTime.Now);    // Below minPrice
            repository.Create(name: "Mid-Range Bike", productNumber: "MID-001", standardCost: 200m, listPrice: 400m, sellStartDate: DateTime.Now);   // Within range
            repository.Create(name: "Premium Bike", productNumber: "PREMIUM-001", standardCost: 500m, listPrice: 1000m, sellStartDate: DateTime.Now); // To high on the maxPrice side

            var results = repository.GetByPriceRange(minPrice: 200m, maxPrice: 900m);

            Assert.Equal(2, results.Count);
            Assert.Contains(results, p => p.Name == "Premium Bike");
            Assert.Contains(results, p => p.Name == "Mid-Range Bike");
            Assert.DoesNotContain(results, r => r.Name == "Budget Bike");
        }



    }//END Class    
}//END Namespace