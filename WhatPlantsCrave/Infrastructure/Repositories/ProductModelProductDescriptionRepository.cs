using Brawndo_Components;
using Brawndo_Components.Models;

namespace WhatPlantsCrave.Infrastructure.Repositories
{
    public interface IProductModelProductDescriptionRepository
    {
        bool Create(int productModelId, int productDescriptionId, string culture);
        bool Delete(int productModelId, int productDescriptionId, string culture);
        ProductModelProductDescription? Get(int productModelId, int productDescriptionId, string culture);
        List<ProductModelProductDescription> GetAll();
        bool Update(int productModelId, int productDescriptionId, string cultureOld, string cultureNew);
    }

    public class ProductModelProductDescriptionRepository : IProductModelProductDescriptionRepository
    {
        private readonly IProductModelProductDescriptionService _service;

        public ProductModelProductDescriptionRepository(IProductModelProductDescriptionService service)
        {
            _service = service;
        }

        public bool Create(int productModelId, int productDescriptionId, string culture)
            => _service.Create(productModelId, productDescriptionId, culture);

        public bool Delete(int productModelId, int productDescriptionId, string culture)
            => _service.Delete(productModelId, productDescriptionId, culture);

        public ProductModelProductDescription? Get(int productModelId, int productDescriptionId, string culture)
            => _service.Get(productModelId, productDescriptionId, culture);

        public List<ProductModelProductDescription> GetAll()
            => _service.GetAll();

        public bool Update(int productModelId, int productDescriptionId, string cultureOld, string cultureNew)
            => _service.Update(productModelId, productDescriptionId, cultureOld, cultureNew);
    }
}
