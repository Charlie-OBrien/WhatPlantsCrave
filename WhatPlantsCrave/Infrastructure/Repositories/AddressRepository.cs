using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    public interface IAddressRepository
    {
        int Create(string addressLine1, string city, string stateProvince, string countryRegion, string postalCode, string? addressLine2 = null);
        bool Delete(int addressId);
        List<Address> GetAll();
        List<Address> GetByCountryRegion(string countryRegion);
        Address? GetByID(int addressId);
        List<Address> SearchByCity(string city);
        bool Update(int addressId, string? addressLine1 = null, string? city = null, string? stateProvince = null, string? postalCode = null);
    }

    public class AddressRepository : IAddressRepository
    {
        private readonly IAddressService _service;

        public AddressRepository(IAddressService service)
        {
            _service = service;
        }

        public int Create(string addressLine1, string city, string stateProvince, string countryRegion, string postalCode, string? addressLine2 = null)
            => _service.Create(addressLine1, city, stateProvince, countryRegion, postalCode, addressLine2);

        public bool Delete(int addressId)
            => _service.Delete(addressId);

        public List<Address> GetAll()
            => _service.GetAll();

        public List<Address> GetByCountryRegion(string countryRegion)
            => _service.GetByCountryRegion(countryRegion);

        public Address? GetByID(int addressId)
            => _service.GetByID(addressId);

        public List<Address> SearchByCity(string city)
            => _service.SearchByCity(city);

        public bool Update(int addressId, string? addressLine1 = null, string? city = null, string? stateProvince = null, string? postalCode = null)
            => _service.Update(addressId, addressLine1, city, stateProvince, postalCode);
    }
}
