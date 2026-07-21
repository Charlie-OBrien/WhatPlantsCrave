using Brawndo_Components.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhatPlantsCrave.Infrastructure.Repositories;

namespace WhatPlantsCrave.Pages.Views
{
    public class ProductModelsModel : PageModel
    {
        private readonly IProductModelRepository _repository;

        public List<ProductModel> ProductModels { get; set; } = new();

        public ProductModelsModel(IProductModelRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            ProductModels = _repository.GetAll();
        }
    }
}
