using Brawndo_Components.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhatPlantsCrave.Infrastructure.Repositories;

namespace WhatPlantsCrave.Pages.Views
{
    public class ProductModelProductDescriptionsModel : PageModel
    {
        private readonly IProductModelProductDescriptionRepository _repository;

        public List<ProductModelProductDescription> ProductModelProductDescriptions { get; set; } = new();

        public ProductModelProductDescriptionsModel(IProductModelProductDescriptionRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            ProductModelProductDescriptions = _repository.GetAll();
        }
    }
}
