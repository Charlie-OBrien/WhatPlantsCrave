using Brawndo_Components;
using Brawndo_Components.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace WhatPlantsCraveTests.Sql.Services
{
    public class AddressServiceVerificationTest : IDisposable
    {
        private readonly AdventureWorksContext _context;
        private readonly AddressService _service;

        public AddressServiceVerificationTest()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseSqlServer("Server=DESKTOP-OAJVK2J;Database=AdventureWorksLT2016;Trusted_Connection=true;TrustServerCertificate=true;")
                .Options;

            _context = new AdventureWorksContext(options);
            _service = new AddressService(_context);
        }

        [Fact]
        public void GetAll_ReturnsAddresses()
        {
            var addresses = _service.GetAll();

            Assert.NotNull(addresses);
            Assert.NotEmpty(addresses);
            Assert.All(addresses, a =>
            {
                Assert.True(a.AddressId > 0);
                Assert.False(string.IsNullOrEmpty(a.AddressLine1));
                Assert.False(string.IsNullOrEmpty(a.City));
                Assert.False(string.IsNullOrEmpty(a.StateProvince));
                Assert.False(string.IsNullOrEmpty(a.CountryRegion));
                Assert.False(string.IsNullOrEmpty(a.PostalCode));
            });
        }

        [Fact]
        public void GetByID_ReturnsExistingAddress()
        {
            var allAddresses = _service.GetAll();
            Assert.NotEmpty(allAddresses);

            var knownId = allAddresses.First().AddressId;
            var address = _service.GetByID(knownId);

            Assert.NotNull(address);
            Assert.Equal(knownId, address.AddressId);
            Assert.False(string.IsNullOrEmpty(address.AddressLine1));
        }

        [Fact]
        public void GetByID_ReturnsNullForNonExistentAddress()
        {
            var address = _service.GetByID(999999);

            Assert.Null(address);
        }

        [Fact]
        public void GetByCountryRegion_ReturnsAddresses()
        {
            var allAddresses = _service.GetAll();
            Assert.NotEmpty(allAddresses);

            var knownCountry = allAddresses.First().CountryRegion;
            var results = _service.GetByCountryRegion(knownCountry);

            Assert.NotNull(results);
            Assert.NotEmpty(results);
        }

        [Fact]
        public void SearchByCity_ReturnsMatchingAddresses()
        {
            var allAddresses = _service.GetAll();
            Assert.NotEmpty(allAddresses);

            var knownCity = allAddresses.First().City;
            var results = _service.SearchByCity(knownCity);

            Assert.NotNull(results);
            Assert.NotEmpty(results);
        }

        [Fact]
        public void Create_Update_Delete_FullLifecycle()
        {
            int addressId = _service.Create(
                addressLine1: "123 Test Street",
                city: "TestCity",
                stateProvince: "TestState",
                countryRegion: "United States",
                postalCode: "12345");

            Assert.True(addressId > 0);

            var created = _service.GetByID(addressId);
            Assert.NotNull(created);
            Assert.Equal("123 Test Street", created.AddressLine1);
            Assert.Equal("TestCity", created.City);

            bool updated = _service.Update(addressId, city: "UpdatedCity");
            Assert.True(updated);

            var afterUpdate = _service.GetByID(addressId);
            Assert.NotNull(afterUpdate);
            Assert.Equal("UpdatedCity", afterUpdate.City);

            bool deleted = _service.Delete(addressId);
            Assert.True(deleted);

            var afterDelete = _service.GetByID(addressId);
            Assert.Null(afterDelete);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
