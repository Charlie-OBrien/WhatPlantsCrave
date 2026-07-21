using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    public interface ISalesOrderDetailRepository
    {
        int Create(int salesOrderId, int productId, short orderQty, decimal unitPrice, decimal unitPriceDiscount = 0);
        bool Delete(int salesOrderId, int salesOrderDetailId);
        SalesOrderDetail? Get(int salesOrderId, int salesOrderDetailId);
        List<SalesOrderDetail> GetAll();
        List<SalesOrderDetail> GetByOrder(int salesOrderId);
        bool Update(int salesOrderId, int salesOrderDetailId, short? orderQty = null, decimal? unitPrice = null);
    }

    public class SalesOrderDetailRepository : ISalesOrderDetailRepository
    {
        private readonly ISalesOrderDetailService _service;

        public SalesOrderDetailRepository(ISalesOrderDetailService service)
        {
            _service = service;
        }

        public int Create(int salesOrderId, int productId, short orderQty, decimal unitPrice, decimal unitPriceDiscount = 0)
            => _service.Create(salesOrderId, productId, orderQty, unitPrice, unitPriceDiscount);

        public bool Delete(int salesOrderId, int salesOrderDetailId)
            => _service.Delete(salesOrderId, salesOrderDetailId);

        public SalesOrderDetail? Get(int salesOrderId, int salesOrderDetailId)
            => _service.Get(salesOrderId, salesOrderDetailId);

        public List<SalesOrderDetail> GetAll()
            => _service.GetAll();

        public List<SalesOrderDetail> GetByOrder(int salesOrderId)
            => _service.GetByOrder(salesOrderId);

        public bool Update(int salesOrderId, int salesOrderDetailId, short? orderQty = null, decimal? unitPrice = null)
            => _service.Update(salesOrderId, salesOrderDetailId, orderQty, unitPrice);
    }
}
