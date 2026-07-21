using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    public interface ICustomerAddressRepository
    {
        bool Create(int customerId, int addressId, string addressType);
        bool Delete(int customerId, int addressId);
        CustomerAddress? Get(int customerId, int addressId);
        List<CustomerAddress> GetAll();
        List<CustomerAddress> GetByCustomer(int customerId);
        bool Update(int customerId, int addressId, string? addressType = null);
    }

    public class CustomerAddressRepository : ICustomerAddressRepository
    {
        private readonly ICustomerAddressService _service;

        public CustomerAddressRepository(ICustomerAddressService service)
        {
            _service = service;
        }

        public bool Create(int customerId, int addressId, string addressType)
            => _service.Create(customerId, addressId, addressType);

        public bool Delete(int customerId, int addressId)
            => _service.Delete(customerId, addressId);

        public CustomerAddress? Get(int customerId, int addressId)
            => _service.Get(customerId, addressId);

        public List<CustomerAddress> GetAll()
            => _service.GetAll();

        public List<CustomerAddress> GetByCustomer(int customerId)
            => _service.GetByCustomer(customerId);

        public bool Update(int customerId, int addressId, string? addressType = null)
            => _service.Update(customerId, addressId, addressType);
    }
}
