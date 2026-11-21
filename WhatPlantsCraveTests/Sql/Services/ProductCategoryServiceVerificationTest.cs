using Brawndo_Components;
using Brawndo_Components.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace WhatPlantsCraveTests.Sql.Services
{
    public class ProductCategoryServiceVerificationTest : IDisposable
    {
        private readonly AdventureWorksContext _context;
        private readonly ProductCategoryService _service;

        public ProductCategoryServiceVerificationTest()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseSqlServer("Server=DESKTOP-OAJVK2J;Database=AdventureWorksLT2016;Trusted_Connection=true;TrustServerCertificate=true;")
                .Options;

            _context = new AdventureWorksContext(options);
            _service = new ProductCategoryService(_context);
        }

        [Fact]
        public void GetAll_ReturnsCategories()
        {
            var categories = _service.GetAll();

            Assert.NotNull(categories);
            Assert.NotEmpty(categories);
            Assert.All(categories, c =>
            {
                Assert.True(c.ProductCategoryId > 0);
                Assert.False(string.IsNullOrEmpty(c.Name));
            });
        }

        [Fact]
        public void GetByID_ReturnsExistingCategory()
        {
            var all = _service.GetAll();
            Assert.NotEmpty(all);

            var knownId = all.First().ProductCategoryId;
            var category = _service.GetByID(knownId);

            Assert.NotNull(category);
            Assert.Equal(knownId, category.ProductCategoryId);
            Assert.False(string.IsNullOrEmpty(category.Name));
        }

        [Fact]
        public void GetByID_ReturnsNullForNonExistentCategory()
        {
            var category = _service.GetByID(999999);

            Assert.Null(category);
        }

        [Fact]
        public void GetRootCategories_ReturnsCategoriesWithNoParent()
        {
            var roots = _service.GetRootCategories();

            Assert.NotNull(roots);
            Assert.NotEmpty(roots);
            Assert.All(roots, c => Assert.Null(c.ParentProductCategoryId));
        }

        [Fact]
        public void Create_Update_Delete_FullLifecycle()
        {
            int categoryId = _service.Create(name: "SvcTest Category");

            Assert.True(categoryId > 0);

            var created = _service.GetByID(categoryId);
            Assert.NotNull(created);
            Assert.Equal("SvcTest Category", created.Name);

            bool updated = _service.Update(categoryId, name: "SvcTest Updated");
            Assert.True(updated);

            var afterUpdate = _service.GetByID(categoryId);
            Assert.NotNull(afterUpdate);
            Assert.Equal("SvcTest Updated", afterUpdate.Name);

            bool deleted = _service.Delete(categoryId);
            Assert.True(deleted);

            var afterDelete = _service.GetByID(categoryId);
            Assert.Null(afterDelete);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
