using Brawndo_Components.Models;
using WhatPlantsCrave.Infrastructure.Repositories.InMemory;

namespace WhatPlantsCraveTests.InMemory
{
    public class SalesOrderHeaderRepositoryTest
    {
        [Fact]
        public void Create_AddsOrderToRepository()
        {
            var repository = new InMemorySalesOrderHeaderRepository();
            int id = repository.Create(
                revisionNumber: 1,
                orderDate: DateTime.Now,
                dueDate: DateTime.Now.AddDays(14),
                customerId: 100,
                shipMethod: "CARGO TRANSPORT 5",
                subTotal: 500m,
                taxAmt: 40m,
                freight: 12.50m
            );
            Assert.True(id > 0);
            var order = repository.GetByID(id);
            Assert.NotNull(order);
            Assert.Equal(100, order.CustomerId);
            Assert.Equal(500m, order.SubTotal);
        }

        [Fact]
        public void Create_CalculatesTotalDue()
        {
            var repository = new InMemorySalesOrderHeaderRepository();
            int id = repository.Create(1, DateTime.Now, DateTime.Now.AddDays(14), 100, "CARGO", 500m, 40m, 10m);
            var order = repository.GetByID(id);
            Assert.NotNull(order);
            Assert.Equal(550m, order.TotalDue);
        }

        [Fact]
        public void GetAll_ReturnsMultipleOrders()
        {
            var repository = new InMemorySalesOrderHeaderRepository();
            repository.Create(1, DateTime.Now, DateTime.Now.AddDays(14), 100, "CARGO", 500m, 40m, 10m);
            repository.Create(1, DateTime.Now, DateTime.Now.AddDays(7), 200, "SHIP", 300m, 24m, 8m);
            var orders = repository.GetAll();
            Assert.Equal(2, orders.Count);
        }

        [Fact]
        public void Update_ModifiesOrderStatus()
        {
            var repository = new InMemorySalesOrderHeaderRepository();
            int id = repository.Create(1, DateTime.Now, DateTime.Now.AddDays(14), 100, "CARGO", 500m, 40m, 10m);
            bool updated = repository.Update(id, status: 5, shipDate: DateTime.Now);
            Assert.True(updated);
            var order = repository.GetByID(id);
            Assert.NotNull(order);
            Assert.Equal(5, order.Status);
        }

        [Fact]
        public void Delete_RemovesOrder()
        {
            var repository = new InMemorySalesOrderHeaderRepository();
            int id = repository.Create(1, DateTime.Now, DateTime.Now.AddDays(14), 100, "CARGO", 500m, 40m, 10m);
            bool deleted = repository.Delete(id);
            Assert.True(deleted);
            Assert.Null(repository.GetByID(id));
        }

        [Fact]
        public void GetByCustomer_FiltersCorrectly()
        {
            var repository = new InMemorySalesOrderHeaderRepository();
            repository.Create(1, DateTime.Now, DateTime.Now.AddDays(14), 100, "CARGO", 500m, 40m, 10m);
            repository.Create(1, DateTime.Now, DateTime.Now.AddDays(7), 200, "SHIP", 300m, 24m, 8m);
            repository.Create(1, DateTime.Now, DateTime.Now.AddDays(10), 100, "CARGO", 150m, 12m, 5m);
            var customer100Orders = repository.GetByCustomer(100);
            Assert.Equal(2, customer100Orders.Count);
        }
    }
}
