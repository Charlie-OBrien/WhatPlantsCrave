using Brawndo_Components;
using Brawndo_Components.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace WhatPlantsCraveTests.Sql.Services
{
    public class ProductDescriptionServiceVerificationTest : IDisposable
    {
        private readonly AdventureWorksContext _context;
        private readonly ProductDescriptionService _service;

        public ProductDescriptionServiceVerificationTest()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseSqlServer("Server=DESKTOP-OAJVK2J;Database=AdventureWorksLT2016;Trusted_Connection=true;TrustServerCertificate=true;")
                .Options;

            _context = new AdventureWorksContext(options);
            _service = new ProductDescriptionService(_context);
        }

        [Fact]
        public void GetAll_ReturnsDescriptions()
        {
            var descriptions = _service.GetAll();

            Assert.NotNull(descriptions);
            Assert.NotEmpty(descriptions);
            Assert.All(descriptions, d =>
            {
                Assert.True(d.ProductDescriptionId > 0);
                Assert.False(string.IsNullOrEmpty(d.Description));
            });
        }

        [Fact]
        public void GetByID_ReturnsExistingDescription()
        {
            var all = _service.GetAll();
            Assert.NotEmpty(all);

            var knownId = all.First().ProductDescriptionId;
            var description = _service.GetByID(knownId);

            Assert.NotNull(description);
            Assert.Equal(knownId, description.ProductDescriptionId);
            Assert.False(string.IsNullOrEmpty(description.Description));
        }

        [Fact]
        public void GetByID_ReturnsNullForNonExistentDescription()
        {
            var description = _service.GetByID(999999);

            Assert.Null(description);
        }

        [Fact]
        public void Create_Update_Delete_FullLifecycle()
        {
            int descriptionId = _service.Create(description: "SvcTest description for verification");

            Assert.True(descriptionId > 0);

            var created = _service.GetByID(descriptionId);
            Assert.NotNull(created);
            Assert.Equal("SvcTest description for verification", created.Description);

            bool updated = _service.Update(descriptionId, description: "SvcTest updated description");
            Assert.True(updated);

            var afterUpdate = _service.GetByID(descriptionId);
            Assert.NotNull(afterUpdate);
            Assert.Equal("SvcTest updated description", afterUpdate.Description);

            bool deleted = _service.Delete(descriptionId);
            Assert.True(deleted);

            var afterDelete = _service.GetByID(descriptionId);
            Assert.Null(afterDelete);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
