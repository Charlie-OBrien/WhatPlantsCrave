using Brawndo_Components.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhatPlantsCrave.Infrastructure.Repositories;

namespace WhatPlantsCrave.Pages.Views
{
    public class ProductsModel : PageModel
    {
        private readonly IProductRepository _repository;

        public List<Product> Products { get; set; } = new();

        public ProductsModel(IProductRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            Products = _repository.GetAll();
        }
    }
}
