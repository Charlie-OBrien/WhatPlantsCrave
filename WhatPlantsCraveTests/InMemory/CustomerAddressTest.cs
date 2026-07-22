using Brawndo_Components.Models;
using WhatPlantsCrave.Infrastructure.Repositories.InMemory;

namespace WhatPlantsCraveTests.InMemory
{
    public class CustomerAddressRepositoryTest
    {
        [Fact]
        public void Create_AddsCustomerAddressToRepository()
        {
            var repository = new InMemoryCustomerAddressRepository();
            bool created = repository.Create(1, 100, "Main Office");
            Assert.True(created);
            var result = repository.Get(1, 100);
            Assert.NotNull(result);
            Assert.Equal("Main Office", result.AddressType);
        }

        [Fact]
        public void GetAll_ReturnsMultipleEntries()
        {
            var repository = new InMemoryCustomerAddressRepository();
            repository.Create(1, 100, "Main Office");
            repository.Create(1, 200, "Shipping");
            repository.Create(2, 100, "Main Office");
            var all = repository.GetAll();
            Assert.Equal(3, all.Count);
        }

        [Fact]
        public void Update_ModifiesAddressType()
        {
            var repository = new InMemoryCustomerAddressRepository();
            repository.Create(1, 100, "Main Office");
            bool updated = repository.Update(1, 100, addressType: "Billing");
            Assert.True(updated);
            var result = repository.Get(1, 100);
            Assert.NotNull(result);
            Assert.Equal("Billing", result.AddressType);
        }

        [Fact]
        public void Delete_RemovesCustomerAddress()
        {
            var repository = new InMemoryCustomerAddressRepository();
            repository.Create(1, 100, "Main Office");
            bool deleted = repository.Delete(1, 100);
            Assert.True(deleted);
            Assert.Null(repository.Get(1, 100));
        }

        [Fact]
        public void GetByCustomer_FiltersCorrectly()
        {
            var repository = new InMemoryCustomerAddressRepository();
            repository.Create(1, 100, "Main Office");
            repository.Create(1, 200, "Shipping");
            repository.Create(2, 300, "Billing");
            var customer1Addresses = repository.GetByCustomer(1);
            Assert.Equal(2, customer1Addresses.Count);
        }
    }
}
