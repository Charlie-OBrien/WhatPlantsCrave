using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    /// <summary>
    /// Test/mock repository implementation that stores data in memory.
    /// Use this in unit tests to avoid database dependencies.
    /// </summary>
    public class InMemorySalesOrderDetailRepository : ISalesOrderDetailRepository
    {
        private readonly List<SalesOrderDetail> _details = new();
        private int _nextId = 1;

        public int Create(int salesOrderId, int productId, short orderQty, decimal unitPrice, decimal unitPriceDiscount = 0)
        {
            var detail = new SalesOrderDetail
            {
                SalesOrderDetailId = _nextId++,
                SalesOrderId = salesOrderId,
                ProductId = productId,
                OrderQty = orderQty,
                UnitPrice = unitPrice,
                LineTotal = orderQty * unitPrice * (1 - unitPriceDiscount)
            };
            _details.Add(detail);
            return detail.SalesOrderDetailId;
        }

        public bool Delete(int salesOrderId, int salesOrderDetailId)
        {
            var detail = _details.FirstOrDefault(d => d.SalesOrderId == salesOrderId && d.SalesOrderDetailId == salesOrderDetailId);
            if (detail == null) return false;
            _details.Remove(detail);
            return true;
        }

        public SalesOrderDetail? Get(int salesOrderId, int salesOrderDetailId) =>
            _details.FirstOrDefault(d => d.SalesOrderId == salesOrderId && d.SalesOrderDetailId == salesOrderDetailId);

        public List<SalesOrderDetail> GetAll() => _details.ToList();

        public List<SalesOrderDetail> GetByOrder(int salesOrderId) =>
            _details.Where(d => d.SalesOrderId == salesOrderId).ToList();

        public bool Update(int salesOrderId, int salesOrderDetailId, short? orderQty = null, decimal? unitPrice = null)
        {
            var detail = _details.FirstOrDefault(d => d.SalesOrderId == salesOrderId && d.SalesOrderDetailId == salesOrderDetailId);
            if (detail == null) return false;

            if (orderQty != null) detail.OrderQty = (short)orderQty;
            if (unitPrice != null) detail.UnitPrice = (decimal)unitPrice;
            if (orderQty != null || unitPrice != null)
                detail.LineTotal = detail.OrderQty * detail.UnitPrice;

            return true;
        }
    }
}
