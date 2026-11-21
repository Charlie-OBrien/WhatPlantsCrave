using Brawndo_Components;
using Brawndo_Components.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace WhatPlantsCraveTests.Sql.Services
{
    public class CustomerAddressServiceVerificationTest : IDisposable
    {
        private readonly AdventureWorksContext _context;
        private readonly CustomerAddressService _service;

        public CustomerAddressServiceVerificationTest()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseSqlServer("Server=DESKTOP-OAJVK2J;Database=AdventureWorksLT2016;Trusted_Connection=true;TrustServerCertificate=true;")
                .Options;

            _context = new AdventureWorksContext(options);
            _service = new CustomerAddressService(_context);
        }

        [Fact]
        public void GetAll_ReturnsCustomerAddresses()
        {
            var customerAddresses = _service.GetAll();

            Assert.NotNull(customerAddresses);
            Assert.NotEmpty(customerAddresses);
            Assert.All(customerAddresses, ca =>
            {
                Assert.True(ca.CustomerId > 0);
                Assert.True(ca.AddressId > 0);
                Assert.False(string.IsNullOrEmpty(ca.AddressType));
            });
        }

        [Fact]
        public void Get_ReturnsExistingCustomerAddress()
        {
            var all = _service.GetAll();
            Assert.NotEmpty(all);

            var known = all.First();
            var result = _service.Get(known.CustomerId, known.AddressId);

            Assert.NotNull(result);
            Assert.Equal(known.CustomerId, result.CustomerId);
            Assert.Equal(known.AddressId, result.AddressId);
        }

        [Fact]
        public void GetByCustomer_ReturnsAddressesForCustomer()
        {
            var all = _service.GetAll();
            Assert.NotEmpty(all);

            var knownCustomerId = all.First().CustomerId;
            var results = _service.GetByCustomer(knownCustomerId);

            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.All(results, ca => Assert.Equal(knownCustomerId, ca.CustomerId));
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
