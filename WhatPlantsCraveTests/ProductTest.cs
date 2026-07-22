using Brawndo_Components.Models;
using WhatPlantsCrave.Infrastructure.Repositories;
using WhatPlantsCrave.Infrastructure.Repositories.InMemory;

namespace WhatPlantsCraveTests
{
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
    }
}