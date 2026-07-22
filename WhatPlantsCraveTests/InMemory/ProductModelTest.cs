using Brawndo_Components.Models;
using WhatPlantsCrave.Infrastructure.Repositories.InMemory;

namespace WhatPlantsCraveTests.InMemory
{
    public class ProductModelRepositoryTest
    {
        [Fact]
        public void Create_AddsModelToRepository()
        {
            var repository = new InMemoryProductModelRepository();
            int id = repository.Create("Mountain-100", catalogDescription: "Entry level mountain bike");
            Assert.True(id > 0);
            var model = repository.GetByID(id);
            Assert.NotNull(model);
            Assert.Equal("Mountain-100", model.Name);
            Assert.Equal("Entry level mountain bike", model.CatalogDescription);
        }

        [Fact]
        public void GetAll_ReturnsMultipleModels()
        {
            var repository = new InMemoryProductModelRepository();
            repository.Create("Mountain-100");
            repository.Create("Road-150");
            var models = repository.GetAll();
            Assert.Equal(2, models.Count);
        }

        [Fact]
        public void Delete_RemovesModel()
        {
            var repository = new InMemoryProductModelRepository();
            int id = repository.Create("Mountain-100");
            bool deleted = repository.Delete(id);
            Assert.True(deleted);
            Assert.Null(repository.GetByID(id));
        }

        [Fact]
        public void GetByID_ReturnsNullForNonExistent()
        {
            var repository = new InMemoryProductModelRepository();
            var model = repository.GetByID(999);
            Assert.Null(model);
        }
    }
}
