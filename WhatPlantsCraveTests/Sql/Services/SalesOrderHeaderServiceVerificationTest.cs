using Brawndo_Components;
using Brawndo_Components.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace WhatPlantsCraveTests.Sql.Services
{
    public class SalesOrderHeaderServiceVerificationTest : IDisposable
    {
        private readonly AdventureWorksContext _context;
        private readonly SalesOrderHeaderService _service;

        public SalesOrderHeaderServiceVerificationTest()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseSqlServer("Server=DESKTOP-OAJVK2J;Database=AdventureWorksLT2016;Trusted_Connection=true;TrustServerCertificate=true;")
                .Options;

            _context = new AdventureWorksContext(options);
            _service = new SalesOrderHeaderService(_context);
        }

        [Fact]
        public void GetAll_ReturnsSalesOrderHeaders()
        {
            var orders = _service.GetAll();

            Assert.NotNull(orders);
            Assert.NotEmpty(orders);
            Assert.All(orders, o =>
            {
                Assert.True(o.SalesOrderId > 0);
                Assert.True(o.CustomerId > 0);
                Assert.True(o.TotalDue >= 0);
            });
        }

        [Fact]
        public void GetByID_ReturnsExistingOrder()
        {
            var all = _service.GetAll();
            Assert.NotEmpty(all);

            var knownId = all.First().SalesOrderId;
            var order = _service.GetByID(knownId);

            Assert.NotNull(order);
            Assert.Equal(knownId, order.SalesOrderId);
            Assert.True(order.CustomerId > 0);
            Assert.True(order.TotalDue >= 0);
        }

        [Fact]
        public void GetByID_ReturnsNullForNonExistentOrder()
        {
            var order = _service.GetByID(999999);

            Assert.Null(order);
        }

        [Fact]
        public void GetByCustomer_ReturnsOrdersForCustomer()
        {
            var all = _service.GetAll();
            Assert.NotEmpty(all);

            var knownCustomerId = all.First().CustomerId;
            var results = _service.GetByCustomer(knownCustomerId);

            Assert.NotNull(results);
            Assert.NotEmpty(results);
            Assert.All(results, o => Assert.Equal(knownCustomerId, o.CustomerId));
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
