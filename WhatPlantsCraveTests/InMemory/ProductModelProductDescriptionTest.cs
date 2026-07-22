using Brawndo_Components.Models;
using WhatPlantsCrave.Infrastructure.Repositories.InMemory;

namespace WhatPlantsCraveTests.InMemory
{
    public class ProductModelProductDescriptionRepositoryTest
    {
        [Fact]
        public void Create_AddsLinkToRepository()
        {
            var repository = new InMemoryProductModelProductDescriptionRepository();
            bool created = repository.Create(1, 100, "en");
            Assert.True(created);
            var result = repository.Get(1, 100, "en");
            Assert.NotNull(result);
            Assert.Equal("en", result.Culture);
        }

        [Fact]
        public void GetAll_ReturnsMultipleLinks()
        {
            var repository = new InMemoryProductModelProductDescriptionRepository();
            repository.Create(1, 100, "en");
            repository.Create(1, 100, "fr");
            repository.Create(2, 200, "en");
            var all = repository.GetAll();
            Assert.Equal(3, all.Count);
        }

        [Fact]
        public void Update_ChangesCulture()
        {
            var repository = new InMemoryProductModelProductDescriptionRepository();
            repository.Create(1, 100, "en");
            bool updated = repository.Update(1, 100, "en", "fr");
            Assert.True(updated);
            Assert.Null(repository.Get(1, 100, "en"));
            var result = repository.Get(1, 100, "fr");
            Assert.NotNull(result);
            Assert.Equal("fr", result.Culture);
        }

        [Fact]
        public void Delete_RemovesLink()
        {
            var repository = new InMemoryProductModelProductDescriptionRepository();
            repository.Create(1, 100, "en");
            bool deleted = repository.Delete(1, 100, "en");
            Assert.True(deleted);
            Assert.Null(repository.Get(1, 100, "en"));
        }
    }
}
