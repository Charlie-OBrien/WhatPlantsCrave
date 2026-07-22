using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    /// <summary>
    /// Test/mock repository implementation that stores data in memory.
    /// Use this in unit tests to avoid database dependencies.
    /// </summary>
    public class InMemoryProductCategoryRepository : IProductCategoryRepository
    {
        private readonly List<ProductCategory> _categories = new();
        private int _nextId = 1;

        public int Create(string name, int? parentProductCategoryId = null)
        {
            var category = new ProductCategory
            {
                ProductCategoryId = _nextId++,
                Name = name,
                ParentProductCategoryId = parentProductCategoryId,
                Rowguid = Guid.NewGuid(),
                ModifiedDate = DateTime.UtcNow
            };
            _categories.Add(category);
            return category.ProductCategoryId;
        }

        public bool Delete(int productCategoryId)
        {
            var category = _categories.FirstOrDefault(c => c.ProductCategoryId == productCategoryId);
            if (category == null) return false;
            _categories.Remove(category);
            return true;
        }

        public List<ProductCategory> GetAll() => _categories.ToList();

        public ProductCategory? GetByID(int productCategoryId) =>
            _categories.FirstOrDefault(c => c.ProductCategoryId == productCategoryId);

        public List<ProductCategory> GetRootCategories() =>
            _categories.Where(c => c.ParentProductCategoryId == null).ToList();

        public bool Update(int productCategoryId, string? name = null, int? parentProductCategoryId = null)
        {
            var category = _categories.FirstOrDefault(c => c.ProductCategoryId == productCategoryId);
            if (category == null) return false;

            if (name != null) category.Name = name;
            if (parentProductCategoryId != null) category.ParentProductCategoryId = parentProductCategoryId;
            category.ModifiedDate = DateTime.UtcNow;

            return true;
        }
    }
}
