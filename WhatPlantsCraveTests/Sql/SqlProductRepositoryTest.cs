using Brawndo_Components.Data;
using Brawndo_Components.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WhatPlantsCrave.Infrastructure.Repositories.Sql;
using Xunit;

namespace WhatPlantsCraveTests.Sql
{
    public class SqlProductRepositoryTest : IDisposable
    {
        private readonly AdventureWorksContext _context;
        private readonly SqlProductRepository _repository;

        public SqlProductRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseSqlServer("Server=DESKTOP-OAJVK2J;Database=AdventureWorksLT2016;Trusted_Connection=true;TrustServerCertificate=true;")
                .Options;

            _context = new AdventureWorksContext(options);
            var logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<SqlProductRepository>();
            _repository = new SqlProductRepository(_context, logger);
        }

        [Fact]
        public void GetAll_ReturnsProductsFromDatabase()
        {
            var products = _repository.GetAll();

            Assert.NotNull(products);
            Assert.NotEmpty(products);
        }

        [Fact]
        public void GetByID_ReturnsExistingProduct()
        {
            var allProducts = _repository.GetAll();
            Assert.NotEmpty(allProducts);

            var knownId = allProducts.First().ProductId;
            var product = _repository.GetByID(knownId);

            Assert.NotNull(product);
            Assert.Equal(knownId, product.ProductId);
        }

        [Fact]
        public void GetByID_ReturnsNullForNonExistentProduct()
        {
            var product = _repository.GetByID(999999);

            Assert.Null(product);
        }

        [Fact]
        public void Create_AddsProductToDatabase()
        {
            string productName = $"Test Product {DateTime.UtcNow.Ticks}";
            string productNumber = $"TEST-{DateTime.UtcNow.Ticks}";

            int newProductId = _repository.Create(
                name: productName,
                productNumber: productNumber,
                standardCost: 10m,
                listPrice: 20m,
                sellStartDate: DateTime.Now
            );

            Assert.True(newProductId > 0);

            var savedProduct = _repository.GetByID(newProductId);
            Assert.NotNull(savedProduct);
            Assert.Equal(productName, savedProduct.Name);

            _repository.Delete(newProductId);
        }

        [Fact]
        public void Update_ModifiesExistingProduct()
        {
            string originalName = $"Original {DateTime.UtcNow.Ticks}";
            string productNumber = $"UPD-{DateTime.UtcNow.Ticks}";
            int productId = _repository.Create(originalName, productNumber, 10m, 20m, DateTime.Now);
            Assert.True(productId > 0);

            string updatedName = $"Updated {DateTime.UtcNow.Ticks}";
            bool updated = _repository.Update(productId, name: updatedName, listPrice: 25m);

            Assert.True(updated);
            var retrievedProduct = _repository.GetByID(productId);
            Assert.NotNull(retrievedProduct);
            Assert.Equal(updatedName, retrievedProduct.Name);
            Assert.Equal(25m, retrievedProduct.ListPrice);

            _repository.Delete(productId);
        }

        [Fact]
        public void Delete_RemovesProductFromDatabase()
        {
            int productId = _repository.Create(
                $"To Delete {DateTime.UtcNow.Ticks}",
                $"DEL-{DateTime.UtcNow.Ticks}",
                10m, 20m, DateTime.Now);
            Assert.True(productId > 0);

            bool deleted = _repository.Delete(productId);

            Assert.True(deleted);
            var deletedProduct = _repository.GetByID(productId);
            Assert.Null(deletedProduct);
        }

        [Fact]
        public void SearchByName_ReturnsResults()
        {
            var allProducts = _repository.GetAll();
            Assert.NotEmpty(allProducts);

            var firstProductName = allProducts.First().Name;
            var results = _repository.SearchByName(firstProductName[..3]);

            Assert.NotNull(results);
            Assert.NotEmpty(results);
        }

        [Fact]
        public void GetActive_ReturnsActiveProducts()
        {
            var activeProducts = _repository.GetActive();

            Assert.NotNull(activeProducts);
            Assert.NotEmpty(activeProducts);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
