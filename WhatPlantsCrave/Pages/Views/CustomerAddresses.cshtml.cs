using Brawndo_Components.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhatPlantsCrave.Infrastructure.Repositories;

namespace WhatPlantsCrave.Pages.Views
{
    public class CustomerAddressesModel : PageModel
    {
        private readonly ICustomerAddressRepository _repository;

        public List<CustomerAddress> CustomerAddresses { get; set; } = new();

        public CustomerAddressesModel(ICustomerAddressRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            CustomerAddresses = _repository.GetAll();
        }
    }
}
