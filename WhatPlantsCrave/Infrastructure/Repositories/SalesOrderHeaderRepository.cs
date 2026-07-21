using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    public interface ISalesOrderHeaderRepository
    {
        int Create(byte revisionNumber, DateTime orderDate, DateTime dueDate, int customerId, string shipMethod, decimal subTotal, decimal taxAmt, decimal freight);
        bool Delete(int salesOrderId);
        List<SalesOrderHeader> GetAll(int limit = 100);
        List<SalesOrderHeader> GetByCustomer(int customerId);
        SalesOrderHeader? GetByID(int salesOrderId);
        bool Update(int salesOrderId, byte? status = null, DateTime? shipDate = null);
    }

    public class SalesOrderHeaderRepository : ISalesOrderHeaderRepository
    {
        private readonly ISalesOrderHeaderService _service;

        public SalesOrderHeaderRepository(ISalesOrderHeaderService service)
        {
            _service = service;
        }

        public int Create(byte revisionNumber, DateTime orderDate, DateTime dueDate, int customerId, string shipMethod, decimal subTotal, decimal taxAmt, decimal freight)
            => _service.Create(revisionNumber, orderDate, dueDate, customerId, shipMethod, subTotal, taxAmt, freight);

        public bool Delete(int salesOrderId)
            => _service.Delete(salesOrderId);

        public List<SalesOrderHeader> GetAll(int limit = 100)
            => _service.GetAll(limit);

        public List<SalesOrderHeader> GetByCustomer(int customerId)
            => _service.GetByCustomer(customerId);

        public SalesOrderHeader? GetByID(int salesOrderId)
            => _service.GetByID(salesOrderId);

        public bool Update(int salesOrderId, byte? status = null, DateTime? shipDate = null)
            => _service.Update(salesOrderId, status, shipDate);
    }
}
