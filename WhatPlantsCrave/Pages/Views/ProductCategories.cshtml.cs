using Brawndo_Components.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhatPlantsCrave.Infrastructure.Repositories;

namespace WhatPlantsCrave.Pages.Views
{
    public class ProductCategoriesModel : PageModel
    {
        private readonly IProductCategoryRepository _repository;

        public List<ProductCategory> ProductCategories { get; set; } = new();

        public ProductCategoriesModel(IProductCategoryRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            ProductCategories = _repository.GetAll();
        }
    }
}
