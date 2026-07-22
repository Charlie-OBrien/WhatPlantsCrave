using Brawndo_Components.Models;
using WhatPlantsCrave.Infrastructure.Repositories.InMemory;

namespace WhatPlantsCraveTests.InMemory
{
    public class CustomerRepositoryTest
    {
        [Fact]
        public void Create_AddsCustomerToRepository()
        {
            var repository = new InMemoryCustomerRepository();
            int id = repository.Create(true, "John", "Doe", "hash123", "salt456");
            Assert.True(id > 0);
            var customer = repository.GetByID(id);
            Assert.NotNull(customer);
            Assert.Equal("John", customer.FirstName);
            Assert.Equal("Doe", customer.LastName);
        }

        [Fact]
        public void GetAll_ReturnsMultipleCustomers()
        {
            var repository = new InMemoryCustomerRepository();
            repository.Create(true, "John", "Doe", "hash1", "salt1");
            repository.Create(false, "Jane", "Smith", "hash2", "salt2");
            var customers = repository.GetAll();
            Assert.Equal(2, customers.Count);
        }

        [Fact]
        public void Update_ModifiesCustomerData()
        {
            var repository = new InMemoryCustomerRepository();
            int id = repository.Create(true, "John", "Doe", "hash1", "salt1");
            bool updated = repository.Update(id, firstName: "Jonathan", emailAddress: "john@test.com");
            Assert.True(updated);
            var customer = repository.GetByID(id);
            Assert.NotNull(customer);
            Assert.Equal("Jonathan", customer.FirstName);
            Assert.Equal("john@test.com", customer.EmailAddress);
        }

        [Fact]
        public void Delete_RemovesCustomer()
        {
            var repository = new InMemoryCustomerRepository();
            int id = repository.Create(true, "John", "Doe", "hash1", "salt1");
            bool deleted = repository.Delete(id);
            Assert.True(deleted);
            Assert.Null(repository.GetByID(id));
        }

        [Fact]
        public void GetByEmail_FindsCustomer()
        {
            var repository = new InMemoryCustomerRepository();
            repository.Create(true, "John", "Doe", "hash1", "salt1", emailAddress: "john@test.com");
            var customer = repository.GetByEmail("john@test.com");
            Assert.NotNull(customer);
            Assert.Equal("John", customer.FirstName);
        }

        [Fact]
        public void SearchByName_FindsMatches()
        {
            var repository = new InMemoryCustomerRepository();
            repository.Create(true, "John", "Doe", "hash1", "salt1");
            repository.Create(false, "Jane", "Smith", "hash2", "salt2");
            var results = repository.SearchByName("John");
            Assert.Single(results);
        }
    }
}
