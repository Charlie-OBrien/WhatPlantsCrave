using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    /// <summary>
    /// Test/mock repository implementation that stores data in memory.
    /// Use this in unit tests to avoid database dependencies.
    /// </summary>
    public class InMemoryCustomerAddressRepository : ICustomerAddressRepository
    {
        private readonly List<CustomerAddress> _customerAddresses = new();

        public bool Create(int customerId, int addressId, string addressType)
        {
            var customerAddress = new CustomerAddress
            {
                CustomerId = customerId,
                AddressId = addressId,
                AddressType = addressType,
                ModifiedDate = DateTime.UtcNow
            };
            _customerAddresses.Add(customerAddress);
            return true;
        }

        public bool Delete(int customerId, int addressId)
        {
            var customerAddress = _customerAddresses.FirstOrDefault(ca => ca.CustomerId == customerId && ca.AddressId == addressId);
            if (customerAddress == null) return false;
            _customerAddresses.Remove(customerAddress);
            return true;
        }

        public CustomerAddress? Get(int customerId, int addressId) =>
            _customerAddresses.FirstOrDefault(ca => ca.CustomerId == customerId && ca.AddressId == addressId);

        public List<CustomerAddress> GetAll() => _customerAddresses.ToList();

        public List<CustomerAddress> GetByCustomer(int customerId) =>
            _customerAddresses.Where(ca => ca.CustomerId == customerId).ToList();

        public bool Update(int customerId, int addressId, string? addressType = null)
        {
            var customerAddress = _customerAddresses.FirstOrDefault(ca => ca.CustomerId == customerId && ca.AddressId == addressId);
            if (customerAddress == null) return false;

            if (addressType != null) customerAddress.AddressType = addressType;
            customerAddress.ModifiedDate = DateTime.UtcNow;

            return true;
        }
    }
}
