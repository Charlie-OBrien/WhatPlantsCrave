using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories.InMemory
{
    /// <summary>
    /// Test/mock repository implementation that stores data in memory.
    /// Use this in unit tests to avoid database dependencies.
    /// </summary>
    public class InMemoryProductRepository : IProductRepository
    {
        private readonly List<Product> _products = new();
        private int _nextId = 1;

        public int Create(string name, string productNumber, decimal standardCost, decimal listPrice, DateTime sellStartDate, string? color = null, string? size = null, decimal? weight = null, int? productCategoryId = null, int? productModelId = null, DateTime? sellEndDate = null, DateTime? discontinuedDate = null, string? thumbnailPhotoFileName = null)
        {
            var product = new Product
            {
                ProductId = _nextId++,
                Name = name,
                ProductNumber = productNumber,
                StandardCost = standardCost,
                ListPrice = listPrice,
                SellStartDate = sellStartDate,
                Color = color,
                Size = size,
                Weight = weight,
                ProductCategoryId = productCategoryId,
                ProductModelId = productModelId,
                SellEndDate = sellEndDate,
                DiscontinuedDate = discontinuedDate,
                ThumbnailPhotoFileName = thumbnailPhotoFileName,
                Rowguid = Guid.NewGuid(),
                ModifiedDate = DateTime.UtcNow
            };
            _products.Add(product);
            return product.ProductId;
        }

        public bool Delete(int productId)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null) return false;
            _products.Remove(product);
            return true;
        }

        public List<Product> GetActive()
        {
            return _products.Where(p => p.SellEndDate == null && p.DiscontinuedDate == null).ToList();
        }

        public List<Product> GetAll() => _products.ToList();

        public Product? GetByID(int productId) => _products.FirstOrDefault(p => p.ProductId == productId);

        public List<Product> SearchByName(string searchTerm) =>
            _products.Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

        public bool Update(int productId, string? name = null, decimal? standardCost = null, decimal? listPrice = null, string? color = null)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null) return false;

            if (name != null) product.Name = name;
            if (standardCost != null) product.StandardCost = (decimal)standardCost;
            if (listPrice != null) product.ListPrice = (decimal)listPrice;
            if (color != null) product.Color = color;
            product.ModifiedDate = DateTime.UtcNow;

            return true;
        }
    }
}
