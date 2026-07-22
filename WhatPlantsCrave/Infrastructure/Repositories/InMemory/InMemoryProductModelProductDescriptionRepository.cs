using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories.InMemory
{
    /// <summary>
    /// Test/mock repository implementation that stores data in memory.
    /// Use this in unit tests to avoid database dependencies.
    /// </summary>
    public class InMemoryProductModelProductDescriptionRepository : IProductModelProductDescriptionRepository
    {
        private readonly List<ProductModelProductDescription> _descriptions = new();

        public bool Create(int productModelId, int productDescriptionId, string culture)
        {
            var desc = new ProductModelProductDescription
            {
                ProductModelId = productModelId,
                ProductDescriptionId = productDescriptionId,
                Culture = culture,
                ModifiedDate = DateTime.UtcNow
            };
            _descriptions.Add(desc);
            return true;
        }

        public bool Delete(int productModelId, int productDescriptionId, string culture)
        {
            var desc = _descriptions.FirstOrDefault(d => 
                d.ProductModelId == productModelId && 
                d.ProductDescriptionId == productDescriptionId && 
                d.Culture == culture);
            if (desc == null) return false;
            _descriptions.Remove(desc);
            return true;
        }

        public ProductModelProductDescription? Get(int productModelId, int productDescriptionId, string culture) =>
            _descriptions.FirstOrDefault(d => 
                d.ProductModelId == productModelId && 
                d.ProductDescriptionId == productDescriptionId && 
                d.Culture == culture);

        public List<ProductModelProductDescription> GetAll() => _descriptions.ToList();

        public bool Update(int productModelId, int productDescriptionId, string cultureOld, string cultureNew)
        {
            var desc = _descriptions.FirstOrDefault(d => 
                d.ProductModelId == productModelId && 
                d.ProductDescriptionId == productDescriptionId && 
                d.Culture == cultureOld);
            if (desc == null) return false;

            desc.Culture = cultureNew;
            desc.ModifiedDate = DateTime.UtcNow;

            return true;
        }
    }
}
