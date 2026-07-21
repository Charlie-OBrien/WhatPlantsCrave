using Brawndo_Components.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WhatPlantsCrave.Infrastructure.Repositories;

namespace WhatPlantsCrave.Pages.Views
{
    public class AddressesModel : PageModel
    {
        private readonly IAddressRepository _repository;

        public List<Address> Addresses { get; set; } = new();

        public AddressesModel(IAddressRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            Addresses = _repository.GetAll();
        }
    }
}
