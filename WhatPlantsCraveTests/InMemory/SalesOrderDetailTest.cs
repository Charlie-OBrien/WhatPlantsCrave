using Brawndo_Components.Models;
using WhatPlantsCrave.Infrastructure.Repositories.InMemory;

namespace WhatPlantsCraveTests.InMemory
{
    public class SalesOrderDetailRepositoryTest
    {
        [Fact]
        public void Create_AddsDetailToRepository()
        {
            var repository = new InMemorySalesOrderDetailRepository();
            int id = repository.Create(salesOrderId: 1, productId: 100, orderQty: 5, unitPrice: 25.00m);
            Assert.True(id > 0);
            var detail = repository.Get(1, id);
            Assert.NotNull(detail);
            Assert.Equal(5, detail.OrderQty);
            Assert.Equal(25.00m, detail.UnitPrice);
        }

        [Fact]
        public void Create_CalculatesLineTotal()
        {
            var repository = new InMemorySalesOrderDetailRepository();
            int id = repository.Create(salesOrderId: 1, productId: 100, orderQty: 3, unitPrice: 10.00m);
            var detail = repository.Get(1, id);
            Assert.NotNull(detail);
            Assert.Equal(30.00m, detail.LineTotal);
        }

        [Fact]
        public void GetAll_ReturnsMultipleDetails()
        {
            var repository = new InMemorySalesOrderDetailRepository();
            repository.Create(1, 100, 2, 10m);
            repository.Create(1, 200, 3, 20m);
            repository.Create(2, 100, 1, 15m);
            var all = repository.GetAll();
            Assert.Equal(3, all.Count);
        }

        [Fact]
        public void Update_ModifiesDetailData()
        {
            var repository = new InMemorySalesOrderDetailRepository();
            int id = repository.Create(1, 100, 5, 25m);
            bool updated = repository.Update(1, id, orderQty: 10);
            Assert.True(updated);
            var detail = repository.Get(1, id);
            Assert.NotNull(detail);
            Assert.Equal(10, detail.OrderQty);
        }

        [Fact]
        public void Delete_RemovesDetail()
        {
            var repository = new InMemorySalesOrderDetailRepository();
            int id = repository.Create(1, 100, 5, 25m);
            bool deleted = repository.Delete(1, id);
            Assert.True(deleted);
            Assert.Null(repository.Get(1, id));
        }

        [Fact]
        public void GetByOrder_FiltersCorrectly()
        {
            var repository = new InMemorySalesOrderDetailRepository();
            repository.Create(1, 100, 2, 10m);
            repository.Create(1, 200, 3, 20m);
            repository.Create(2, 100, 1, 15m);
            var order1Details = repository.GetByOrder(1);
            Assert.Equal(2, order1Details.Count);
        }
    }
}
