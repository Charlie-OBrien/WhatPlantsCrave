using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories.InMemory
{
    /// <summary>
    /// Test/mock repository implementation that stores data in memory.
    /// Use this in unit tests to avoid database dependencies.
    /// </summary>
    public class InMemoryProductDescriptionRepository : IProductDescriptionRepository
    {
        private readonly List<ProductDescription> _descriptions = new();
        private int _nextId = 1;

        public int Create(string description)
        {
            var desc = new ProductDescription
            {
                ProductDescriptionId = _nextId++,
                Description = description,
                ModifiedDate = DateTime.UtcNow
            };
            _descriptions.Add(desc);
            return desc.ProductDescriptionId;
        }

        public bool Delete(int productDescriptionId)
        {
            var desc = _descriptions.FirstOrDefault(d => d.ProductDescriptionId == productDescriptionId);
            if (desc == null) return false;
            _descriptions.Remove(desc);
            return true;
        }

        public List<ProductDescription> GetAll() => _descriptions.ToList();

        public ProductDescription? GetByID(int productDescriptionId) =>
            _descriptions.FirstOrDefault(d => d.ProductDescriptionId == productDescriptionId);

        public bool Update(int productDescriptionId, string? description = null)
        {
            var desc = _descriptions.FirstOrDefault(d => d.ProductDescriptionId == productDescriptionId);
            if (desc == null) return false;

            if (description != null) desc.Description = description;
            desc.ModifiedDate = DateTime.UtcNow;

            return true;
        }
    }
}
