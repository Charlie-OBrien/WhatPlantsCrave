using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories.InMemory
{
    /// <summary>
    /// Test/mock repository implementation that stores data in memory.
    /// Use this in unit tests to avoid database dependencies.
    /// </summary>
    public class InMemorySalesOrderHeaderRepository : ISalesOrderHeaderRepository
    {
        private readonly List<SalesOrderHeader> _headers = new();
        private int _nextId = 1;

        public int Create(byte revisionNumber, DateTime orderDate, DateTime dueDate, int customerId, string shipMethod, decimal subTotal, decimal taxAmt, decimal freight)
        {
            var header = new SalesOrderHeader
            {
                SalesOrderId = _nextId++,
                SalesOrderNumber = $"SO{_nextId:D8}",
                OrderDate = orderDate,
                DueDate = dueDate,
                Status = 1,
                CustomerId = customerId,
                SubTotal = subTotal,
                TaxAmt = taxAmt,
                Freight = freight,
                TotalDue = subTotal + taxAmt + freight,
                ModifiedDate = DateTime.UtcNow
            };
            _headers.Add(header);
            return header.SalesOrderId;
        }

        public bool Delete(int salesOrderId)
        {
            var header = _headers.FirstOrDefault(h => h.SalesOrderId == salesOrderId);
            if (header == null) return false;
            _headers.Remove(header);
            return true;
        }

        public List<SalesOrderHeader> GetAll(int limit = 100) =>
            _headers.Take(limit).ToList();

        public List<SalesOrderHeader> GetByCustomer(int customerId) =>
            _headers.Where(h => h.CustomerId == customerId).ToList();

        public SalesOrderHeader? GetByID(int salesOrderId) =>
            _headers.FirstOrDefault(h => h.SalesOrderId == salesOrderId);

        public bool Update(int salesOrderId, byte? status = null, DateTime? shipDate = null)
        {
            var header = _headers.FirstOrDefault(h => h.SalesOrderId == salesOrderId);
            if (header == null) return false;

            if (status != null) header.Status = (byte)status;
            if (shipDate != null) header.ShipDate = (DateTime)shipDate;
            header.ModifiedDate = DateTime.UtcNow;

            return true;
        }
    }
}
