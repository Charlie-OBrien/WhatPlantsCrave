using Brawndo_Components;
using Brawndo_Components.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace WhatPlantsCraveTests.Sql.Services
{
    public class ProductServiceVerificationTest : IDisposable
    {
        private readonly AdventureWorksContext _context;
        private readonly ProductService _service;

        public ProductServiceVerificationTest()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseSqlServer("Server=DESKTOP-OAJVK2J;Database=AdventureWorksLT2016;Trusted_Connection=true;TrustServerCertificate=true;")
                .Options;

            _context = new AdventureWorksContext(options);
            _service = new ProductService(_context);
        }

        [Fact]
        public void GetAll_ReturnsProducts()
        {
            var products = _service.GetAll();

            Assert.NotNull(products);
            Assert.NotEmpty(products);
            Assert.All(products, p =>
            {
                Assert.True(p.ProductId > 0);
                Assert.False(string.IsNullOrEmpty(p.Name));
                Assert.False(string.IsNullOrEmpty(p.ProductNumber));
                Assert.True(p.ListPrice >= 0);
                Assert.True(p.StandardCost >= 0);
            });
        }

        [Fact]
        public void GetByID_ReturnsExistingProduct()
        {
            var allProducts = _service.GetAll();
            Assert.NotEmpty(allProducts);

            var knownId = allProducts.First().ProductId;
            var product = _service.GetByID(knownId);

            Assert.NotNull(product);
            Assert.Equal(knownId, product.ProductId);
            Assert.False(string.IsNullOrEmpty(product.Name));
            Assert.False(string.IsNullOrEmpty(product.ProductNumber));
        }

        [Fact]
        public void GetByID_ReturnsNullForNonExistentProduct()
        {
            var product = _service.GetByID(999999);

            Assert.Null(product);
        }

        [Fact]
        public void GetActive_ReturnsActiveProducts()
        {
            var activeProducts = _service.GetActive();

            Assert.NotNull(activeProducts);
            Assert.NotEmpty(activeProducts);
            Assert.All(activeProducts, p =>
            {
                Assert.True(p.ProductId > 0);
                Assert.False(string.IsNullOrEmpty(p.Name));
            });
        }

        [Fact]
        public void SearchByName_ReturnsMatchingProducts()
        {
            var allProducts = _service.GetAll();
            Assert.NotEmpty(allProducts);

            var searchTerm = allProducts.First().Name[..3];
            var results = _service.SearchByName(searchTerm);

            Assert.NotNull(results);
            Assert.NotEmpty(results);
        }

        [Fact]
        public void GetByPriceRange_ReturnsProductsInRange()
        {
            var allProducts = _service.GetAll();
            Assert.NotEmpty(allProducts);

            var minPrice = allProducts.Min(p => p.ListPrice);
            var maxPrice = allProducts.Max(p => p.ListPrice);
            var midPoint = (minPrice + maxPrice) / 2;

            var results = _service.GetByPriceRange(minPrice, midPoint);

            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.All(results, p =>
            {
                Assert.True(p.ListPrice >= minPrice);
                Assert.True(p.ListPrice <= midPoint);
            });
        }

        [Fact]
        public void Create_Update_Delete_FullLifecycle()
        {
            string productName = $"SvcTest Product {DateTime.UtcNow.Ticks}";
            string productNumber = $"SVCT-{DateTime.UtcNow.Ticks}";

            int productId = _service.Create(
                name: productName,
                productNumber: productNumber,
                standardCost: 10m,
                listPrice: 25m,
                sellStartDate: DateTime.Now);

            Assert.True(productId > 0);

            var created = _service.GetByID(productId);
            Assert.NotNull(created);
            Assert.Equal(productName, created.Name);
            Assert.Equal(25m, created.ListPrice);

            string updatedName = $"SvcTest Updated {DateTime.UtcNow.Ticks}";
            bool updated = _service.Update(productId, name: updatedName, listPrice: 50m);
            Assert.True(updated);

            var afterUpdate = _service.GetByID(productId);
            Assert.NotNull(afterUpdate);
            Assert.Equal(updatedName, afterUpdate.Name);
            Assert.Equal(50m, afterUpdate.ListPrice);

            bool deleted = _service.Delete(productId);
            Assert.True(deleted);

            var afterDelete = _service.GetByID(productId);
            Assert.Null(afterDelete);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
