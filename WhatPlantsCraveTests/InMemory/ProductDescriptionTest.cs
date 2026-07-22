using Brawndo_Components.Models;
using WhatPlantsCrave.Infrastructure.Repositories.InMemory;

namespace WhatPlantsCraveTests.InMemory
{
    public class ProductDescriptionRepositoryTest
    {
        [Fact]
        public void Create_AddsDescriptionToRepository()
        {
            var repository = new InMemoryProductDescriptionRepository();
            int id = repository.Create("High-performance mountain bike");
            Assert.True(id > 0);
            var desc = repository.GetByID(id);
            Assert.NotNull(desc);
            Assert.Equal("High-performance mountain bike", desc.Description);
        }

        [Fact]
        public void GetAll_ReturnsMultipleDescriptions()
        {
            var repository = new InMemoryProductDescriptionRepository();
            repository.Create("Mountain bike description");
            repository.Create("Road bike description");
            var descriptions = repository.GetAll();
            Assert.Equal(2, descriptions.Count);
        }

        [Fact]
        public void Update_ModifiesDescription()
        {
            var repository = new InMemoryProductDescriptionRepository();
            int id = repository.Create("Original description");
            bool updated = repository.Update(id, description: "Updated description");
            Assert.True(updated);
            var desc = repository.GetByID(id);
            Assert.NotNull(desc);
            Assert.Equal("Updated description", desc.Description);
        }

        [Fact]
        public void Delete_RemovesDescription()
        {
            var repository = new InMemoryProductDescriptionRepository();
            int id = repository.Create("To be deleted");
            bool deleted = repository.Delete(id);
            Assert.True(deleted);
            Assert.Null(repository.GetByID(id));
        }
    }
}
