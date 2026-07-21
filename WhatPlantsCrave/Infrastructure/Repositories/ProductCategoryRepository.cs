using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    public interface IProductCategoryRepository
    {
        int Create(string name, int? parentProductCategoryId = null);
        bool Delete(int productCategoryId);
        List<ProductCategory> GetAll();
        ProductCategory? GetByID(int productCategoryId);
        List<ProductCategory> GetRootCategories();
        bool Update(int productCategoryId, string? name = null, int? parentProductCategoryId = null);
    }

    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly IProductCategoryService _service;

        public ProductCategoryRepository(IProductCategoryService service)
        {
            _service = service;
        }

        public int Create(string name, int? parentProductCategoryId = null)
            => _service.Create(name, parentProductCategoryId);

        public bool Delete(int productCategoryId)
            => _service.Delete(productCategoryId);

        public List<ProductCategory> GetAll()
            => _service.GetAll();

        public ProductCategory? GetByID(int productCategoryId)
            => _service.GetByID(productCategoryId);

        public List<ProductCategory> GetRootCategories()
            => _service.GetRootCategories();

        public bool Update(int productCategoryId, string? name = null, int? parentProductCategoryId = null)
            => _service.Update(productCategoryId, name, parentProductCategoryId);
    }
}
