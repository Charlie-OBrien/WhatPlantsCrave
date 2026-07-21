using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    public interface IProductModelRepository
    {
        int Create(string name, string? catalogDescription = null);
        bool Delete(int productModelId);
        List<ProductModel> GetAll();
        ProductModel? GetByID(int productModelId);
    }

    public class ProductModelRepository : IProductModelRepository
    {
        private readonly IProductModelService _service;

        public ProductModelRepository(IProductModelService service)
        {
            _service = service;
        }

        public int Create(string name, string? catalogDescription = null)
            => _service.Create(name, catalogDescription);

        public bool Delete(int productModelId)
            => _service.Delete(productModelId);

        public List<ProductModel> GetAll()
            => _service.GetAll();

        public ProductModel? GetByID(int productModelId)
            => _service.GetByID(productModelId);
    }
}
