using Brawndo_Components;
using Brawndo_Components.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace WhatPlantsCraveTests.Sql.Services
{
    public class ProductModelProductDescriptionServiceVerificationTest : IDisposable
    {
        private readonly AdventureWorksContext _context;
        private readonly ProductModelProductDescriptionService _service;

        public ProductModelProductDescriptionServiceVerificationTest()
        {
            var options = new DbContextOptionsBuilder<AdventureWorksContext>()
                .UseSqlServer("Server=DESKTOP-OAJVK2J;Database=AdventureWorksLT2016;Trusted_Connection=true;TrustServerCertificate=true;")
                .Options;

            _context = new AdventureWorksContext(options);
            _service = new ProductModelProductDescriptionService(_context);
        }

        [Fact]
        public void GetAll_ReturnsRecords()
        {
            var records = _service.GetAll();

            Assert.NotNull(records);
            Assert.NotEmpty(records);
            Assert.All(records, r =>
            {
                Assert.True(r.ProductModelId > 0);
                Assert.True(r.ProductDescriptionId > 0);
                Assert.False(string.IsNullOrEmpty(r.Culture));
            });
        }

        [Fact]
        public void Get_ReturnsExistingRecord()
        {
            var all = _service.GetAll();
            Assert.NotEmpty(all);

            var known = all.First();
            var result = _service.Get(known.ProductModelId, known.ProductDescriptionId, known.Culture);

            Assert.NotNull(result);
            Assert.Equal(known.ProductModelId, result.ProductModelId);
            Assert.Equal(known.ProductDescriptionId, result.ProductDescriptionId);
            Assert.Equal(known.Culture, result.Culture);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
