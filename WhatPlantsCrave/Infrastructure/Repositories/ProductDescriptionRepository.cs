using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    public interface IProductDescriptionRepository
    {
        int Create(string description);
        bool Delete(int productDescriptionId);
        List<ProductDescription> GetAll();
        ProductDescription? GetByID(int productDescriptionId);
        bool Update(int productDescriptionId, string? description = null);
    }

    public class ProductDescriptionRepository : IProductDescriptionRepository
    {
        private readonly IProductDescriptionService _service;

        public ProductDescriptionRepository(IProductDescriptionService service)
        {
            _service = service;
        }

        public int Create(string description)
            => _service.Create(description);

        public bool Delete(int productDescriptionId)
            => _service.Delete(productDescriptionId);

        public List<ProductDescription> GetAll()
            => _service.GetAll();

        public ProductDescription? GetByID(int productDescriptionId)
            => _service.GetByID(productDescriptionId);

        public bool Update(int productDescriptionId, string? description = null)
            => _service.Update(productDescriptionId, description);
    }
}
