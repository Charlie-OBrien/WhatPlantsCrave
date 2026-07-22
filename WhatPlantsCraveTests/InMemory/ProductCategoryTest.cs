using Brawndo_Components.Models;
using WhatPlantsCrave.Infrastructure.Repositories.InMemory;

namespace WhatPlantsCraveTests.InMemory
{
    public class ProductCategoryRepositoryTest
    {
        [Fact]
        public void Create_AddsCategoryToRepository()
        {
            var repository = new InMemoryProductCategoryRepository();
            int id = repository.Create("Bikes");
            Assert.True(id > 0);
            var category = repository.GetByID(id);
            Assert.NotNull(category);
            Assert.Equal("Bikes", category.Name);
        }

        [Fact]
        public void GetAll_ReturnsMultipleCategories()
        {
            var repository = new InMemoryProductCategoryRepository();
            repository.Create("Bikes");
            repository.Create("Accessories");
            var categories = repository.GetAll();
            Assert.Equal(2, categories.Count);
        }

        [Fact]
        public void Update_ModifiesCategoryData()
        {
            var repository = new InMemoryProductCategoryRepository();
            int id = repository.Create("Bikes");
            bool updated = repository.Update(id, name: "Mountain Bikes");
            Assert.True(updated);
            var category = repository.GetByID(id);
            Assert.NotNull(category);
            Assert.Equal("Mountain Bikes", category.Name);
        }

        [Fact]
        public void Delete_RemovesCategory()
        {
            var repository = new InMemoryProductCategoryRepository();
            int id = repository.Create("Bikes");
            bool deleted = repository.Delete(id);
            Assert.True(deleted);
            Assert.Null(repository.GetByID(id));
        }

        [Fact]
        public void GetRootCategories_ReturnsOnlyRoots()
        {
            var repository = new InMemoryProductCategoryRepository();
            int parentId = repository.Create("Bikes");
            repository.Create("Mountain Bikes", parentProductCategoryId: parentId);
            repository.Create("Accessories");
            var roots = repository.GetRootCategories();
            Assert.Equal(2, roots.Count);
        }
    }
}
