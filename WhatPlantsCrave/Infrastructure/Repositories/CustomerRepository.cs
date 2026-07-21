using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    public interface ICustomerRepository
    {
        int Create(bool nameStyle, string firstName, string lastName, string passwordHash, string passwordSalt, string? title = null, string? middleName = null, string? emailAddress = null, string? phone = null, string? companyName = null);
        bool Delete(int customerId);
        List<Customer> GetAll();
        Customer? GetByEmail(string emailAddress);
        Customer? GetByID(int customerId);
        List<Customer> SearchByName(string searchTerm);
        bool Update(int customerId, string? firstName = null, string? lastName = null, string? emailAddress = null, string? phone = null);
    }

    public class CustomerRepository : ICustomerRepository
    {
        private readonly ICustomerService _service;

        public CustomerRepository(ICustomerService service)
        {
            _service = service;
        }

        public int Create(bool nameStyle, string firstName, string lastName, string passwordHash, string passwordSalt, string? title = null, string? middleName = null, string? emailAddress = null, string? phone = null, string? companyName = null)
            => _service.Create(nameStyle, firstName, lastName, passwordHash, passwordSalt, title, middleName, emailAddress, phone, companyName);

        public bool Delete(int customerId)
            => _service.Delete(customerId);

        public List<Customer> GetAll()
            => _service.GetAll();

        public Customer? GetByEmail(string emailAddress)
            => _service.GetByEmail(emailAddress);

        public Customer? GetByID(int customerId)
            => _service.GetByID(customerId);

        public List<Customer> SearchByName(string searchTerm)
            => _service.SearchByName(searchTerm);

        public bool Update(int customerId, string? firstName = null, string? lastName = null, string? emailAddress = null, string? phone = null)
            => _service.Update(customerId, firstName, lastName, emailAddress, phone);
    }
}
