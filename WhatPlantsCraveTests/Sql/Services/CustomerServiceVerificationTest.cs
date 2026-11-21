using Brawndo_Components;
using Brawndo_Components.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace WhatPlantsCraveTests.Sql.Services
{
    public class CustomerServiceVerificationTest : IDisposable
    {
        private readonly AdventureWorksContext _context;
        private readonly CustomerService _service;

        public CustomerServiceVerificationTest()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseSqlServer("Server=DESKTOP-OAJVK2J;Database=AdventureWorksLT2016;Trusted_Connection=true;TrustServerCertificate=true;")
                .Options;

            _context = new AdventureWorksContext(options);
            _service = new CustomerService(_context);
        }

        [Fact]
        public void GetAll_ReturnsCustomers()
        {
            var customers = _service.GetAll();

            Assert.NotNull(customers);
            Assert.NotEmpty(customers);
            Assert.All(customers, c =>
            {
                Assert.True(c.CustomerId > 0);
                Assert.False(string.IsNullOrEmpty(c.FirstName));
                Assert.False(string.IsNullOrEmpty(c.LastName));
            });
        }

        [Fact]
        public void GetByID_ReturnsExistingCustomer()
        {
            var allCustomers = _service.GetAll();
            Assert.NotEmpty(allCustomers);

            var knownId = allCustomers.First().CustomerId;
            var customer = _service.GetByID(knownId);

            Assert.NotNull(customer);
            Assert.Equal(knownId, customer.CustomerId);
            Assert.False(string.IsNullOrEmpty(customer.FirstName));
            Assert.False(string.IsNullOrEmpty(customer.LastName));
        }

        [Fact]
        public void GetByID_ReturnsNullForNonExistentCustomer()
        {
            var customer = _service.GetByID(999999);

            Assert.Null(customer);
        }

        [Fact]
        public void GetByEmail_ReturnsCustomerWithEmail()
        {
            var allCustomers = _service.GetAll();
            Assert.NotEmpty(allCustomers);

            var customerWithEmail = allCustomers.FirstOrDefault(c => !string.IsNullOrEmpty(c.EmailAddress));
            if (customerWithEmail != null)
            {
                var result = _service.GetByEmail(customerWithEmail.EmailAddress!);

                Assert.NotNull(result);
                Assert.False(string.IsNullOrEmpty(result.FirstName));
            }
        }

        [Fact]
        public void SearchByName_ReturnsMatchingCustomers()
        {
            var allCustomers = _service.GetAll();
            Assert.NotEmpty(allCustomers);

            var searchTerm = allCustomers.First().LastName[..3];
            var results = _service.SearchByName(searchTerm);

            Assert.NotNull(results);
            Assert.NotEmpty(results);
        }

        [Fact]
        public void Create_Update_Delete_FullLifecycle()
        {
            int customerId = _service.Create(
                nameStyle: false,
                firstName: "TestFirst",
                lastName: "TestLast",
                passwordHash: "L/Rlwxzp4w7RWmEgXX+/A7cXaePEPcp+KwQhl2fJL7w=",
                passwordSalt: "1KjXYs4=",
                emailAddress: "svctest@example.com",
                phone: "555-0199");

            Assert.True(customerId > 0);

            var created = _service.GetByID(customerId);
            Assert.NotNull(created);
            Assert.Equal("TestFirst", created.FirstName);
            Assert.Equal("TestLast", created.LastName);

            bool updated = _service.Update(customerId, firstName: "UpdatedFirst");
            Assert.True(updated);

            var afterUpdate = _service.GetByID(customerId);
            Assert.NotNull(afterUpdate);
            Assert.Equal("UpdatedFirst", afterUpdate.FirstName);

            bool deleted = _service.Delete(customerId);
            Assert.True(deleted);

            var afterDelete = _service.GetByID(customerId);
            Assert.Null(afterDelete);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
