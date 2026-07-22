using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    /// <summary>
    /// Test/mock repository implementation that stores data in memory.
    /// Use this in unit tests to avoid database dependencies.
    /// </summary>
    public class InMemoryAddressRepository : IAddressRepository
    {
        private readonly List<Address> _addresses = new();
        private int _nextId = 1;

        public int Create(string addressLine1, string city, string stateProvince, string countryRegion, string postalCode, string? addressLine2 = null)
        {
            var address = new Address
            {
                AddressId = _nextId++,
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                City = city,
                StateProvince = stateProvince,
                CountryRegion = countryRegion,
                PostalCode = postalCode,
                Rowguid = Guid.NewGuid(),
                ModifiedDate = DateTime.UtcNow
            };
            _addresses.Add(address);
            return address.AddressId;
        }

        public bool Delete(int addressId)
        {
            var address = _addresses.FirstOrDefault(a => a.AddressId == addressId);
            if (address == null) return false;
            _addresses.Remove(address);
            return true;
        }

        public List<Address> GetAll() => _addresses.ToList();

        public List<Address> GetByCountryRegion(string countryRegion) =>
            _addresses.Where(a => a.CountryRegion == countryRegion).ToList();

        public Address? GetByID(int addressId) => _addresses.FirstOrDefault(a => a.AddressId == addressId);

        public List<Address> SearchByCity(string city) =>
            _addresses.Where(a => a.City.Contains(city, StringComparison.OrdinalIgnoreCase)).ToList();

        public bool Update(int addressId, string? addressLine1 = null, string? city = null, string? stateProvince = null, string? postalCode = null)
        {
            var address = _addresses.FirstOrDefault(a => a.AddressId == addressId);
            if (address == null) return false;

            if (addressLine1 != null) address.AddressLine1 = addressLine1;
            if (city != null) address.City = city;
            if (stateProvince != null) address.StateProvince = stateProvince;
            if (postalCode != null) address.PostalCode = postalCode;
            address.ModifiedDate = DateTime.UtcNow;

            return true;
        }
    }
}
