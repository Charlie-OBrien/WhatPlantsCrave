using Brawndo_Components;
using Brawndo_Components.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace WhatPlantsCraveTests.Sql.Services
{
    public class SalesOrderDetailServiceVerificationTest : IDisposable
    {
        private readonly AdventureWorksContext _context;
        private readonly SalesOrderDetailService _service;

        public SalesOrderDetailServiceVerificationTest()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseSqlServer("Server=DESKTOP-OAJVK2J;Database=AdventureWorksLT2016;Trusted_Connection=true;TrustServerCertificate=true;")
                .Options;

            _context = new AdventureWorksContext(options);
            _service = new SalesOrderDetailService(_context);
        }

        [Fact]
        public void GetAll_ReturnsSalesOrderDetails()
        {
            var details = _service.GetAll();

            Assert.NotNull(details);
            Assert.NotEmpty(details);
            Assert.All(details, d =>
            {
                Assert.True(d.SalesOrderId > 0);
                Assert.True(d.SalesOrderDetailId > 0);
                Assert.True(d.OrderQty > 0);
                Assert.True(d.ProductId > 0);
                Assert.True(d.UnitPrice >= 0);
            });
        }

        [Fact]
        public void Get_ReturnsExistingDetail()
        {
            var all = _service.GetAll();
            Assert.NotEmpty(all);

            var known = all.First();
            var result = _service.Get(known.SalesOrderId, known.SalesOrderDetailId);

            Assert.NotNull(result);
            Assert.Equal(known.SalesOrderId, result.SalesOrderId);
            Assert.Equal(known.SalesOrderDetailId, result.SalesOrderDetailId);
        }

        [Fact]
        public void GetByOrder_ReturnsDetailsForOrder()
        {
            var all = _service.GetAll();
            Assert.NotEmpty(all);

            var knownOrderId = all.First().SalesOrderId;
            var results = _service.GetByOrder(knownOrderId);

            Assert.NotNull(results);
            Assert.NotEmpty(results);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
