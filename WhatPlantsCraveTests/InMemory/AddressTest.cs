using Brawndo_Components.Models;
using WhatPlantsCrave.Infrastructure.Repositories.InMemory;

namespace WhatPlantsCraveTests.InMemory
{
    public class AddressRepositoryTest
    {
        [Fact]
        public void Create_AddsAddressToRepository()
        {
            var repository = new InMemoryAddressRepository();
            int id = repository.Create("123 Main St", "Springfield", "IL", "United States", "62701");
            Assert.True(id > 0);
            var address = repository.GetByID(id);
            Assert.NotNull(address);
            Assert.Equal("123 Main St", address.AddressLine1);
            Assert.Equal("Springfield", address.City);
        }

        [Fact]
        public void GetAll_ReturnsMultipleAddresses()
        {
            var repository = new InMemoryAddressRepository();
            repository.Create("123 Main St", "Springfield", "IL", "United States", "62701");
            repository.Create("456 Oak Ave", "Chicago", "IL", "United States", "60601");
            var addresses = repository.GetAll();
            Assert.Equal(2, addresses.Count);
        }

        [Fact]
        public void Update_ModifiesAddressData()
        {
            var repository = new InMemoryAddressRepository();
            int id = repository.Create("123 Main St", "Springfield", "IL", "United States", "62701");
            bool updated = repository.Update(id, city: "Chicago");
            Assert.True(updated);
            var address = repository.GetByID(id);
            Assert.NotNull(address);
            Assert.Equal("Chicago", address.City);
        }

        [Fact]
        public void Delete_RemovesAddress()
        {
            var repository = new InMemoryAddressRepository();
            int id = repository.Create("123 Main St", "Springfield", "IL", "United States", "62701");
            bool deleted = repository.Delete(id);
            Assert.True(deleted);
            Assert.Null(repository.GetByID(id));
        }

        [Fact]
        public void GetByCountryRegion_FiltersCorrectly()
        {
            var repository = new InMemoryAddressRepository();
            repository.Create("123 Main St", "Springfield", "IL", "United States", "62701");
            repository.Create("10 Downing St", "London", "England", "United Kingdom", "SW1A");
            var usAddresses = repository.GetByCountryRegion("United States");
            Assert.Single(usAddresses);
        }

        [Fact]
        public void SearchByCity_FindsMatches()
        {
            var repository = new InMemoryAddressRepository();
            repository.Create("123 Main St", "Springfield", "IL", "United States", "62701");
            repository.Create("456 Oak Ave", "Chicago", "IL", "United States", "60601");
            var results = repository.SearchByCity("Spring");
            Assert.Single(results);
        }
    }
}
