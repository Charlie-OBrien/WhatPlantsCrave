using Brawndo_Components.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhatPlantsCrave.Infrastructure.Repositories;

namespace WhatPlantsCrave.Pages.Views
{
    public class CustomersModel : PageModel
    {
        private readonly ICustomerRepository _repository;

        public List<Customer> Customers { get; set; } = new();

        public CustomersModel(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            Customers = _repository.GetAll();
        }
    }
}
