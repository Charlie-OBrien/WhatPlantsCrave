using Brawndo_Components;
using Brawndo_Components.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace WhatPlantsCraveTests.Sql.Services
{
    public class ProductModelServiceVerificationTest : IDisposable
    {
        private readonly AdventureWorksContext _context;
        private readonly ProductModelService _service;

        public ProductModelServiceVerificationTest()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseSqlServer("Server=DESKTOP-OAJVK2J;Database=AdventureWorksLT2016;Trusted_Connection=true;TrustServerCertificate=true;")
                .Options;

            _context = new AdventureWorksContext(options);
            _service = new ProductModelService(_context);
        }

        [Fact]
        public void GetAll_ReturnsProductModels()
        {
            var models = _service.GetAll();

            Assert.NotNull(models);
            Assert.NotEmpty(models);
            Assert.All(models, m =>
            {
                Assert.True(m.ProductModelId > 0);
                Assert.False(string.IsNullOrEmpty(m.Name));
            });
        }

        [Fact]
        public void GetByID_ReturnsExistingModel()
        {
            var all = _service.GetAll();
            Assert.NotEmpty(all);

            var knownId = all.First().ProductModelId;
            var model = _service.GetByID(knownId);

            Assert.NotNull(model);
            Assert.Equal(knownId, model.ProductModelId);
            Assert.False(string.IsNullOrEmpty(model.Name));
        }

        [Fact]
        public void GetByID_ReturnsNullForNonExistentModel()
        {
            var model = _service.GetByID(999999);

            Assert.Null(model);
        }

        [Fact]
        public void Create_Delete_FullLifecycle()
        {
            int modelId = _service.Create(name: "SvcTest Model");

            Assert.True(modelId > 0);

            var created = _service.GetByID(modelId);
            Assert.NotNull(created);
            Assert.Equal("SvcTest Model", created.Name);

            bool deleted = _service.Delete(modelId);
            Assert.True(deleted);

            var afterDelete = _service.GetByID(modelId);
            Assert.Null(afterDelete);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
