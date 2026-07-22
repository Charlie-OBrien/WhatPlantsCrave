using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    /// <summary>
    /// Test/mock repository implementation that stores data in memory.
    /// Use this in unit tests to avoid database dependencies.
    /// </summary>
    public class InMemoryCustomerRepository : ICustomerRepository
    {
        private readonly List<Customer> _customers = new();
        private int _nextId = 1;

        public int Create(bool nameStyle, string firstName, string lastName, string passwordHash, string passwordSalt, string? title = null, string? middleName = null, string? emailAddress = null, string? phone = null, string? companyName = null)
        {
            var customer = new Customer
            {
                CustomerId = _nextId++,
                NameStyle = nameStyle,
                Title = title,
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                CompanyName = companyName,
                EmailAddress = emailAddress,
                Phone = phone,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                Rowguid = Guid.NewGuid(),
                ModifiedDate = DateTime.UtcNow
            };
            _customers.Add(customer);
            return customer.CustomerId;
        }

        public bool Delete(int customerId)
        {
            var customer = _customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer == null) return false;
            _customers.Remove(customer);
            return true;
        }

        public List<Customer> GetAll() => _customers.ToList();

        public Customer? GetByEmail(string emailAddress) =>
            _customers.FirstOrDefault(c => c.EmailAddress == emailAddress);

        public Customer? GetByID(int customerId) =>
            _customers.FirstOrDefault(c => c.CustomerId == customerId);

        public List<Customer> SearchByName(string searchTerm) =>
            _customers.Where(c => c.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) || 
                                   c.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

        public bool Update(int customerId, string? firstName = null, string? lastName = null, string? emailAddress = null, string? phone = null)
        {
            var customer = _customers.FirstOrDefault(c => c.CustomerId == customerId);
            if (customer == null) return false;

            if (firstName != null) customer.FirstName = firstName;
            if (lastName != null) customer.LastName = lastName;
            if (emailAddress != null) customer.EmailAddress = emailAddress;
            if (phone != null) customer.Phone = phone;
            customer.ModifiedDate = DateTime.UtcNow;

            return true;
        }
    }
}
