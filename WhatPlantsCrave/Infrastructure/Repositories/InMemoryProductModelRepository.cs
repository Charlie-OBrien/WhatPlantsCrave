using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    /// <summary>
    /// Test/mock repository implementation that stores data in memory.
    /// Use this in unit tests to avoid database dependencies.
    /// </summary>
    public class InMemoryProductModelRepository : IProductModelRepository
    {
        private readonly List<ProductModel> _models = new();
        private int _nextId = 1;

        public int Create(string name, string? catalogDescription = null)
        {
            var model = new ProductModel
            {
                ProductModelId = _nextId++,
                Name = name,
                CatalogDescription = catalogDescription,
                ModifiedDate = DateTime.UtcNow
            };
            _models.Add(model);
            return model.ProductModelId;
        }

        public bool Delete(int productModelId)
        {
            var model = _models.FirstOrDefault(m => m.ProductModelId == productModelId);
            if (model == null) return false;
            _models.Remove(model);
            return true;
        }

        public List<ProductModel> GetAll() => _models.ToList();

        public ProductModel? GetByID(int productModelId) =>
            _models.FirstOrDefault(m => m.ProductModelId == productModelId);
    }
}
