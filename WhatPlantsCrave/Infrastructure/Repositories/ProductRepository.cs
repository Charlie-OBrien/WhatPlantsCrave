using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    public interface IProductRepository
    {
        int Create(string name, string productNumber, decimal standardCost, decimal listPrice, DateTime sellStartDate, string? color = null, string? size = null, decimal? weight = null, int? productCategoryId = null, int? productModelId = null, DateTime? sellEndDate = null, DateTime? discontinuedDate = null, string? thumbnailPhotoFileName = null);
        bool Delete(int productId);
        List<Product> GetActive();
        List<Product> GetAll();
        Product? GetByID(int productId);
        List<Product> SearchByName(string searchTerm);
        bool Update(int productId, string? name = null, decimal? standardCost = null, decimal? listPrice = null, string? color = null);
    }

    public class ProductRepository : IProductRepository
    {
        private readonly IProductService _service;

        public ProductRepository(IProductService service)
        {
            _service = service;
        }

        public int Create(string name, string productNumber, decimal standardCost, decimal listPrice, DateTime sellStartDate, string? color = null, string? size = null, decimal? weight = null, int? productCategoryId = null, int? productModelId = null, DateTime? sellEndDate = null, DateTime? discontinuedDate = null, string? thumbnailPhotoFileName = null)
            => _service.Create(name, productNumber, standardCost, listPrice, sellStartDate, color, size, weight, productCategoryId, productModelId, sellEndDate, discontinuedDate, thumbnailPhotoFileName);

        public bool Delete(int productId)
            => _service.Delete(productId);

        public List<Product> GetActive()
            => _service.GetActive();

        public List<Product> GetAll()
            => _service.GetAll();

        public Product? GetByID(int productId)
            => _service.GetByID(productId);

        public List<Product> SearchByName(string searchTerm)
            => _service.SearchByName(searchTerm);

        public bool Update(int productId, string? name = null, decimal? standardCost = null, decimal? listPrice = null, string? color = null)
            => _service.Update(productId, name, standardCost, listPrice, color);
    }
}
