using Brawndo_Components.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhatPlantsCrave.Infrastructure.Repositories;

namespace WhatPlantsCrave.Pages.Views
{
    public class ProductDescriptionsModel : PageModel
    {
        private readonly IProductDescriptionRepository _repository;

        public List<ProductDescription> ProductDescriptions { get; set; } = new();

        public ProductDescriptionsModel(IProductDescriptionRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            ProductDescriptions = _repository.GetAll();
        }
    }
}
